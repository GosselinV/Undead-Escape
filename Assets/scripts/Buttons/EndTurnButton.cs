using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Button))]
public class EndTurnButton : MonoBehaviour
{
    StringEvent userFeedbackEvent = new StringEvent();
   	Event endTurnEvent = new Event();
	Button button;

	void Start(){
		EventManager.AddEndTurnInvoker (this);
		button = GetComponent<Button> ();
	}

	void Update(){
		if (Input.GetKey (KeyCode.Space)) {
			button.onClick.Invoke ();
		}
	}

	public void EndTurnHandle(){
		endTurnEvent.Invoke ();
        userFeedbackEvent.Invoke("Enemy Turn");
    }


	public void AddEndTurnListener(UnityAction listener){
		endTurnEvent.AddListener (listener);
	}

    public void AddEndTurnUserFeedbackListener(UnityAction<string> listener)
    {
        userFeedbackEvent.AddListener(listener);
    }
}

