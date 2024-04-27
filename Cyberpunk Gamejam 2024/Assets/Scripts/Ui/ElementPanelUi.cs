using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementPanelUi : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Element _element;

    // Start is called before the first frame update
    void Start()
    {
        if(_element == null)
        {
            return;
        }
        _image.color = _element.PrimaryColor;
    }
}
