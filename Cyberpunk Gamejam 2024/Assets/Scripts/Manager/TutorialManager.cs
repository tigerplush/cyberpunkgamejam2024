using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private Manager _partyManager;
    [SerializeField]
    private Manager _enemyManager;
    [SerializeField]
    private BattleManager _battleManager;
    [SerializeField]
    private Element _firstCharacterElement;
    [SerializeField]
    private Element _firstEnemyElement;
    [SerializeField]
    private Element _secondEnemyElement;
    [SerializeField]
    private Element _secondRecruitElement;
    [SerializeField]
    private RuntimeSet _movementControllerRuntimeset;
    [SerializeField]
    private RoomManager _roomManager;
    [SerializeField]
    private RewardManager _rewardManager;
    [SerializeField]
    private RunManager _runManager;

    [SerializeField]
    private DropZoneUi _offensiveZone;
    [SerializeField]
    private DropZoneUi _benchZone;
    [SerializeField]
    private ScriptableBoolAttribute _isTutorialEnabled;
    [SerializeField]
    private TextbubbleAttribute _textbubble;
    [SerializeField]
    private Currency _currency;

    // Start is called before the first frame update
    private void Start()
    {
        if (_isTutorialEnabled != null && !_isTutorialEnabled.Value)
        {
            //If the tutorial is disabled, we return.
            return;
        }
        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        //Setup
        _benchZone.Disable();

        // Room 1 - basic combat
        // A party member defeats an enemy, player receives enough money for recruitment
        _partyManager.AddOffensiveCharacter(_firstCharacterElement);
        yield return new WaitForNextFrameUnit();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _textbubble.Set(true, "This is your first character.Click this bubble, an enemy will appear and they will fight.");
        yield return new WaitUntil(() => !_textbubble.Enabled);
        _enemyManager.AddOffensiveCharacter(_firstEnemyElement);
        yield return new WaitForNextFrameUnit();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _battleManager.StartBattle();
        yield return new WaitUntil(() => _enemyManager.Defeated);
        _currency.Credits += 100;
        _textbubble.Set(true, "Great job, you defeated the enemy and received <i>100 Credits</i>.");
        yield return new WaitUntil(() => !_textbubble.Enabled);
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));

        // Room 2 - recruit a new party member into the defense
        yield return _roomManager.SpawnAndScrollToNextRoom();
        yield return new WaitUntil(() => _roomManager.DestinationReached);
        _partyManager.ResetPositions();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _rewardManager.SpawnRecruit(_secondRecruitElement, 100, 0);
        _offensiveZone.Disable();
        yield return new WaitForNextFrameUnit();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _textbubble.Set(true, "Recruit the new character by dragging them into the defensive drop zone.");
        yield return new WaitUntil(() => _partyManager.Party.Count == 2);
        _textbubble.Set(true, "Defensive characters will contribute less to the damage but protect your offensive characters.");
        yield return new WaitUntil(() => !_textbubble.Enabled);
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));

        // Room 3 - Offense barely survives, active and passive abilities are shown
        yield return _roomManager.SpawnAndScrollToNextRoom();
        _partyManager.ResetPositions();
        _enemyManager.AddOffensiveCharacter(_secondEnemyElement);
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _battleManager.StartBattle();
        yield return new WaitUntil(() => _enemyManager.Defeated);
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));

        // Room 4 - resort
        // put offensive member on bench
        // put defensive member in offense
        // click start
        yield return _roomManager.SpawnAndScrollToNextRoom();
        _partyManager.ResetPositions();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));


        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));

        // Room 5 - TPK
        // player is left with 100 credits, barely enough to recruit
        yield return _roomManager.SpawnAndScrollToNextRoom();
        _partyManager.ResetPositions();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));

        _enemyManager.AddOffensiveCharacter(_secondEnemyElement);
        _enemyManager.AddOffensiveCharacter(_secondEnemyElement);
        _enemyManager.AddOffensiveCharacter(_secondEnemyElement);
        _enemyManager.AddOffensiveCharacter(_secondEnemyElement);
        _enemyManager.AddDefensiveCharacter(_secondEnemyElement);
        _enemyManager.AddDefensiveCharacter(_secondEnemyElement);
        _enemyManager.AddDefensiveCharacter(_secondEnemyElement);
        _enemyManager.AddDefensiveCharacter(_secondEnemyElement);
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _battleManager.StartBattle();
        yield return new WaitUntil(() => _partyManager.Defeated);
        _enemyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _enemyManager.Reset();
        _textbubble.Set(true, "You loose when your party is defeated, but this is was the tutorial, the real game starts now.");
        yield return new WaitUntil(() => !_textbubble.Enabled);

        // Teardown
        _offensiveZone.Enable();
        _benchZone.Enable();
        _runManager.StartRun();
        enabled = false;
    }
}
