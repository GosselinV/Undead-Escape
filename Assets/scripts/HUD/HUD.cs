using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
	GameObject endTurnButton;

	void Awake(){
		EventManager.AddChangeTurnListener (HideEndTurnButton);
		endTurnButton = transform.Find ("EndTurnButton").gameObject;
	}

	void HideEndTurnButton(Creature turn){
		if (turn.Name != CreatureNames.player) {
			endTurnButton.SetActive (false);
		} else {
			endTurnButton.SetActive (true);
		}
	}
}

