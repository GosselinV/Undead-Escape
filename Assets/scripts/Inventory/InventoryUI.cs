using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryUI : InventoryParentClass {

	#region fields
	OrderedDictionary weapons = new  OrderedDictionary();
	OrderedDictionary armor = new OrderedDictionary();
	OrderedDictionary gear = new OrderedDictionary();
	OrderedDictionary usable = new OrderedDictionary();
	#endregion

	#region init methods
	protected override void Awake ()
	{
		whoami = "InventoryUI";
		base.Awake ();
	}
	#endregion

	#region Item methods
	protected override void AddToInventory (Item item){
		OrderedDictionary dict = GetDict (item);
		GameObject newItem = (GameObject)GameObject.Instantiate (Resources.Load ("prefabs/Inventory/Toggle"));
		GameObject toolTip = newItem.transform.Find ("ToolTip").gameObject;
		Text toolTipString = toolTip.transform.Find ("Text").GetComponent<Text> ();
		toolTipString.text = item.ToolTipString;
		toolTip.SetActive (false);
		ItemHandle itemHandle = newItem.GetComponent<ItemHandle> ();
		itemHandle.item = item;
		itemHandle.AddEquipItemListener(InventoryManager.AddToEquipped);
		itemHandle.AddUnequipItemListener (InventoryManager.Unequip);
		itemHandle.AddRemoveItemListener (InventoryManager.Remove);
		itemHandle.AddUseItemListener (InventoryManager.Use);
		removeButton.onClick.AddListener (itemHandle.Remove);
		equipButton.onClick.AddListener (itemHandle.Equip);
		unequipButton.onClick.AddListener (itemHandle.UnEquip);
		useButton.onClick.AddListener (itemHandle.Use);
		Toggle toggle = newItem.GetComponent<Toggle> ();
		toggle.group = toggleGroup;
		dict.Add (item, newItem);
		ToggleText (item);
		SetPos (item, newItem, item.Type.ToString ());
	}
	#endregion

	#region utils methods
	protected override OrderedDictionary GetDict(Item item){
		if (equipped.Contains (item)) {
			return equipped;
		} else if (item.Type == ItemType.Weapon) {
			return weapons;
		} else if (item.Type == ItemType.Armor) {
			return armor;
		} else if (item.Type == ItemType.Gear) {
			return gear;
		} else if (item.Type == ItemType.Usable) {
			return usable;
		}
		return null;
	}
	#endregion
}
