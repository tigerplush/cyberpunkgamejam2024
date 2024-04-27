using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private List<Character> _party = new List<Character>();
    [SerializeField]
    private GameObject[] _spawnPoints;
    [SerializeField]
    private GameObject _characterPrefab;

    /// <summary>
    /// Resets the whole party. Called by Reset button
    /// </summary>
    public void Reset()
    {
        foreach(Character c in _party)
        {
            Destroy(c.gameObject);
        }
        _party.Clear();
    }

    /// <summary>
    /// Adds a character. Called by dropping an element in the drop zone.
    /// </summary>
    /// <param name="element"></param>
    public void AddCharacter(Element element)
    {
        Vector3 spawnPosition = Vector3.zero;
        if (_spawnPoints != null && _spawnPoints.Length > 0)
        {
            int i = _party.Count % _spawnPoints.Length;
            GameObject spawnPoint = _spawnPoints[i];
            spawnPosition = spawnPoint.transform.position + new Vector3(0.25f, 0.25f, 0.25f) * (_party.Count / _spawnPoints.Length);
        }

        Character character = Instantiate(_characterPrefab, spawnPosition, Quaternion.identity)
            .GetComponent<Character>()
            .SetElement(element)
            .SetName($"Party Member #{_party.Count}");

        _party.Add(character);
    }
}
