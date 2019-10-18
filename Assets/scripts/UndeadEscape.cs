using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UndeadEscape : GameManager {

	Event closeLootCrateEvent = new Event();

	protected override void Awake(){
		base.Awake ();
		EventManager.AddEndTurnBoolListener(ContinueButtonSetActive);
		EventManager.AddHalfHudEventListener (SetHalfHudActive);
		EventManager.AddHudEventListener (SetHudActive);
		hud = (GameObject)GameObject.FindGameObjectWithTag ("HUD");
		FeedBackController.Initialize (hud);
		InventoryManager.Initialize ();
		InventoryManager.AddHudEventListener (SetHudActive);
	}

	protected override void Start(){
		gameState = GameState.Running;
		if (hud != null) {
			hudEndTurnButton = hud.transform.Find ("EndTurnButton").gameObject;
			hudActionsPanel = hud.transform.Find ("ActionsPanel").gameObject;
			hudHealthText = hud.transform.Find ("Health").gameObject.GetComponent<Text> ();
			hudActionsText = hud.transform.Find ("Actions").gameObject.GetComponent<Text> ();
			hud.SetActive (false);
		}
		if (hudEndTurnButton != null) {
			hudEndTurnButton.SetActive (true);
		}
		base.Start ();

	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (!GameConstants.GamePause) {
				//Time.timeScale = 0;
				PauseAll ();
				pauseMenu.SetActive (true);
				GameConstants.GamePause = true;
			} else {
				PauseAll ();
				if (hud != null) {
					SetHalfHudActive (false);
					SetHudActive (true);
					closeLootCrateEvent.Invoke ();
//					hud.SetActive (true);
				}
				GameConstants.GamePause = false;
				//Time.timeScale = 1;
			}
		} else if (Input.GetKeyDown (KeyCode.C)) {
			//Time.timeScale = 0;
			PauseAll ();
			helpControls.SetActive (true);
			GameConstants.GamePause = true;
		} else if (Input.GetKeyDown (KeyCode.H)) {
			//Time.timeScale = 0;
			PauseAll ();
			helpAbout.SetActive (true);
			GameConstants.GamePause = true;
		} else if (Input.GetKeyDown (KeyCode.G)) {
			//Time.timeScale = 0;
			PauseAll ();
			helpUI.SetActive (true);
			GameConstants.GamePause = true;
		} else if (Input.GetKeyDown (KeyCode.I)) {
            //Time.timeScale = 0;
            if (!inventory.activeSelf)
            {
                PauseAll();
                inventory.SetActive(true);
                SetHalfHudActive(true);
                GameConstants.GamePause = true;
            } else
            {
                inventory.SetActive(false);
                SetHalfHudActive(false);
                SetHudActive(true);
                GameConstants.GamePause = false;
            }
		}
	}

	protected override void PauseAll(){
		base.PauseAll ();
		if (hud != null) {
//			hud.SetActive (false);
			SetHudActive(false);
		}
		foreach (GameObject lootCrate in lootCrates) {
			lootCrate.transform.Find ("LootCrateUI").gameObject.transform.Find ("Panel").gameObject.SetActive (false);
		}

	}

	void SetHudActive(bool active){
		for (int i = 0; i < hud.transform.childCount; i++) {
			hud.transform.GetChild (i).gameObject.SetActive (active);
		}
	}

	void SetHalfHudActive(bool active){
		if (hud != null) {
			if (!active) {

				hudEndTurnButton.SetActive (!active);
				hudActionsPanel.SetActive (!active);
				hudHealthText.text = "Health :";
				hudActionsText.text = "Actions :";
	//			hud.SetActive (active);
				SetHudActive(active);
			} else {
//				hud.SetActive (active);
				SetHudActive (active);
				hudEndTurnButton.SetActive (!active);
				hudActionsPanel.SetActive (!active);
				hudHealthText.text = " ";
				hudActionsText.text = " ";
			}
		}
	}


	void ContinueButtonSetActive(bool active){
		hudEndTurnButton.SetActive (active);
	}

	public void AddLootCrate(GameObject lootCrate){
		lootCrates.Add (lootCrate);
	}

	public void AddCloseLootCrateEventListener(UnityAction listener){
		closeLootCrateEvent.AddListener (listener);
	}
}
