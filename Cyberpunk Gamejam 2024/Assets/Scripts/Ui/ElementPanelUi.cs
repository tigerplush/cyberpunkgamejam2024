using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementPanelUi : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Element _element;
    [SerializeField]
    private Pointer _pointer;

    // Start is called before the first frame update
    void Start()
    {
        if(_element == null)
        {
            return;
        }
        _image.color = _element.PrimaryColor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _pointer.Set(true, eventData.position, _element);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _pointer.Set(true, eventData.position, _element);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _pointer.Set(false, eventData.position, _element);
    }
}
