using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private StandaloneInputModule _inputModule;

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
        Debug.Log(Input.mousePosition);
        //transform.position = pos;
    }
    private void UpdateSize()
    {
        int headerLength = _header.text.Length;
        int contentLength = _description.text.Length;
        _layoutElement.enabled = headerLength > _characterWrapLimit || contentLength > _characterWrapLimit;
    }
}
