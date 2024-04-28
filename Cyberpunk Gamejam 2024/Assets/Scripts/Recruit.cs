using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Recruit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Element _element;
    [SerializeField]
    private int _cost;
    [SerializeField]
    private CurrentSelectedRecruit _currentSelectedRecruit;

    public Element Element
    {
        get
        {
            return _element;
        }
    }

    public int Cost
    {
        get
        {
            return _cost;
        }
    }

    public Recruit SetElement(Element element)
    {
        _element = element;
        _spriteRenderer.color = _element.PrimaryColor;
        return this;
    }

    public Recruit SetCost(int cost)
    {
        _cost = cost;
        return this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _currentSelectedRecruit.Recruit = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(eventData.position);
        position.z = 0f;
        transform.position = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _currentSelectedRecruit.Recruit = null;
        GetComponent<MovementController>()
            .Reset();
    }

    public void Consume()
    {
        Destroy(gameObject);
    }
}
