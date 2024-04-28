using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public Element Element;
    public AttackType AttackType;
    public bool Debuffed = false;
    public float Multiplier = 1f;

    public override string ToString()
    {
        string debuffed = Debuffed ? "Debuffed " : "";
        string crit = Multiplier > 1f ? "Crit " : "";
        return $"{debuffed}{crit}{AttackType} {Element.name}";
    }
}
