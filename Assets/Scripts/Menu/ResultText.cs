using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultText : MonoBehaviour
{
    private SpriteRenderer              spriteRender;
    [SerializeField] private Sprite[]   sprite;

    public int spriteIndex
    {
        set { spriteRender.sprite = sprite[ value ]; }
    }

    void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

}
