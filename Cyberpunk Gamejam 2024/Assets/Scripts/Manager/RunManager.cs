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
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        _startRoom = _roomManager.CurrentRoom;
        for(int i = 0; i < _allElements.Length; i++)
        {
            _rewardManager.SpawnRecruit(_allElements[i], 100, i);
            yield return new WaitForNextFrameUnit();
        }
        yield return new WaitUntil(() => _movementControllerRuntimeset.DestinationReached);
        yield return new WaitUntil(() => _partyManager.Party.Count == 1);
        _rewardManager.RemoveAllRecruits();
        yield return new WaitForNextFrameUnit();
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.DestinationReached);

        yield return FightRound();
        yield return SpawnRecruitStage();
        yield return FightRound();

        yield return null;
    }

    private IEnumerator SpawnEnemyRoom()
    {
        //Spawn next room
        yield return _roomManager.SpawnAndScrollToNextRoom();
        _partyManager.ResetPositions();
        _enemyManager.AddOffensiveCharacter(_allElements[0]);
        yield return new WaitUntil(() => _movementControllerRuntimeset.DestinationReached);
    }

    private IEnumerator SpawnRecruitStage()
    {
        int currentPartySize = _partyManager.Party.Count;
        //Spawn next room
        yield return _roomManager.SpawnAndScrollToNextRoom();
        _partyManager.ResetPositions();
        for (int i = 0; i < _allElements.Length; i++)
        {
            _rewardManager.SpawnRecruit(_allElements[i], 100, i);
            yield return new WaitForNextFrameUnit();
        }
        yield return new WaitUntil(() => _movementControllerRuntimeset.DestinationReached);
        //wait until one recruit selected or skipped
        _skipEnabled.Value = false;
        _skipButtonArmed.Value = true;
        yield return new WaitUntil(() => _partyManager.Party.Count == currentPartySize + 1 || _skipEnabled.Value);
        _skipButtonArmed.Value = false;
        _rewardManager.RemoveAllRecruits();
        yield return new WaitForNextFrameUnit();
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.DestinationReached);
    }

    private IEnumerator FightRound()
    {
        yield return SpawnEnemyRoom();
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
