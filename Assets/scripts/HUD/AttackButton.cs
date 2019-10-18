using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AttackButton : MonoBehaviour {

	StatusEvent stateEvent = new StatusEvent();
	Text attackText;

	Toggle toggle;
	ColorBlock cb;

	void Awake(){
		EventManager.AddAttackButtonInvoker (this);
		EventManager.AddHudEquipWeaponListener (EquipNewWeapon);
		InventoryManager.AddAmmoEventListener (EquipNewWeapon);
		toggle = GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener (OnToggleValueChange);
		cb = toggle.colors;
		attackText = gameObject.transform.Find ("Label").gameObject.GetComponent<Text> ();

	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			if (!toggle.isOn) {
				toggle.isOn = !toggle.isOn;
			}
		}
	}

	void OnToggleValueChange(bool isOn){
		if (isOn) {
			cb.normalColor = new Color32 (70, 145, 40, 255);
			// Player.SetState()
			stateEvent.Invoke (Status.attack);
		} else {
			cb.normalColor = new Color (245, 245, 245, 255);
		}
		toggle.colors = cb;
	}

	void EquipNewWeapon(Item item){
		string weaponString;
		Weapon weapon = (Weapon)item;
		if (weapon.CurrentAmmo >= 0) {
			weaponString = "Attack\n(" + weapon.Name.ToString () + " - " + weapon.CurrentAmmo + ")";
		} else {
			weaponString = "Attack\n(" + weapon.Name.ToString ()+ ")";
		}
		attackText.text = weaponString;
	}


	public void AddStateEventListener(UnityAction<Status> listener){
		stateEvent.AddListener (listener);
	}
		
}
