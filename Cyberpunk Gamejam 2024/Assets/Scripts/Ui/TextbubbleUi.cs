using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextbubbleUi : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private TextbubbleAttribute _textbubble;
    [SerializeField]
    private TextMeshProUGUI _text;

    private void OnEnable()
    {
        if(_textbubble != null )
        {
            _textbubble.OnTextChanged += UpdateUi;
        }
    }

    private void OnDisable()
    {
        if (_textbubble != null)
        {
            _textbubble.OnTextChanged += UpdateUi;
        }
    }

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void UpdateUi(bool enabled, string text)
    {
        gameObject.SetActive(enabled);
        _text.text = text;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _textbubble.Set(false, "");
    }
}
