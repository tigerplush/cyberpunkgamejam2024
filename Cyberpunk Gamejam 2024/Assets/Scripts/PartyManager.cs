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
        AddCharacter(element, _offensiveMembers, _offensiveSpawnPoints, AttackType.Offensive);
    }

    /// <summary>
    /// Adds a character. Called by dropping an element in the drop zone.
    /// </summary>
    /// <param name="element"></param>
    public void AddDefensiveCharacter(Element element)
    {
        AddCharacter(element, _defensiveMembers, _defensiveSpawnPoints, AttackType.Defensive);
    }

    private void AddCharacter(Element element, List<Character> list, GameObject[] spawnPoints, AttackType attackType)
    {
        Vector3 spawnPosition = Vector3.zero;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int i = list.Count % spawnPoints.Length;
            GameObject spawnPoint = spawnPoints[i];
            spawnPosition = spawnPoint.transform.position + new Vector3(0.25f, 0.25f, 0.25f) * (list.Count / spawnPoints.Length);
        }

        Character character = Instantiate(_characterPrefab, spawnPosition, Quaternion.identity)
            .GetComponent<Character>()
            .SetElement(element)
            .SetAttackType(attackType)
            .SetName($"{attackType} Party Member #{list.Count}");

        list.Add(character);
    }
}
