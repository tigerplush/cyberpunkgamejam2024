using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script manages a complete run.
/// </summary>
public class RunManager : MonoBehaviour
{
    public UnityEvent<string> OnGameOver;
    [SerializeField]
    private List<RoomConfiguration> _roomConfigs = new List<RoomConfiguration>();
    [SerializeField]
    private ScriptableBoolAttribute _isTutorialEnabled;
    [SerializeField]
    private Element[] _allElements;
    [SerializeField]
    private RuntimeSet _movementControllerRuntimeset;

    [SerializeField]
    private RewardManager _rewardManager;
    [SerializeField]
    private Manager _partyManager;
    [SerializeField]
    private Manager _enemyManager;
    [SerializeField]
    private RoomManager _roomManager;

    [SerializeField]
    private BattleManager _battleManager;

    [SerializeField]
    private ScriptableBoolAttribute _fightEnabled;
    [SerializeField]
    private ScriptableBoolAttribute _fightButtonArmed;
    [SerializeField]
    private ScriptableBoolAttribute _skipEnabled;
    [SerializeField]
    private ScriptableBoolAttribute _skipButtonArmed;
    [SerializeField]
    private Currency _currency;

    private int _startRoom;

    private void Start()
    {
        if(_isTutorialEnabled != null && _isTutorialEnabled.Value)
        {
            //If the tutorial is enabled, we return.
            return;
        }
        StartRun();
    }

    /// <summary>
    /// Actually starts a run.
    /// </summary>
    /// <returns></returns>
    public void StartRun()
    {
        _startRoom = _roomManager.CurrentRoom;
        CollapseProbabilites();
        StartCoroutine(Run());
    }

    /// <summary>
    /// This method should take all randomness out of the configuration
    /// </summary>
    private void CollapseProbabilites()
    {
        foreach(RoomConfiguration room in _roomConfigs)
        {
            if(room.RoomType == RoomType.RecruitmentRoom)
            {
                // If it's a recruitment room, we don't need to collapse anything
                continue;
            }
            foreach(EnemyConfiguration config in room.OffensiveEnemies)
            {
                int random = Random.Range(0, config.PossibleElements.Length);
                Element element = config.PossibleElements[random];
                config.PossibleElements = new[] { element };
            }
        }
    }

    private IEnumerator Run()
    {
        int currentRoom = _roomManager.CurrentRoom - _startRoom;
        Debug.Log(currentRoom);
        if(currentRoom >= _roomConfigs.Count)
        {
            OnGameOver?.Invoke("You made it to the top of the tower!");
            yield break;
        }
        RoomConfiguration config = _roomConfigs[currentRoom];
        switch(config.RoomType)
        {
            case RoomType.RecruitmentRoom:
                yield return SpawnRecruitStage(config);
                break;
            case RoomType.FightRoom:
                yield return FightRound(config);
                break;
        }

        //Spawn next room
        yield return _roomManager.SpawnAndScrollToNextRoom();

        yield return Run();
    }

    private IEnumerator SpawnRecruitStage(RoomConfiguration config)
    {
        int currentPartySize = _partyManager.Party.Count;
        _partyManager.ResetPositions();
        for (int i = 0; i < config.PossibleElements.Length; i++)
        {
            _rewardManager.SpawnRecruit(config.PossibleElements[i], 100, i);
            yield return new WaitForNextFrameUnit();
        }
        yield return new WaitUntil(() => _movementControllerRuntimeset.DestinationReached);
        //wait until one recruit selected or skipped
        _skipEnabled.Value = false;
        if(_partyManager.Party.Count != 0)
        {
            _skipButtonArmed.Value = true;
        }
        yield return new WaitUntil(() => _partyManager.Party.Count == currentPartySize + 1 || _skipEnabled.Value);
        _skipButtonArmed.Value = false;
        _rewardManager.RemoveAllRecruits();
        yield return new WaitForNextFrameUnit();
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.DestinationReached);
    }

    private IEnumerator FightRound(RoomConfiguration config)
    {
        //Spawn next room
        _partyManager.ResetPositions();

        foreach(EnemyConfiguration enemyConfig in config.OffensiveEnemies)
        {
            int i = Random.Range(0, enemyConfig.PossibleElements.Length);
            _enemyManager.AddOffensiveCharacter(enemyConfig.PossibleElements[i]);
        }

        foreach (EnemyConfiguration enemyConfig in config.DefensiveEnemies)
        {
            int i = Random.Range(0, enemyConfig.PossibleElements.Length);
            _enemyManager.AddDefensiveCharacter(enemyConfig.PossibleElements[i]);
        }

        yield return new WaitUntil(() => _movementControllerRuntimeset.DestinationReached);

        _fightEnabled.Value = false;
        _fightButtonArmed.Value = true;
        yield return new WaitUntil(() => _fightEnabled.Value);
        _battleManager.StartBattle();
        yield return new WaitUntil(() => _partyManager.Defeated || _enemyManager.Defeated);
        if (_partyManager.Defeated)
        {
            int rooms = _roomManager.CurrentRoom - _startRoom - 1;
            string suffix = rooms == 1 ? "" : "s";
            OnGameOver?.Invoke($"You survived {rooms} room{suffix}");
            yield break;
        }
        _currency.Credits += 100;
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.DestinationReached);
    }
}
