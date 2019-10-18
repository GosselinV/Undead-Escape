using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitLootCrateButton : MenuButtons {

	Event closeLootCrateEvent = new Event();

	public void Exit(){
		Time.timeScale = 1;
		GameConstants.GamePause = false;
//		Camera.main.GetComponent<UndeadEscape> ().hud.SetActive (true);
		hudEvent.Invoke(true);
		closeLootCrateEvent.Invoke ();
		closeInventoryEvent.Invoke ();
//		gameObject.transform.parent.gameObject.SetActive (false);
	}

	public void AddCloseLootCrateEventListener(UnityAction listener){
		closeLootCrateEvent.AddListener (listener);
	}
}
