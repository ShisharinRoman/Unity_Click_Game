using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
   
    private SpriteRenderer                      spriteRender;
    [SerializeField] private Sprite[]           sprite;
    public int  spriteIndex
    {
        set { spriteRender.sprite = sprite[ value ]; }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }
}
