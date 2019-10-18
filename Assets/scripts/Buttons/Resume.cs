using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MenuButtons {

	public void ResumeHandle(){
		Time.timeScale = 1;
		GameConstants.GamePause = false;
		halfHudEvent.Invoke (false);
//		cam.hud.SetActive (true);
		hudEvent.Invoke(true);
		transform.parent.gameObject.SetActive (false);
	}
}
