using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private Character _character;

    private void OnEnable()
    {
        _character.OnHealthChanged += UpdateUi;
    }

    private void OnDisable()
    {
        _character.OnHealthChanged -= UpdateUi;
    }

    private void UpdateUi(float newHealth)
    {
        _slider.value = newHealth;
    }
}
