using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Element : ScriptableObject
{
    public Color PrimaryColor;
    public Element Weakness;
    public Element Strength;
    public int CritEveryRound = -1;
    public float CritMultiplier = 1f;
}
