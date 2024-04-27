using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Pointer : ScriptableObject
{
    public bool Enabled;
    public Vector2 Position;
    public Element Element;

    public delegate void OnValuesChangedHandler();
    public OnValuesChangedHandler OnValuesChanged;

    public void Set(bool enabled, Vector2 position, Element element)
    {
        Enabled = enabled;
        Position = position;
        Element = element;
        OnValuesChanged?.Invoke();
    }
}
