using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private Manager _partyManager;
    [SerializeField]
    private Manager _enemyManager;

    public void StartBattle()
    {
        StartCoroutine(Battle());
    }

    private IEnumerator Battle()
    {
        yield return new WaitForSeconds(1f);
        Round();
        if (_partyManager.Defeated)
        {
            Debug.Log("Party is defeated...");
            yield break;
        }
        if (_enemyManager.Defeated)
        {
            Debug.Log("Enemy is defeated...");
            yield break;
        }
        StartCoroutine(Battle());
    }

    private void Round()
    {
        _partyManager.NewRound();
        _enemyManager.NewRound();
        Special[] partySpecials = _partyManager.GetSpecials();
        _enemyManager.Resolve(partySpecials);

        Special[] enemySpecials = _enemyManager.GetSpecials();
        _partyManager.Resolve(enemySpecials);

        Attack[] partyAttacks = _partyManager.GetAttacks();
        _enemyManager.Resolve(partyAttacks);
        if(_enemyManager.Defeated)
        {
            return;
        }
        Attack[] enemyAttacks = _enemyManager.GetAttacks();
        _partyManager.Resolve(enemyAttacks);
        if (_partyManager.Defeated)
        {
            return;
        }
    }
}
