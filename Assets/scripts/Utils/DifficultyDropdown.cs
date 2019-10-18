using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DifficultyDropdown : MonoBehaviour
{
	[SerializeField]
	Dropdown dropdown;
	GameObject toddlerImg;
	GameObject walkImg;
	GameObject fightImg;
	GameObject bleedImg;
	List<string> difficulties = new List<string>() {"Toddling Away", "A Walk in the Park", "Itching for a Fight", "The Need to Bleed"};

	void Awake(){
		toddlerImg = GameObject.FindGameObjectWithTag ("0-toddler");
		walkImg = GameObject.FindGameObjectWithTag ("1-walk");
		fightImg = GameObject.FindGameObjectWithTag ("2-fight");
		bleedImg = GameObject.FindGameObjectWithTag ("3-bleed");
		//toddlerImg.SetActive (false);
		//walkImg.SetActive (false);
		//fightImg.SetActive (false);
		//bleedImg.SetActive (false);
	}

	// Use this for initialization
	void Start ()
	{
		dropdown.AddOptions (difficulties);
	}

	public void GetOptionHandle(int choice){
        choice = dropdown.value;
        GameConstants.Difficulty = choice;
		toddlerImg.SetActive (false);
		walkImg.SetActive (false);
		fightImg.SetActive (false);
		bleedImg.SetActive (false);	
		switch (choice) {
		case(0):
			toddlerImg.SetActive (true);
			break;
		case(1):
			walkImg.SetActive (true);
			break;
		case(2):
			fightImg.SetActive (true);
			break;
		case(3):
			bleedImg.SetActive (true);
			break;
		}
	}

	public void ShowCurrentDifficulty(){
		dropdown.value = GameConstants.Difficulty;
		toddlerImg.SetActive (false);
		walkImg.SetActive (false);
		fightImg.SetActive (false);
		bleedImg.SetActive (false);

		switch (GameConstants.Difficulty) {
		case(0):
			toddlerImg.SetActive (true);
			break;
		case(1):
			walkImg.SetActive (true);
			break;
		case(2):
			fightImg.SetActive (true);
			break;
		case(3):
			bleedImg.SetActive (true);
			break;
		} 
	}
}


