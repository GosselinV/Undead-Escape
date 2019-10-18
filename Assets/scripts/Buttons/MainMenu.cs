using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void MainMenuHandle(){
		GameConstants.GamePause = false;
		SceneManager.LoadScene ("MainMenu");
	}
}
