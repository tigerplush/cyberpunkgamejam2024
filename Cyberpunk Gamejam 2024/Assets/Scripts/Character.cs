using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Element _element;

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

    public Character SetName(string name)
    {
        gameObject.name = name;
        return this;
    }
}
