using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

abstract public class InventoryParentClass : MonoBehaviour {

	protected string whoami;
	protected ItemEvent ReloadEvent = new ItemEvent();
	protected IntEvent useMedKitEvent = new IntEvent ();

	protected ToggleGroup toggleGroup;
	protected Button removeButton; 
	protected Button equipButton;
	protected Button unequipButton;
	protected Button useButton;
	Text actionsText;
	Text healthText;

	protected Transform panel;
	protected OrderedDictionary equipped = new OrderedDictionary();


	#region Init Methods
	protected virtual void Awake(){
		panel = gameObject.transform.Find ("Panel");
		if (panel == null) {
			Debug.Log ("houston");
		}
        toggleGroup = GetComponent<ToggleGroup>();
        removeButton = panel.transform.Find("Remove").GetComponent<Button>();
        equipButton = panel.transform.Find("Equip").GetComponent<Button>();
        unequipButton = panel.transform.Find("Unequip").GetComponent<Button>();
        useButton = panel.transform.Find("Use").GetComponent<Button>();

        //EventManager.AddStartingGearlistener (AddToInventory);

        EventManager.AddArmorClassEventListener (ArmorClass);
		EventManager.AddCloseInventoryEventListener (CloseInventory);
		EventManager.AddInventoryHealthListener (HealthText);
		EventManager.AddInventoryActionsListener (ActionsText);
		EventManager.AddInventoryAmmoEventListener (AmmoEvent);
		InventoryManager.AddAmmoEventListener (AmmoEvent);
		InventoryManager.AddToEquippedEventListener (EquipItem);
		InventoryManager.AddUnequipEventListener (UnequipItem);
		InventoryManager.AddRemoveEventListener (RemoveItem);
		InventoryManager.AddToInventoryEventListener (AddToInventory);
		InventoryManager.AddTurnOverEventListener (CloseInventory);
		actionsText = panel.transform.Find ("InventoryActions").GetComponent<Text> ();
		//InventoryManager.AddShowActionsEventListener (ActionsText);
		healthText = panel.transform.Find ("InventoryHealth").GetComponent<Text> ();
		EventManager.AddHudHealthEventListener (HealthText);
	}

	protected virtual void Start(){
		//toggleGroup = GetComponent<ToggleGroup> ();
		//removeButton = panel.transform.Find("Remove").GetComponent<Button>();
		//equipButton = panel.transform.Find ("Equip").GetComponent<Button> ();
		//unequipButton = panel.transform.Find ("Unequip").GetComponent<Button> ();
		//useButton = panel.transform.Find ("Use").GetComponent<Button> ();

	}

	#endregion

	#region ITEMS Methods
	protected abstract void AddToInventory (Item item);

	void UnequipItem (Item item){
		GameObject unequipItem = (GameObject)equipped [item];
		ResetPos (unequipItem);
		equipped.Remove (item);
		OrderedDictionary dict = GetDict (item);
		dict.Add (item, unequipItem);
		SetPos (item, unequipItem, item.Type.ToString ());
	}

	protected virtual void EquipItem(Item item){
		OrderedDictionary dict = GetDict (item);
		GameObject itemToEquip = (GameObject)dict [item];
		ResetPos (itemToEquip);
		dict.Remove (item);
		equipped.Add (item, itemToEquip); 
		SetPos (item, itemToEquip, "Equipped");
	}


	protected virtual void RemoveItem(Item item){
		GameObject itemObject = (GameObject)GetDict (item) [item];
		ResetPos (itemObject);
		GetDict (item).Remove (item);
		Destroy (itemObject);
	}
	#endregion

	#region Utils methods

	protected abstract OrderedDictionary GetDict (Item item);

	protected void ActionsText(int actions){
		actionsText.text = "Actions: "+ actions.ToString ();
	}

	protected void HealthText(int health){
		healthText.text = "Health: " + health.ToString ();
	}

	protected void ToggleText(Item item){
		GameObject newItem = (GameObject)GetDict (item) [item];
		Text text = newItem.transform.Find ("Label").GetComponent<Text> ();
		if (item.Type == ItemType.Weapon) {
			Weapon wp = (Weapon)item;
			if (wp.CurrentAmmo >= 0) {
				text.text = item.Name.ToString () + " (" + wp.CurrentAmmo + ")";					
				return;
			}
		}
		text.text = item.Name.ToString ();
	}

	protected virtual void SetPos(Item item, GameObject toggle, string category){
		Transform parent = panel.transform.Find (category);
		float deltaY = 50f;
		float deltaX = 20f;
        float ypos = 0;
		if (category == "Equipped") {
			string subcategory = item.Type.ToString ();
			parent = parent.transform.Find (subcategory);
			deltaY = 30f;
			deltaX = 20f;
		}
		toggle.transform.SetParent(parent, false);
        if (item.Type == ItemType.Armor & category == "Equipped")
        {
            ypos = (-parent.childCount + 1) * deltaY;
        }
        else
        {
            ypos = -parent.childCount * deltaY;
        }
        toggle.transform.localPosition = new Vector3 (deltaX, ypos, 0);
	}

	protected void ResetPos(GameObject toggle){
		Transform parent = toggle.gameObject.transform.parent;
		bool go = false;
		float deltaY;
		if (parent.gameObject.transform.parent.name == "Equipped") {
			deltaY = 30f;
		} else {
			deltaY = 50f;
		}
		for (int i = 0; i < parent.childCount; i++) {
			if (go) {
				parent.GetChild (i).gameObject.transform.localPosition += new Vector3 (0, deltaY, 0);
			} else if (parent.GetChild (i).gameObject == toggle) {
				go = true;
			}
		}
	}

	void ArmorClass(int AC){
		Text acText = panel.transform.Find ("Equipped").Find ("Armor").Find ("Text").gameObject.GetComponent<Text> ();
		acText.text = AC.ToString ();
	}

	void AmmoEvent(Item item){
		Weapon wp = (Weapon)item;
		GameObject toggle = (GameObject)GetDict (item)[item];
		Text text = toggle.transform.Find("Label").GetComponent<Text> ();
		text.text = item.Name.ToString () + " (" + wp.CurrentAmmo + ")";
	}

	void CloseInventory(){
		if (gameObject.transform.Find ("Panel").gameObject.activeSelf) {
			panel.gameObject.SetActive (false);
		}
	}
	#endregion

}
	
