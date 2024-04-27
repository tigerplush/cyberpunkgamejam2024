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
    private Pointer _pointer;

    public UnityEvent<Element> OnDrop;

    private bool _pointerIsInBounds = false;

    private void OnEnable()
    {
        if(_pointer != null)
        {
            _pointer.OnValuesChanged += UpdateUi;
        }
    }
    private void OnDisable()
    {
        if (_pointer != null)
        {
            _pointer.OnValuesChanged -= UpdateUi;
        }
    }

    private void UpdateUi()
    {
        if(_pointerIsInBounds && !_pointer.Enabled)
        {
            // if the pointer is in bounds of the drop zone and the pointer is no longer enabled,
            // it means we did a drop here
            OnDrop?.Invoke(_pointer.Element);
        }
        _panel.SetActive(_pointer.Enabled);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerIsInBounds = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerIsInBounds = false;
    }
}
