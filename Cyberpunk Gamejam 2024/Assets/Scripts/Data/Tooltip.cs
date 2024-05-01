using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tooltip : ScriptableObject
{
    public string Description;
    public string Header;
    public bool Enabled;

    public delegate void OnTooltipChangedHandler(Tooltip tooltip);
    public OnTooltipChangedHandler OnTooltipChanged;

    public void Set(bool enabled, string description, string header = null)
    {
        Enabled = enabled;
        Description = description;
        Header = header;
        OnTooltipChanged?.Invoke(this);
    }
}
