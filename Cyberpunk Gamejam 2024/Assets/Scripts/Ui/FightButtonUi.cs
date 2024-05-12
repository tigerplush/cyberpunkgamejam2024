using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightButtonUi : MonoBehaviour
{
    [SerializeField]
    private ScriptableBoolAttribute _fightArmedAttribute;

    private void Awake()
    {
        if (_fightArmedAttribute != null)
        {
            _fightArmedAttribute.OnValueChanged += EnableButton;
        }
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_fightArmedAttribute != null)
        {
            _fightArmedAttribute.OnValueChanged -= EnableButton;
        }
    }

    private void EnableButton(bool value)
    {
        gameObject.SetActive(value);
    }
}
