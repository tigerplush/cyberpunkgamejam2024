using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Currency : ScriptableObject
{
    [SerializeField]
    private int _credits;
    public delegate void CurrencyHandler(int newCredits);
    public CurrencyHandler OnCurrencyChanged;

    public int Credits
    {
        get
        {
            return _credits;
        }
        set
        {
            _credits = value;
            OnCurrencyChanged?.Invoke(_credits);
        }
    }
}
