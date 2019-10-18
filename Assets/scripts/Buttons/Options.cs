using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour
{
	GameObject mainMenu;
	GameObject optionsMenu;


	void Awake(){
		mainMenu = transform.parent.gameObject;
		optionsMenu = GameObject.FindGameObjectWithTag ("Options");

	}

	void Start(){
		optionsMenu.SetActive (false);
	}
	public void LoadOptions(){
		optionsMenu.SetActive (true);
		Camera.main.GetComponent<DifficultyDropdown> ().ShowCurrentDifficulty ();
		mainMenu.SetActive (false);
	}
		
}

