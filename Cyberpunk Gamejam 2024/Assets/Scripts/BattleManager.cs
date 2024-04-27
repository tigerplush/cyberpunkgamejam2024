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
        Attack[] partyAttacks = _partyManager.GetAttacks();
        _enemyManager.Resolve(partyAttacks);
        if(_enemyManager.Defeated)
        {
            Debug.Log("Enemies is defeated...");
            return;
        }
        Attack[] enemyAttacks = _enemyManager.GetAttacks();
        _partyManager.Resolve(enemyAttacks);
        if (_partyManager.Defeated)
        {
            Debug.Log("Party is defeated...");
            return;
        }
    }
}
