using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeHelp : MenuButtons
{
    public override void Handle()
    {
        base.Handle();
        //if (cam.mainMenu != null)
        //{
        //    cam.mainMenu.SetActive(true);
        //    Debug.Log("mainMenu active");
        //}
        //else if (cam.pauseMenu != null)
        //{
        //    cam.pauseMenu.SetActive(true);
        //    Debug.Log("pauseMenu active");
        //}
        helpAbout.SetActive(true);
        //cam.mainMenu.SetActive(false);
        //gameObject.transform.parent.gameObject.SetActive(false);
        //Instantiate(Resources.Load("prefabs/Menus/HelpAbout"));
        //if (cam.hud != null)
        //{
        //    halfHudEvent.Invoke(false);
        //    Debug.Log("things");
        //    hudEvent.Invoke(false);
        //    Debug.Log("stuff");
        //}
    }
    
}
