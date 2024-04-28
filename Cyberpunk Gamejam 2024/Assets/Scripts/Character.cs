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

    [SerializeField]
    private int _counter;

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

    public void Resolve(Attack[] attacks)
    {
        float cumulativeDamage = 0f;
        string log = "";
        foreach(Attack attack in attacks.Where(a => a.AttackType == AttackType.Offensive))
        {
            float damage = 0f;
            if(_element.Weakness == attack.Element && !attack.Debuffed)
            {
                damage += _damage.WeaknessDamage;
            }
            else if(_element.Strength == attack.Element)
            {
                damage += _damage.StrengthDamage;
            }
            else
            {
                damage += _damage.NormalDamage;
            }
            damage *= attack.Multiplier;
            log += $"{attack} does {damage} damage to {name}\n";
            cumulativeDamage += damage;
        }
        foreach (Attack attack in attacks.Where(a => a.AttackType == AttackType.Defensive))
        {
            float damage = 0f;
            if (_element.Weakness == attack.Element && !attack.Debuffed)
            {
                damage += _defensiveDamage.WeaknessDamage;
            }
            else if (_element.Strength == attack.Element)
            {
                damage += _defensiveDamage.StrengthDamage;
            }
            else
            {
                damage += _defensiveDamage.NormalDamage;
            }
            log += $"{attack} does {damage} damage to {name}\n";
            cumulativeDamage += damage;
        }
        Debug.Log(log);
        _hp -= cumulativeDamage;
        OnHealthChanged?.Invoke(_hp);
    }

    public Attack GetAttack()
    {
        Attack attack = new Attack()
        {
            Element = _element,
            AttackType = _attackType,
            // if the counter is equal to the crit every round, we take the multiplier
            // we need to subtract 1 because our counter starts at 0
            Multiplier = _counter == (_element.CritEveryRound - 1) ? _element.CritMultiplier : 1f,
        };
        _counter += 1;
        if(_counter > (_element.CritEveryRound - 1))
        {
            //if the increased counter is greater than crit every round, we reset it
            _counter = 0;
        }
        return attack;
    }
}
