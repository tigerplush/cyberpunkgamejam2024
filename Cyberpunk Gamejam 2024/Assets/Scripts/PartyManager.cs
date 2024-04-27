using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool Defeated
    {
        get
        {
            return _party.Count == 0;
        }
    }

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

    public Element[] GetAttackTypes()
    {
        return _party.Select(c => c.Element).ToArray();
    }

    public void Resolve(Element[] elements)
    {
        for (int i = _party.Count - 1; i >= 0; i--)
        {
            Character character = _party[i];
            character.Resolve(elements);
            if (character.IsDead)
            {
                _party.Remove(character);
                Destroy(character.gameObject);
            }
        }
    }
}
