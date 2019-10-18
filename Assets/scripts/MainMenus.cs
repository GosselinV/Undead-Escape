using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenus : GameManager {

    protected override void Awake()
    {
        base.Awake();
        ScreenUtils.Initialize();
    }
    void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (!GameConstants.GamePause) {
				PauseAll ();
				pauseMenu.SetActive (true);
				GameConstants.GamePause = true;
			} else {
				PauseAll ();
				GameConstants.GamePause = false;
				Time.timeScale = 1;
			}
		} else if (Input.GetKeyDown (KeyCode.C)) {
			Time.timeScale = 0;
			PauseAll ();
			helpControls.SetActive (true);
			GameConstants.GamePause = true;
		} else if (Input.GetKeyDown (KeyCode.H)) {
			Time.timeScale = 0;
			PauseAll ();
			helpAbout.SetActive (true);
			GameConstants.GamePause = true;
		} else if (Input.GetKeyDown (KeyCode.G)) {
			Time.timeScale = 0;
			PauseAll ();
			helpUI.SetActive (true);
			GameConstants.GamePause = true;
		} else if (Input.GetKeyDown (KeyCode.I)) {
			Time.timeScale = 0;
			PauseAll ();
			inventory.SetActive (true);
			GameConstants.GamePause = true;
		}
	}

}
