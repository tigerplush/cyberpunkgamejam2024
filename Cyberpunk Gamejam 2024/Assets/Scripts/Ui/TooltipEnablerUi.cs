using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipEnablerUi : MonoBehaviour
{
    [SerializeField]
    private TooltipUi _tooltipUi;
    [SerializeField]
    private Tooltip _tooltip;

    private void OnEnable()
    {
        if(_tooltip != null)
        {
            _tooltip.OnTooltipChanged += UpdateUi;
        }
    }

    private void OnDisable()
    {
        if (_tooltip != null)
        {
            _tooltip.OnTooltipChanged -= UpdateUi;
        }
    }

    private void UpdateUi(Tooltip tooltip)
    {
        _tooltipUi.Set(tooltip.Description, tooltip.Header);
        _tooltipUi.gameObject.SetActive(tooltip.Enabled);
    }
}
