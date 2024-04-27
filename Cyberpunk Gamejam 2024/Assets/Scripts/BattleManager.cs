using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PartyManager _partyManager;
    [SerializeField]
    private EnemyManager _enemyManager;

    public void StartBattle()
    {
        if(_partyManager.Defeated)
        {
            Debug.Log("Party is defeated...");
            return;
        }
        if(_enemyManager.Defeated)
        {
            return;
        }
        Round();
    }

    public void Round()
    {
        Element[] partyElements = _partyManager.GetAttackTypes();
        _enemyManager.Resolve(partyElements);
        if(_enemyManager.Defeated)
        {
            Debug.Log("Enemies is defeated...");
            return;
        }
        Element[] enemyElements = _enemyManager.GetAttackTypes();
        _partyManager.Resolve(enemyElements);
        if (_partyManager.Defeated)
        {
            Debug.Log("Party is defeated...");
            return;
        }
    }
}
