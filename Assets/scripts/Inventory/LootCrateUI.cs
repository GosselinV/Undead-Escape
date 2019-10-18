using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LootCrateUI : InventoryParentClass{

	#region fields;
	ItemEvent lootCrateClickEvent = new ItemEvent();

	Weapon weapon;
	Armor armor;
	Gear gear;
	Usable usable;

	List<Item> containedItems = new List<Item>();
	OrderedDictionary takeMe = new OrderedDictionary();
	OrderedDictionary inventory = new OrderedDictionary();

	Button takeButton;
	Button takeAllButton;
	#endregion

	#region Initialize methods
	protected override void Awake(){
		whoami = "LootCrateUI";
		base.Awake ();
		InventoryManager.AddPutItBackEventListener (RemoveItem);
	}

	protected override void Start () {
		base.Start ();
		takeButton = panel.transform.Find ("Take").GetComponent<Button> ();
		takeAllButton = panel.transform.Find ("TakeAll").GetComponent<Button> ();
		AddItem (RandomItem ());
		AddItem (RandomItem ());
			
	}

	Item RandomItem(){
		Item randomItem = new Shirt ();

		switch (Random.Range (1, 10)) {
		case(1):
			switch (Random.Range (1, 5)) {
			case(1):
				randomItem = new Bat ();
				break;
			case(2):
				randomItem = new Knife ();
				break;
			case(3):
				randomItem = new Shotgun ();
				break;
			case(4):
				randomItem = new Pistol ();
				break;
			}
			break;
		case(2):
			switch (Random.Range (1, 4)) {
			case(1):
				randomItem = new Helmet ();
				break;
			case(2):
				randomItem = new Shield ();
				break;
			case(3):
				randomItem = new Chestpiece ();
				break;
			}
			break;
		case(3):
			switch (Random.Range (1, 4)) {
			case(1):
				randomItem = new Belt ();
				break;
			case(2):
				randomItem = new Boots ();
				break;
			case(3):
				randomItem = new BackPack ();
				break;
			}
			break;
		default:
			switch (Random.Range (1, 4)) {
			case(1):
				randomItem = new AmmoPistol ();
				break;
			case(2):
				randomItem = new AmmoShotgun ();
				break;
			case(3):
				randomItem = new MedKit ();
				break;
			}
			break;
		}
		return randomItem;

	}
	#endregion

	#region Item methods
	protected override void AddToInventory(Item item){
		if (!takeMe.Contains (item)) {
			AddItem (item);
		}
		if (!inventory.Contains (item)) {
			//AddItem (item);
			Take (item);
		}
	}
		
	void AddItem (Item item){
		// Instantiate and add to takeMe dictionary
		GameObject newItem = (GameObject)GameObject.Instantiate(Resources.Load("prefabs/Inventory/Toggle"));
		takeMe.Add(item, newItem);
		GameObject toolTip = newItem.transform.Find ("ToolTip").gameObject;
		Text toolTipString = toolTip.transform.Find ("Text").GetComponent<Text>();
		toolTipString.text = item.ToolTipString;
		toolTip.SetActive (false);

		// Handle Buttons handles
		ItemHandle itemHandle = newItem.GetComponent<ItemHandle> ();
		itemHandle.item = item;
		itemHandle.AddEquipItemListener (InventoryManager.AddToEquipped);
		itemHandle.AddUnequipItemListener (InventoryManager.Unequip);
		itemHandle.AddTakeItemListener (InventoryManager.AddToInventory);

		takeButton.onClick.AddListener (itemHandle.Take);
		takeAllButton.onClick.AddListener (itemHandle.TakeAll);
		ToggleText(item);
		Toggle toggle = newItem.GetComponent<Toggle> ();
		toggle.group = toggleGroup;

		// Place in LootCrate contents
		SetPos (item, newItem, "Contents");
	}
		
	void Take(Item item){
		GameObject takingItem = (GameObject)takeMe [item];
		ResetPos (takingItem);
		inventory [item] = takeMe [item];
		SetPos (item, takingItem, "Inventory");
		takeMe.Remove (item);
		ItemHandle itemHandle = takingItem.GetComponent<ItemHandle> ();
		itemHandle.AddRemoveItemListener (InventoryManager.Remove);
		//itemHandle.AddRemoveItemListener(RemoveItem);
		removeButton.onClick.AddListener (itemHandle.Remove);
		equipButton.onClick.AddListener (itemHandle.Equip);
		unequipButton.onClick.AddListener (itemHandle.UnEquip);
	}

	protected override void RemoveItem(Item item){
		base.RemoveItem (item);
		if (gameObject.transform.Find ("Panel").gameObject.activeSelf) {
			AddItem (item);
		}
	}
		
	protected override OrderedDictionary GetDict(Item item){
		if (equipped.Contains (item)) {
			return equipped;
		} else if (inventory.Contains (item)) {
			return inventory;
		}
		return takeMe;
	}
		

	protected override void SetPos(Item item, GameObject toggle, string category){
		if (category == item.Type.ToString ()) {
			category = "Inventory";
		}
		Transform parent = panel.transform.Find (category);
        float ypos = 0;
		float deltaY = 50f;
		float deltaX = 20f;
		if (category == "Equipped") {
			string subcategory = item.Type.ToString ();
			parent = parent.transform.Find (subcategory);
			deltaY = 30f;
			deltaX = 20f;
		}
		toggle.transform.SetParent(parent, false);
        if (item.Type == ItemType.Armor & category == "Equipped")
        {
            ypos = (-parent.childCount + 1)* deltaY;
        }
        else
        {
            ypos = -parent.childCount * deltaY;
        }
//		Debug.Log (parent.childCount.ToString ());
		toggle.transform.localPosition = new Vector3 (deltaX, ypos, 0);
	}

	#endregion
}
