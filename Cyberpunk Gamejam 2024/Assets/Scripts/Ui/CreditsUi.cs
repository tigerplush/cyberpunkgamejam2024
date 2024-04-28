using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private Currency _currency;

    [SerializeField]
    private int _currentCredits;
    [SerializeField]
    private int _lastCredits;
    [SerializeField]
    private float _t;

    private void OnEnable()
    {
        if(_currency != null)
        {
            _currency.OnCurrencyChanged += UpdateValues;
            _currentCredits = _currency.Credits;
            UpdateUi();
        }
    }

    private void OnDisable()
    {
        if (_currency != null)
        {
            _currency.OnCurrencyChanged -= UpdateValues;
        }
    }

    private void UpdateValues(int newCredits)
    {
        _lastCredits = _currentCredits;
        _t = 0f;
    }

    private void UpdateUi()
    {
        _text.text = $"© {_currentCredits:00000}";
    }

    private void Update()
    {
        if(_currentCredits == _currency.Credits)
        {
            return;
        }
        _t += Time.deltaTime;
        _currentCredits = (int)Mathf.Lerp(_lastCredits, _currency.Credits, _t);
        UpdateUi();
    }
}
