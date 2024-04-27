using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private Element[] _availableElements;
    [SerializeField]
    private GameObject _characterPrefab;
    [SerializeField]
    private List<Character> _enemies = new List<Character>();
    [SerializeField]
    private GameObject[] _spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        Spawn(1);
    }

    private void SpawnEnemy(int number)
    {
        Vector3 spawnPosition = Vector3.zero;
        if(_spawnPoints != null && _spawnPoints.Length > 0)
        {
            int i = number % _spawnPoints.Length;
            GameObject spawnPoint = _spawnPoints[i];
            spawnPosition = spawnPoint.transform.position + new Vector3(0.25f, 0.25f, 0.25f) * (number / _spawnPoints.Length);
        }

        int randomElementNumber = Random.Range(0, _availableElements.Length);
        Element element = _availableElements[randomElementNumber];

        Character character = Instantiate(_characterPrefab, spawnPosition, Quaternion.identity)
            .GetComponent<Character>()
            .SetElement(element)
            .SetName($"Enemy #{number}");

        _enemies.Add(character);
    }

    /// <summary>
    /// Spawns a number of enemies. This is currently called by the enemy panel buttons.
    /// </summary>
    /// <param name="enemyCount"></param>
    public void Spawn(int enemyCount)
    {
        foreach(Character character in _enemies)
        {
            Destroy(character.gameObject);
        }
        _enemies.Clear();
        for(int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(i);
        }
    }
}
