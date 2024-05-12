using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableBoolAttribute : ScriptableObject
{
    [SerializeField]
    private bool _value;

    public delegate void OnValueChangedHandler(bool newValue);
    public OnValueChangedHandler OnValueChanged;

    public bool Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }
}
