using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MenuSystem
{
    private const int                   MAX_MENU =  3;
    private static readonly int[]       MAX_STATE = { 6, 6, 5 };
    private static readonly int[]       MIN_STATE = { 1, 1, 0 };

    [SerializeField] private GameObject prefubCounter;
    GameObject[]                        counter;
    private Counter[]                   actionCounter;

    [SerializeField] private GameObject matrix;
    Field                               actionMatrix;

    [SerializeField] private GameObject ai;
    AI                                  actionAi;
   
    private void Awake()
    {
        numbMenu =      MAX_MENU;
        counter =       new GameObject[numbMenu];
        actionCounter = new Counter[numbMenu];
        state =         new int[numbMenu];
        maxState =      new int[numbMenu];
        minState =      new int[numbMenu];
        actionMatrix =  matrix.GetComponent<Field>();
        actionAi =      ai.GetComponent<AI>();

        for (int i = 0; i < numbMenu; i++)
        {
            counter[i] = Instantiate
                (
                    prefubCounter,
                    transform.position + new Vector3((i - 1) * 4, 0),
                    transform.rotation
                );
            actionCounter[i] = counter[i].GetComponent<Counter>();
            actionCounter[i].setText(i);
            maxState[i] =   MAX_STATE[i];
            minState[i] =   MIN_STATE[i];
            state[i] =      minState[i];
        }
    }

    public void OnEnable()
    {
        for ( int i = 0; i < numbMenu; i++ )
        {
            counter[i].SetActive( true );
            state[i] = minState[i];
        }
        targetMenu = 0;
    }
    protected override void triggerSelectMenu()
    {
        matrix.SetActive( true );
        ai.SetActive( true );
        actionMatrix.Size = new Vector2Int( state[0] + 1, state[1] + 1 );
        actionMatrix.start();
        actionAi.Level =    state[2] + 1;
        actionAi.SizeField = new Vector2Int( state[0] + 1, state[1] + 1 );

        for ( int i = 0; i < numbMenu;i++ ) 
        {
            counter[i].SetActive( false );
        }
        transform.gameObject.SetActive( false );
    }

    void Update()
    {
        for ( int i = 0; i < numbMenu; i++ )
        {
            if ( targetMenu == i )
                actionCounter[i].IsSelected =       true;
            else
                actionCounter[i].IsSelected =       false;
            actionCounter[i].NumberSpriteIndex =    state[i];
        }
        chooseMenu();
        selectMenu();
    }
}
