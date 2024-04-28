using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    [SerializeField]
    private bool _turnedAround;
    [SerializeField]
    private bool _stunned;

    [SerializeField]
    private Vector3 _originalPosition;

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

    public bool Unavailable
    {
        get
        {
            return _stunned || _turnedAround;
        }
    }

    public bool TurnedAround
    {
        get
        {
            return _turnedAround;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _originalPosition = transform.position;
        if(_element == null)
        {
            return;
        }
        _spriteRenderer.color = _element.PrimaryColor;
    }

    private void Update()
    {
        if(_stunned)
        {
            transform.position = _originalPosition + Random.onUnitSphere * 0.1f;
        }
        else if(_turnedAround)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
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
        if(attacks == null || attacks.Length == 0)
        {
            return;
        }
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
            // if the counter is equal to the apply every x rounds, we take the multiplier
            // we need to subtract 1 because our counter starts at 0
            Multiplier = _counter == (_element.ApplyEveryXRounds - 1) ? _element.CritMultiplier : 1f,
        };
        if(_element.CritMultiplier > 1f)
        {
            _counter += 1;
            if (_counter > (_element.ApplyEveryXRounds - 1))
            {
                //if the increased counter is greater than apply every x rounds, we reset it
                _counter = 0;
            }
        }
        return attack;
    }

    public Special GetSpecial()
    {
        Special special = new Special()
        {
            SpecialType = _counter == (_element.ApplyEveryXRounds - 1) ? _element.SpecialType : SpecialType.None,
        };
        if(_element.SpecialType != SpecialType.None)
        {
            _counter += 1;
            if (_counter > (_element.ApplyEveryXRounds - 1))
            {
                //if the increased counter is greater than apply every x rounds, we reset it
                _counter = 0;
            }
        }
        return special;
    }

    /// <summary>
    /// Applies a special to this character
    /// </summary>
    /// <param name="special"></param>
    public void Apply(Special special)
    {
        switch(special.SpecialType)
        {
            case SpecialType.Stun:
                _stunned = true;
                break;
            case SpecialType.Charm:
                _turnedAround = true;
                break;
            case SpecialType.None:
            default:
                break;
        }
    }

    /// <summary>
    /// Resets conditions
    /// </summary>
    public void NewRound()
    {
        _stunned = false;
        _turnedAround = false;
        transform.rotation = Quaternion.identity;
    }
}
