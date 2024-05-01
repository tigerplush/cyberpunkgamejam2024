using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZoneUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private Image _panelImage;
    [SerializeField]
    private Color _enabledColor;
    [SerializeField]
    private Color _disabledColor;
    [SerializeField]
    private Pointer _pointer;
    [SerializeField]
    private CurrentSelectedRecruit _currentSelectedRecruit;

    public UnityEvent<Element> OnDrop;
    public UnityEvent<Recruit> OnRecruit;

    private bool _pointerIsInBounds = false;
    private Recruit _lastRecruit = null;

    [SerializeField]
    private bool _enabled = true;

    private void OnEnable()
    {
        if(_pointer != null)
        {
            _pointer.OnValuesChanged += UpdateUi;
        }
        if(_currentSelectedRecruit != null)
        {
            _currentSelectedRecruit.OnValueChanged += UpdateRecruit;
        }
    }

    private void OnDisable()
    {
        if (_pointer != null)
        {
            _pointer.OnValuesChanged -= UpdateUi;
        }
        if (_currentSelectedRecruit != null)
        {
            _currentSelectedRecruit.OnValueChanged -= UpdateRecruit;
        }
    }

    private void UpdateUi()
    {
        if(_pointerIsInBounds && !_pointer.Enabled && _enabled)
        {
            // if the pointer is in bounds of the drop zone and the pointer is no longer enabled,
            // it means we did a drop here
            OnDrop?.Invoke(_pointer.Element);
        }
        _panel.SetActive(_pointer.Enabled);
    }

    private void UpdateRecruit(Recruit recruit)
    {
        if(recruit != null)
        {
            _lastRecruit = recruit;
        }
        if(_panel != null)
        {
            _panel.SetActive(recruit != null);
        }
        if (_pointerIsInBounds && recruit == null && _enabled)
        {
            OnRecruit?.Invoke(_lastRecruit);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerIsInBounds = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerIsInBounds = false;
    }

    public void Enable()
    {
        _enabled = true;
        _panelImage.color = _enabledColor;
    }

    public void Disable()
    {
        _enabled = false;
        _panelImage.color = _disabledColor;
    }
}
