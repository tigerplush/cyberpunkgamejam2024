using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Element _element;
    [SerializeField]
    private AttackType _attackType;
    [SerializeField]
    private Damage _damage;
    [SerializeField]
    private Damage _defensiveDamage;

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

    public AttackType AttackType
    {
        get { return _attackType; }
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

    public Character SetAttackType(AttackType attackType)
    {
        _attackType = attackType;
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

    public void Resolve(Attack[] attacks)
    {
        Debug.Log($"{attacks.Length} incoming attacks");
        float damage = 0f;
        string log = "";
        foreach(Attack attack in attacks.Where(a => a.AttackType == AttackType.Offensive))
        {
            log += $"{attack} does ";
            if(_element.Weakness == attack.Element && !attack.Debuffed)
            {
                log += $"{_damage.WeaknessDamage}";
                damage += _damage.WeaknessDamage;
            }
            else if(_element.Strength == attack.Element)
            {
                log += $"{_damage.StrengthDamage}";
                damage += _damage.StrengthDamage;
            }
            else
            {
                log += $"{_damage.NormalDamage}";
                damage += _damage.NormalDamage;
            }
            log += $" damage to {name}\n";
        }
        foreach (Attack attack in attacks.Where(a => a.AttackType == AttackType.Defensive))
        {
            log += $"{attack} does ";
            if (_element.Weakness == attack.Element && !attack.Debuffed)
            {
                log += $"{_defensiveDamage.WeaknessDamage}";
                damage += _defensiveDamage.WeaknessDamage;
            }
            else if (_element.Strength == attack.Element)
            {
                log += $"{_defensiveDamage.StrengthDamage}";
                damage += _defensiveDamage.StrengthDamage;
            }
            else
            {
                log += $"{_defensiveDamage.NormalDamage}";
                damage += _defensiveDamage.NormalDamage;
            }
            log += $" damage to {name}\n";
        }
        Debug.Log(log);
        _hp -= damage;
        OnHealthChanged?.Invoke(_hp);
    }
}
