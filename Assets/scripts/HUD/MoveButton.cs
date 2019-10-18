using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MoveButton : MonoBehaviour {

	StatusEvent stateEvent = new StatusEvent();
	Toggle toggle;
	ColorBlock cb;

	void Awake(){
		EventManager.AddMoveButtonInvoker (this);
		toggle = gameObject.GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener (OnToggleValueChange);
		cb = toggle.colors;
		cb.normalColor =  new Color32 (70, 145, 40, 255);
		toggle.colors = cb;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			if (!toggle.isOn) {
				toggle.isOn = !toggle.isOn;
			}
		}
	}

	void OnToggleValueChange(bool isOn){
		if (isOn) {
			cb.normalColor =  new Color32 (70, 145, 40, 255);
			// Player.SetState()
			stateEvent.Invoke (Status.move);
		} else {
			cb.normalColor = new Color32 (245, 245, 245, 255);
		}
		toggle.colors = cb;
	}

	public void AddStateEventListener(UnityAction<Status> listener){
		stateEvent.AddListener (listener);
	}
}
