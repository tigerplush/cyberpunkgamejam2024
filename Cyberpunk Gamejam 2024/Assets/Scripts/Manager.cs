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
        foreach (Character c in _offensiveMembers.Where(m => !m.Unavailable))
        {
            Debug.Log($"{c.name} is unavailable: {c.Unavailable}");
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
        Debug.Log($"Trying to resolve {specials.Count(s => s.SpecialType != SpecialType.None)} specials");
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
}
