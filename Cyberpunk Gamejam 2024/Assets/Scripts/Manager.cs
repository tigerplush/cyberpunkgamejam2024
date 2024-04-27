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
        foreach (Character c in _offensiveMembers)
        {
            list.Add(new Attack()
            {
                Element = c.Element,
                AttackType = AttackType.Offensive
            });
        }
        foreach (Character c in _defensiveMembers)
        {
            list.Add(new Attack()
            {
                Element = c.Element,
                AttackType = AttackType.Defensive
            });
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
}
