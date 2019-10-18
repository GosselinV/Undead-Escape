using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Toggle))]
public class ItemHandle : MonoBehaviour {
	public Item item;
	//public LootCrateUI lootCrate;
	public InventoryParentClass inventory;
	Toggle toggle;
	ColorBlock cb;
	ItemEvent equipItemEvent = new ItemEvent();
	ItemEvent unequipItemEvent = new ItemEvent();
	ItemEvent takeItemEvent = new ItemEvent();
	ItemEvent removeItemEvent = new ItemEvent();
	ItemEvent putBackEvent = new ItemEvent();
	ItemEvent useItemEvent = new ItemEvent();
//	IntEvent removeItemEncumbranceEvent = new IntEvent();
//	GameObjectEvent removeItemEvent = new GameObjectEvent();


	// Use this for initialization
	void Awake () {
		toggle = gameObject.GetComponent<Toggle> ();
		cb = toggle.colors;
		toggle.isOn = false;
		toggle.onValueChanged.AddListener (OnValueChange);
	}

	public void Remove(){
		if (toggle.isOn) {
			removeItemEvent.Invoke (item);
		}
	}

	public void Equip(){
		if (toggle.isOn) {
			equipItemEvent.Invoke (item);
		}
	}

	public void UnEquip(){
		if (toggle.isOn) {
			unequipItemEvent.Invoke (item);
		}
	}

	public void Take(){
		if (toggle.isOn) {
			takeItemEvent.Invoke (item);
		}
	}

	public void TakeAll(){
		if (transform.parent.name == "Contents") {
			takeItemEvent.Invoke (item);
		}
	}

//	public void PutBack(){
//		if (toggle.isOn) {
//			putBackEvent.Invoke (item);
//		}
//	}
//
	public void Use(){
		if (toggle.isOn) {
			useItemEvent.Invoke (item);
		}
	}

	void OnValueChange(bool isOn){
		if (isOn) {
			cb.normalColor =  new Color32 (4, 165, 5, 255);
		} else {
			cb.normalColor = new Color32 (245, 245, 245, 255);
		}
		toggle.colors = cb;
	}

	public void AddEquipItemListener(UnityAction<Item> listener){
		equipItemEvent.AddListener (listener);
	}

	public void AddUnequipItemListener(UnityAction<Item> listener){
		unequipItemEvent.AddListener (listener);
	}

	public void AddTakeItemListener(UnityAction<Item> listener){
		takeItemEvent.AddListener (listener);
	}

	public void AddRemoveItemListener(UnityAction<Item> listener){
		removeItemEvent.AddListener (listener);
	}

	public void RemoveRemoveItemListener(UnityAction<Item> listener){
		removeItemEvent.RemoveListener (listener);
	}

//	public void AddPutBackItemListener(UnityAction<Item> listener){
//		putBackEvent.AddListener (listener);
//	}
//
	public void AddUseItemListener(UnityAction<Item> listener){
		useItemEvent.AddListener (listener);
	}
	//
//	public void AddRemoveItemEncumbranceListener(UnityAction<int> listener){
//		removeItemEncumbranceEvent.AddListener (listener);
//	}
}

