using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MenuButtons{

	public override void Handle(){
        //if (cam.mainMenu != null) {
        //	cam.mainMenu.SetActive (true);
        //} 
        //else if (cam.pauseMenu != null){
        //	cam.pauseMenu.SetActive (true);
        //}
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
        }
        base.Handle();
//		if (cam.hud != null) {
//			halfHudEvent.Invoke (false);
//			hudEvent.Invoke (false);
//			cam.hud.SetActive (false);
	}
}
