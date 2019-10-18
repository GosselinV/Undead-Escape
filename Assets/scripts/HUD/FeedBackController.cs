using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackController : MonoBehaviour {

	static AttackFeedbackHandle attackFeedback;
	static UserFeedbackHandle userFeedback;
	static GameObject hud; 


	public static void Initialize(GameObject canvas){
		hud = canvas;
		attackFeedback = Resources.Load<AttackFeedbackHandle> ("prefabs/HUD/AttackFeedBack");
		userFeedback = Resources.Load<UserFeedbackHandle> ("prefabs/HUD/UserFeedBack");
		EventManager.AddAttackFeedBackListener (GenerateAttackFeedback);
		EventManager.AddUserFeedbackListener (GenerateUserFeedback);
		InventoryManager.AddUserFeedBackEventListener (GenerateUserFeedback);
	}

	public static void GenerateAttackFeedback(string text, Transform location){
		AttackFeedbackHandle instance = Instantiate (attackFeedback);
		instance.transform.SetParent (hud.transform);
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
		Debug.Log (location.position);
		Debug.Log (screenPosition);
		instance.transform.position = screenPosition;
		instance.newFeedback(text);

	}

	public static void GenerateUserFeedback(string text){
		UserFeedbackHandle instance = Instantiate (userFeedback);
		instance.newFeedback(text);
		instance.transform.SetParent (hud.transform);
		instance.transform.localPosition = new Vector3 (0, 200, -1);
	}

}
