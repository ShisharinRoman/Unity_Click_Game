using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    // Start is called before the first frame update

    protected int   numbMenu;
    protected int[] state;
    protected int[] maxState;
    protected int[] minState;
    protected int   targetMenu;
    protected void chooseMenu()
    {
        if ( Input.GetKeyDown( KeyCode.LeftArrow ))
            if ( targetMenu > 0 )
                targetMenu--;
            else
                targetMenu = numbMenu - 1;

        if ( Input.GetKeyDown( KeyCode.RightArrow ))
            if ( targetMenu < numbMenu - 1 )
                targetMenu++;
            else
                targetMenu = 0;

        if ( Input.GetKeyDown( KeyCode.DownArrow ))
            if ( state[targetMenu] > minState[targetMenu] )
                state[targetMenu]--;
            else
                state[targetMenu] = maxState[targetMenu] - 1;

        if ( Input.GetKeyDown( KeyCode.UpArrow ))
            if ( state[targetMenu] < maxState[targetMenu] - 1 )
                state[targetMenu]++;
            else
                state[targetMenu] = minState[targetMenu];

    }

    protected void selectMenu()
    {
        if ( Input.GetKeyDown( KeyCode.Space ))
            triggerSelectMenu();
    }

    protected virtual void triggerSelectMenu()
    {

    }
}
