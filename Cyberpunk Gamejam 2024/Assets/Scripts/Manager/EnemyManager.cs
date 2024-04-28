using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EnemyManager : Manager
{
    [SerializeField]
    private Element[] _availableElements;
    [SerializeField]
    private GameObject _characterPrefab;
    [SerializeField]
    private GameObject[] _offensiveSpawnPoints;
    [SerializeField]
    private GameObject[] _defensiveSpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        Spawn(1);
    }

    private void SpawnEnemy(int number, List<Character> enemyList, GameObject[] spawnPoints, AttackType attackType)
    {
        Vector3 spawnPosition = Vector3.zero;
        if(spawnPoints != null && spawnPoints.Length > 0)
        {
            int i = number / 2;
            GameObject spawnPoint = spawnPoints[i];
            spawnPosition = spawnPoint.transform.position + new Vector3(0.25f, 0.25f, 0.25f) * (number / 4);
        }

        int randomElementNumber = Random.Range(0, _availableElements.Length);
        Element element = _availableElements[randomElementNumber];

        Character character = Instantiate(_characterPrefab, spawnPosition, Quaternion.identity)
            .GetComponent<Character>()
            .SetElement(element)
            .SetAttackType(attackType)
            .SetName($"Enemy #{number}");

        enemyList.Add(character);
    }

    /// <summary>
    /// Spawns a number of enemies. This is currently called by the enemy panel buttons.
    /// </summary>
    /// <param name="enemyCount"></param>
    public void Spawn(int enemyCount)
    {
        foreach(Character character in _offensiveMembers)
        {
            Destroy(character.gameObject);
        }
        _offensiveMembers.Clear();

        foreach (Character character in _defensiveMembers)
        {
            Destroy(character.gameObject);
        }
        _defensiveMembers.Clear();

        for (int i = 0; i < enemyCount; i++)
        {
            List<Character> enemies = i % 2 == 0 ? _offensiveMembers : _defensiveMembers;
            GameObject[] spawnPoints = i % 2 == 0 ? _offensiveSpawnPoints : _defensiveSpawnPoints;
            AttackType attackType = i % 2 == 0 ? AttackType.Offensive : AttackType.Defensive;
            SpawnEnemy(i, enemies, spawnPoints, attackType);
        }
    }
}
