using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    protected List<Character> _offensiveMembers = new List<Character>();
    [SerializeField]
    protected List<Character> _defensiveMembers = new List<Character>();
    [SerializeField]
    private List<Character> _benchedMembers = new List<Character>();
    [SerializeField]
    private GameObject[] _defensivePlacePoints;
    [SerializeField]
    private GameObject[] _offensivePlacePoints;
    [SerializeField]
    private GameObject _characterPrefab;
    [SerializeField]
    private GameObject _spawnPoint;

    [SerializeField]
    public GameObject _pointOffScreen;

    [SerializeField]
    private int _maxPartySize = 8;
    [SerializeField]
    private int _maxMembersOnField = 4;
    [SerializeField]
    private int _maxMembersInZone = 4;

    public List<Character> Party
    {
        get
        {
            List<Character> list = new List<Character>();
            list.AddRange(_offensiveMembers);
            list.AddRange(_defensiveMembers);
            return list;
        }
    }

    public bool CanAddOffensive
    {
        get
        {
            if(_benchedMembers.Count + Party.Count > _maxPartySize)
            {
                return false;
            }
            if(Party.Count >= _maxMembersOnField)
            {
                return false;
            }
            if(_offensiveMembers.Count >= _maxMembersInZone)
            {
                return false;
            }
            return true;
        }
    }

    public bool CanAddDefensive
    {
        get
        {
            if (_benchedMembers.Count + Party.Count > _maxPartySize)
            {
                return false;
            }
            if (Party.Count >= _maxMembersOnField)
            {
                return false;
            }
            if (_defensiveMembers.Count >= _maxMembersInZone)
            {
                return false;
            }
            return true;
        }
    }

    public bool CanAddBench
    {
        get
        {
            if (_benchedMembers.Count + Party.Count > _maxPartySize)
            {
                return false;
            }
            if (_benchedMembers.Count >= _maxPartySize - _maxMembersOnField)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Resets the whole party. Called by Reset button
    /// </summary>
    public void Reset()
    {
        foreach (Character c in _offensiveMembers)
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
        AddCharacter(element, _offensiveMembers, _offensivePlacePoints, AttackType.Offensive);
    }

    /// <summary>
    /// Adds a character. Called by dropping an element in the drop zone.
    /// </summary>
    /// <param name="element"></param>
    public void AddDefensiveCharacter(Element element)
    {
        AddCharacter(element, _defensiveMembers, _defensivePlacePoints, AttackType.Defensive);
    }

    public void AddBenchedCharacter(Element element)
    {

    }

    private void AddCharacter(Element element, List<Character> list, GameObject[] placePoints, AttackType attackType)
    {
        Vector3 targetPosition = Vector3.zero;
        if (placePoints != null && placePoints.Length > 0)
        {
            int i = list.Count % placePoints.Length;
            GameObject targetPoint = placePoints[i];
            targetPosition = targetPoint.transform.position + new Vector3(0.25f, 0.25f, 0.25f) * (list.Count / placePoints.Length);
        }
        Character character = Instantiate(_characterPrefab, _spawnPoint.transform.position, Quaternion.identity, transform)
            .GetComponent<Character>()
            .SetElement(element)
            .SetAttackType(attackType)
            .SetName($"{attackType} Member #{list.Count}");

        character.GetComponent<MovementController>()
            .SetTargetPosition(targetPosition);

        list.Add(character);
    }

    public bool Defeated
    {
        get
        {
            return _offensiveMembers.Count == 0 && _defensiveMembers.Count == 0;
        }
    }

    public Attack[] GetAttacks()
    {
        List<Attack> list = new List<Attack>();
        foreach (Character c in _offensiveMembers.Where(m => !m.Unavailable))
        {
            list.Add(c.GetAttack());
        }
        foreach (Character c in _defensiveMembers)
        {
            list.Add(c.GetAttack());
        }
        return list.ToArray();
    }

    public void Resolve(Attack[] attacks)
    {
        if (_offensiveMembers.Count > 0)
        {
            // Resolve Offensive
            ResolveOffensive(attacks);
        }
        else
        {
            // Resolve Defensive
            ResolveDefensive(attacks);
        }
    }

    public void Resolve(Special[] specials)
    {
        foreach(Special s in specials.Where(s => s.SpecialType != SpecialType.None))
        {
            Character c = _offensiveMembers.FirstOrDefault(m => !m.Unavailable);
            if(c != null)
            {
                c.Apply(s);
            }
        }
    }

    protected void ResolveOffensive(Attack[] attacks)
    {
        foreach (Character c in _defensiveMembers)
        {
            Attack attack = attacks.FirstOrDefault(a => a.Element == c.Element.Strength && a.Debuffed == false);
            if (attack != null)
            {
                attack.Debuffed = true;
            }
        }

        for (int i = _offensiveMembers.Count - 1; i >= 0; i--)
        {
            Character c = _offensiveMembers[i];
            c.Resolve(attacks);
            if (c.IsDead)
            {
                _offensiveMembers.Remove(c);
                Destroy(c.gameObject);
            }
        }

        List<Attack> turnedAttacks = new List<Attack>();
        foreach(Character c in _offensiveMembers.Where(m => m.TurnedAround))
        {
            turnedAttacks.Add(c.GetAttack());
        }
        ResolveDefensive(turnedAttacks.ToArray());
    }

    protected void ResolveDefensive(Attack[] attacks)
    {
        for (int i = _defensiveMembers.Count - 1; i >= 0; i--)
        {
            Character c = _defensiveMembers[i];
            c.Resolve(attacks);
            if (c.IsDead)
            {
                _defensiveMembers.Remove(c);
                Destroy(c.gameObject);
            }
        }
    }

    public Special[] GetSpecials()
    {
        List<Special> specials = new List<Special>();
        foreach(Character c in _offensiveMembers.Where(m => !m.Unavailable))
        {
            specials.Add(c.GetSpecial());
        }
        return specials.ToArray();
    }

    public void NewRound()
    {
        foreach(Character c in _offensiveMembers)
        {
            c.NewRound();
        }
    }

    public void SendAllOffscreen()
    {
        foreach(Character c in Party)
        {
            c.GetComponent<MovementController>()
                .SetTargetPosition(_pointOffScreen.transform.position);
        }
    }

    /// <summary>
    /// Resets the positions of all party members to the offscreen point and let's them walk to their assigned position again
    /// </summary>
    public void ResetPositions()
    {
        int off = 0;
        int def = 0;
        foreach(Character c in Party)
        {
            Vector3 targetPosition;
            if(c.AttackType == AttackType.Offensive)
            {
                targetPosition = _offensivePlacePoints[off %  _offensivePlacePoints.Length].transform.position + new Vector3(0.25f, 0.25f, 0.25f) * (off / _offensivePlacePoints.Length);
                off += 1;
            }
            else
            {
                targetPosition = _defensivePlacePoints[def % _defensivePlacePoints.Length].transform.position + new Vector3(0.25f, 0.25f, 0.25f) * (def / _defensivePlacePoints.Length);
                def += 1;
            }

            c.GetComponent<MovementController>()
                .SetPosition(_spawnPoint.transform.position)
                .SetTargetPosition(targetPosition);
        }
    }
}
