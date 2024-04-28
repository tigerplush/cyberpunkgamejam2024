using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CurrentSelectedRecruit : ScriptableObject
{
    private Recruit _recruit;

    public delegate void OnValueChangedHandler(Recruit newRecruit);
    public OnValueChangedHandler OnValueChanged;

    public Recruit Recruit
    {
        get { return _recruit; }
        set
        {
            _recruit = value;
            OnValueChanged?.Invoke(_recruit);
        }
    }
}
