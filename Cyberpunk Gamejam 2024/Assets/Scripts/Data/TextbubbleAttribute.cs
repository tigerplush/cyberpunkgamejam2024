using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TextbubbleAttribute : ScriptableObject
{
    [SerializeField]
    private bool _enabled = false;
    [SerializeField]
    private string _text;

    public bool Enabled
    {
        get
        {
            return _enabled;
        }
    }

    public delegate void OnTextChangedHandler(bool enabled, string text);
    public OnTextChangedHandler OnTextChanged;

    public void Set(bool enabled, string text)
    {
        _enabled = enabled;
        _text = text;
        OnTextChanged?.Invoke(_enabled, _text);
    }
}
