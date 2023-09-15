using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMenu : MenuSystem
{
    private const int   MAX_MENU =  1;
    private const int   MAX_STATE = 2;
    private const int   MIN_STATE = 0;

    [SerializeField] private GameObject resultText;
    private ResultText                  actionResultText;

    [SerializeField] private GameObject restartText;
    [SerializeField] private GameObject exitText;

    [SerializeField] private GameObject arrows;
    [SerializeField] private Arrows     actionArrows;

    [SerializeField] private GameObject mainMenu;
    private MainMenu                    actionMainMenu;

    private bool        isWin;
    public bool IsWin
    {
        set { isWin = value; }
    }

    private void Awake()
    {
        actionResultText =  resultText.GetComponent<ResultText>();
        actionArrows =      arrows.GetComponent<Arrows>();
        actionMainMenu =    mainMenu.GetComponent<MainMenu>(); 
        numbMenu =          MAX_MENU;
        state =             new int[numbMenu];
        maxState =          new int[numbMenu];
        minState =          new int[numbMenu];  
        isWin =             false;

        for ( int i = 0; i < numbMenu; i++ )
        {
            maxState[i] = MAX_STATE;
            minState[i] = MIN_STATE;
        }
    }

    public void showResult()
    {
        resultText.SetActive( true );
        restartText.SetActive( true );
        exitText.SetActive( true );
        arrows.SetActive( true );
        actionResultText.spriteIndex =  Convert.ToInt32( !isWin );
        actionArrows.Height =           0;
    }

    protected override void triggerSelectMenu()
    {
        if ( state[0] == 0 )
        {
            mainMenu.SetActive( true );
            resultText.SetActive( false );
            restartText.SetActive( false );
            exitText.SetActive( false );
            arrows.SetActive( false );
            transform.gameObject.SetActive( false );
        }
        else
            Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        chooseMenu();
        selectMenu();
        actionArrows.Height = state[0];
    }
}
