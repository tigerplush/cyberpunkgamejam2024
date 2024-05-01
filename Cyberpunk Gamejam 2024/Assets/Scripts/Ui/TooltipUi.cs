using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _header;
    [SerializeField]
    private TextMeshProUGUI _description;
    [SerializeField]
    private LayoutElement _layoutElement;
    [SerializeField]
    private int _characterWrapLimit;
    [SerializeField]
    private Canvas _parent;

    public void Set(string description, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            _header.gameObject.SetActive(false);
        }
        else
        {
            _header.gameObject.SetActive(true);
            _header.text = header;
        }
        _description.text = description;
        UpdateSize();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            UpdateSize();
        }
        Vector3 mousePosition = Input.mousePosition;
        float width = ((RectTransform)transform).sizeDelta.x;
        float xSize = mousePosition.x + ((RectTransform)transform).sizeDelta.x;
        float canvasWidth = ((RectTransform)_parent.transform).sizeDelta.x;
        if (xSize > canvasWidth)
        {
            mousePosition.x = canvasWidth - width;
        }
        transform.position = mousePosition;
    }

    private void UpdateSize()
    {
        int headerLength = _header.text.Length;
        int contentLength = _description.text.Length;
        _layoutElement.enabled = headerLength > _characterWrapLimit || contentLength > _characterWrapLimit;
    }
}
