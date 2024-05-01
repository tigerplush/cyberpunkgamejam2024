using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Recruit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Element _element;
    [SerializeField]
    private int _cost;
    [SerializeField]
    private CurrentSelectedRecruit _currentSelectedRecruit;
    [SerializeField]
    private Tooltip _tooltip;

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
        _tooltip.Set(false, "");
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

    public void OnPointerEnter(PointerEventData eventData)
    {

        string description = $"<b>Active</b>\n";
        switch(_element.SpecialType)
        {
            case SpecialType.Stun:
                description += $"Stuns an enemy every {_element.ApplyEveryXRounds} turns. Stunned enemies deal no damage.";
                break;
            case SpecialType.Charm:
                description += $"Charms an enemy every {_element.ApplyEveryXRounds} turns. Charmed enemies deal damager to their own defense.";
                break;
            case SpecialType.None:
            default:
                description += $"Deals {_element.CritMultiplier}x damage every {_element.ApplyEveryXRounds} turns.";
                break;
        }
        _tooltip.Set(true, description, $"{_element.name} Mc{_element.name}face");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.Set(false, "");
    }
}
