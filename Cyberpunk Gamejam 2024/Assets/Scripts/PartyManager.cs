using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PartyManager : Manager
{
    [SerializeField]
    private GameObject[] _defensiveSpawnPoints;
    [SerializeField]
    private GameObject[] _offensiveSpawnPoints;
    [SerializeField]
    private GameObject _characterPrefab;


    /// <summary>
    /// Resets the whole party. Called by Reset button
    /// </summary>
    public void Reset()
    {
        foreach(Character c in _offensiveMembers)
        {
            Destroy(c.gameObject);
        }
        foreach (Character c in _defensiveMembers)
        {
            Destroy(c.gameObject);
        }
        _offensiveMembers.Clear();
        _defensiveMembers.Clear();
    }

    /// <summary>
    /// Adds a character. Called by dropping an element in the drop zone.
    /// </summary>
    /// <param name="element"></param>
    public void AddOffensiveCharacter(Element element)
    {
        Vector3 spawnPosition = Vector3.zero;
        if (_offensiveSpawnPoints != null && _offensiveSpawnPoints.Length > 0)
        {
            int i = _offensiveMembers.Count % _offensiveSpawnPoints.Length;
            GameObject spawnPoint = _offensiveSpawnPoints[i];
            spawnPosition = spawnPoint.transform.position + new Vector3(0.25f, 0.25f, 0.25f) * (_offensiveMembers.Count / _offensiveSpawnPoints.Length);
        }

        Character character = Instantiate(_characterPrefab, spawnPosition, Quaternion.identity)
            .GetComponent<Character>()
            .SetElement(element)
            .SetName($"Offensive Party Member #{_offensiveMembers.Count}");

        _offensiveMembers.Add(character);
    }

    /// <summary>
    /// Adds a character. Called by dropping an element in the drop zone.
    /// </summary>
    /// <param name="element"></param>
    public void AddDefensiveCharacter(Element element)
    {
        Vector3 spawnPosition = Vector3.zero;
        if (_defensiveSpawnPoints != null && _defensiveSpawnPoints.Length > 0)
        {
            int i = _defensiveMembers.Count % _defensiveSpawnPoints.Length;
            GameObject spawnPoint = _defensiveSpawnPoints[i];
            spawnPosition = spawnPoint.transform.position + new Vector3(0.25f, 0.25f, 0.25f) * (_defensiveMembers.Count / _defensiveSpawnPoints.Length);
        }

        Character character = Instantiate(_characterPrefab, spawnPosition, Quaternion.identity)
            .GetComponent<Character>()
            .SetElement(element)
            .SetName($"Defensive Party Member #{_defensiveMembers.Count}");

        _defensiveMembers.Add(character);
    }
}
