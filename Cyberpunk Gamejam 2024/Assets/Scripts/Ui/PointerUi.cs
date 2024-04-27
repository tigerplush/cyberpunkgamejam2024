using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerUi : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Pointer _pointer;

    [SerializeField]
    private Canvas _canvas;

    private void OnEnable()
    {
        if(_pointer != null )
        {
            _pointer.OnValuesChanged += UpdateUi;
        }
    }
    private void OnDisable()
    {
        if (_pointer != null)
        {
            _pointer.OnValuesChanged -= UpdateUi;
        }
    }

    private void Awake()
    {
        _image.enabled = false;
    }

    private void UpdateUi()
    {
        _image.enabled = _pointer.Enabled;
        _image.color = _pointer.Element.PrimaryColor;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)_canvas.transform,
            _pointer.Position,
            _canvas.worldCamera,
            out position
            );

        transform.position = _canvas.transform.TransformPoint(position);
    }
}
