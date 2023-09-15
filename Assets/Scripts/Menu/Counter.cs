using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Counter : MonoBehaviour
{
    // Start is called before the first frame update

    private SpriteRenderer                  spriteRender;
    [SerializeField] private Sprite[]       sprite;

    [SerializeField] private GameObject     prefabNumber;
    private GameObject                      number;
    private Number                          actionNumber;

    [SerializeField] private GameObject     prefabText;
    private GameObject                      text;
    private MainMenuText                    actionText;

    public int NumberSpriteIndex
    {
        set 
        { 
            actionNumber.spriteIndex = value;
        }
    }
    public bool IsSelected
    {
        set 
        {
            spriteRender.sprite = sprite[ Convert.ToInt32( value )];
        }
    }
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        number =        Instantiate( prefabNumber, transform.position, transform.rotation );
        text =          Instantiate
            ( 
            prefabText, 
            transform.position - new Vector3( 0, -4.5F ), 
            transform.rotation 
            );
        actionNumber =  number.GetComponent<Number>();
        actionText =    text.GetComponent<MainMenuText>();
    }
    public void setText( int id )
    {
        actionText.spriteIndex = id;
    }
    public void OnDisable()
    {
        number?.SetActive( false );
        text?.SetActive( false );
    }

    public void OnEnable()
    {
        number.SetActive ( true );
        text.SetActive ( true );
    }
}
