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
    private DropZoneUi _offensiveZone;
    [SerializeField]
    private DropZoneUi _benchZone;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        _benchZone.Disable();
        // spawn a character offscreen
        _partyManager.AddOffensiveCharacter(_firstCharacterElement);
        yield return new WaitForNextFrameUnit();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _enemyManager.AddOffensiveCharacter(_firstEnemyElement);
        yield return new WaitForNextFrameUnit();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _battleManager.StartBattle();
        yield return new WaitUntil(() => _enemyManager.Defeated);
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _roomManager.ScrollToRoom(1);
        yield return new WaitUntil(() => _roomManager.DestinationReached);
        _partyManager.ResetPositions();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _rewardManager.SpawnRecruit(_secondRecruitElement, 100);
        _offensiveZone.Disable();
        yield return new WaitForNextFrameUnit();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        yield return new WaitUntil(() => _partyManager.Party.Count == 2);
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _roomManager.SpawnRoom();
        yield return new WaitForNextFrameUnit();
        _roomManager.ScrollToRoom(2);
        yield return new WaitUntil(() => _roomManager.DestinationReached);
        _partyManager.ResetPositions();
        _enemyManager.AddOffensiveCharacter(_secondEnemyElement);
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _battleManager.StartBattle();
        _offensiveZone.Enable();
        _benchZone.Enable();
    }
}
