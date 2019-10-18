using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Restart : MonoBehaviour
{

	public void PlayAgain(){
		GameConstants.GamePause = false;
		SceneManager.LoadScene("map0");
	}
}

