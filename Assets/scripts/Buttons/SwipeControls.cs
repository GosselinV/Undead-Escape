using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControls : MenuButtons {
    public override void Handle()
    {
        base.Handle();
        //    if (cam.mainMenu != null)
        //    {
        //        cam.mainMenu.SetActive(true);
        //    }
        //    else if (cam.pauseMenu != null)
        //    {
        //        cam.pauseMenu.SetActive(true);
        //    }
        helpControls.SetActive(true);
//        if (cam.hud != null)
//        {
//            halfHudEvent.Invoke(false);
//            hudEvent.Invoke(false);
//        }
    }

}
