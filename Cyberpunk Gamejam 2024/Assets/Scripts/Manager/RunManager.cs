using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script manages a complete run.
/// </summary>
public class RunManager : MonoBehaviour
{
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
        for(int i = 0; i < _allElements.Length; i++)
        {
            _rewardManager.SpawnRecruit(_allElements[i], 100, i);
            yield return new WaitForNextFrameUnit();
        }
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        yield return new WaitUntil(() => _partyManager.Party.Count == 1);
        _rewardManager.RemoveAllRecruits();
        yield return new WaitForNextFrameUnit();
        _partyManager.SendAllOffscreen();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));

        yield return SpawnEnemyRoom();

        yield return null;
    }

    private IEnumerator SpawnEnemyRoom()
    {
        //Spawn next room
        yield return _roomManager.SpawnAndScrollToNextRoom();
        _partyManager.ResetPositions();
        _enemyManager.AddOffensiveCharacter(_allElements[0]);
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));

    }
}
