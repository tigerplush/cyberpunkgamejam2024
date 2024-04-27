using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public Element Element;
    public AttackType AttackType;
    public bool Debuffed = false;

    public override string ToString()
    {
        string debuffed = Debuffed ? "Debuffed " : "";
        return $"{debuffed}{AttackType} {Element.name}";
    }
}
