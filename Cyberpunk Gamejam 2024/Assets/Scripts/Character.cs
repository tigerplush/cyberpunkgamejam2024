using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Element _element;
    [SerializeField]
    private Damage _damage;

    [SerializeField]
    private float _hp = 100f;

    public delegate void OnHealthChangedHandler(float newHealth);
    public OnHealthChangedHandler OnHealthChanged;

    public bool IsDead
    {
        get
        {
            return _hp <= 0f;
        }
    }

    public Element Element
    {
        get { return _element; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_element == null )
        {
            return;
        }
        _spriteRenderer.color = _element.PrimaryColor;
    }

    public Character SetElement(Element element)
    {
        _element = element;
        _spriteRenderer.color = _element.PrimaryColor;
        return this;
    }

    public Character SetName(string name)
    {
        gameObject.name = name;
        return this;
    }

    public void Resolve(Element[] elements)
    {
        float damage = 0f;
        foreach(Element element in elements)
        {
            if(_element.Weakness == element)
            {
                damage += _damage.WeaknessDamage;
            }
            else if(_element.Strength == element)
            {
                damage += _damage.StrengthDamage;
            }
            else
            {
                damage += _damage.NormalDamage;
            }
        }
        Debug.Log($"{name} takes {damage} damage");
        _hp -= damage;
        OnHealthChanged?.Invoke(_hp);
    }
}
