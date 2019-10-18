using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StartingGear : MonoBehaviour {

	Button takeButton;
	ToggleGroup toggleGroup;
	GameObject panel;
	List<GameObject> contents = new List<GameObject>();
//	ItemEvent takeItemEvent = new ItemEvent();
//	ItemEvent lootCrateMoveToInventoryEvent = new ItemEvent();

	void Awake(){
		panel = gameObject.transform.Find ("Panel").gameObject;
		takeButton = panel.transform.Find("Take").GetComponent<Button>();
		toggleGroup = GetComponent<ToggleGroup> ();
//		EventManager.StartingGearInvoker(this);

		AddItem (new Shotgun ());
		AddItem (new Pistol ());
		AddItem (new Belt ());
		AddItem (new Chestpiece ());
	}

	void Start () {
		foreach (GameObject item in contents) {

			Toggle toggle = item.GetComponent<Toggle> ();
			if (toggle.isOn) {
				Debug.Log (item.transform.Find ("Label").gameObject.GetComponent<Text> ().text);				
				ColorBlock cb = toggle.colors;
				cb.normalColor =  new Color32 (70, 145, 40, 255);
				toggle.colors = cb;
			}	
		}
	}

	void AddItem (Item item){
		GameObject newItem = (GameObject)GameObject.Instantiate(Resources.Load("prefabs/Inventory/Toggle"));
		contents.Add (newItem);
		GameObject toolTip = newItem.transform.Find ("ToolTip").gameObject;
		Text toolTipString = toolTip.transform.Find ("Text").GetComponent<Text>();
		toolTipString.text = item.ToolTipString;
		toolTip.SetActive (false);

		ItemHandle itemHandle = newItem.GetComponent<ItemHandle> ();
		itemHandle.item = item;
		itemHandle.AddTakeItemListener (InventoryManager.AddToInventory);
		itemHandle.AddTakeItemListener (Take);
		takeButton.onClick.AddListener (itemHandle.Take);
		newItem.transform.SetParent(panel.transform.Find("Contents"), false);
		Text text = newItem.transform.Find("Label").GetComponent<Text> ();
		text.text = item.Name.ToString ();
		Toggle toggle = newItem.GetComponent<Toggle> ();
		toggle.group = toggleGroup;
		float ypos = -contents.Count * 50;
		newItem.transform.localPosition = new Vector3 (0, ypos, 0);
	}

	void Take(Item Item){
		Camera.main.GetComponent<UndeadEscape> ().hud.SetActive (true);
//		Item shirt = new Shirt ();
//		InventoryManager.AddToInventory (shirt);
//		InventoryManager.AddToEquipped (shirt);
		Destroy (gameObject);
	}
//
//	public void AddTakeItemEventListener(UnityAction<Item> listener){
//		takeItemEvent.AddListener (listener);
//	}

}
