using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
	void Awake(){
		EventManager.AddUiEventListener (GameOver);
	}

	void GameOver(){
		SceneManager.LoadScene ("map0");
	}
}

