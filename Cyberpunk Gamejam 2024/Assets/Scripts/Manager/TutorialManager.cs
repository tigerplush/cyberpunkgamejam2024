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
    private RuntimeSet _movementControllerRuntimeset;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        // spawn a character offscreen
        _partyManager.AddOffensiveCharacter(_firstCharacterElement);
        yield return new WaitForNextFrameUnit();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _enemyManager.AddOffensiveCharacter(_firstEnemyElement);
        yield return new WaitForNextFrameUnit();
        yield return new WaitUntil(() => _movementControllerRuntimeset.Set.All(g => g.GetComponent<MovementController>().ReachedDestination));
        _battleManager.StartBattle();
    }
}
