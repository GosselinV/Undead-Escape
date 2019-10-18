using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour {

	#region fields
	static ItemEvent addToInventoryEvent = new ItemEvent ();
	static ItemEvent addToEquippedEvent = new ItemEvent ();
	static ItemEvent removeEvent = new ItemEvent ();
	static ItemEvent unequipEvent = new ItemEvent ();
	static ItemEvent putItBackEvent = new ItemEvent ();
	static ItemEvent ReloadEvent = new ItemEvent();

	static IntEvent actionsMaxValueEvent = new IntEvent();
	static IntEvent encumbranceCurrentValueEvent = new IntEvent();
	static IntEvent encumbranceMaxValueEvent = new IntEvent ();
	static IntEvent expendActionEvent = new IntEvent ();
	static IntEvent useMedKitEvent = new IntEvent();
//	static IntEvent showActionsEvent = new IntEvent();

	static BoolEvent hudEvent = new BoolEvent();

	static Event turnOverEvent = new Event ();

	static StringEvent userFeedbackEvent = new StringEvent();

	static List<Item> inventory = new List<Item>();
	static List<Item> equipped = new List<Item>();

	static int maxEncumbrance;
	static int encumbrance;
	static int maxUsables;

	static Weapon equippedWeapon;
	static List<Item> usables = new List<Item>();

	static int playerActions=0; 

	#endregion

	#region properties
	static public List<Item> Inventory{
		get {return inventory;}
	}

	static public List<Item> Equipped{
		get { return equipped; }
	}

	#endregion

	#region init method
	public static void Initialize(){
		//EventManager.AddHudActionsEventListener (PlayerActions);
		EventManager.AddInventoryActionsListener (PlayerActions);
		AddEncumbranceCurrentValueEventListener (ChangeEncumbrance);
		AddEncumbranceMaxValueEventListener (ChangeMaxEncumbrance);
		equippedWeapon = new Fist ();
		//actions = GameConstants.DefaultPlayerActions;
		maxEncumbrance = GameConstants.DefaultEncumbranceMax;
		encumbrance = 0;
		maxUsables = 1;
	}
	#endregion

	#region Event Methods
	// InventoryUI.AddItem
	// LootCrateUI.AddToInventory
	static public void AddToInventory (Item item)
	{
		if (!inventory.Contains (item)) {
			if (item.Encumbrance <= maxEncumbrance - encumbrance) {
				inventory.Add (item);
				addToInventoryEvent.Invoke (item);
				encumbranceCurrentValueEvent.Invoke (item.Encumbrance);
			} else {
				// AUDIOSOURCE (fix so its only called once on takeall);
				userFeedbackEvent.Invoke("can't carry all this shit");
				Debug.Log ("can't carry all this shit");
			}
		}
	}

	// InventoryParentClass.EquipItem
	static public void AddToEquipped(Item item){
		Debug.Log ("adding to inventory");
		if (!equipped.Contains (item)) {
			if (CanOnlyBeOne(item)) {
				if (playerActions >= 1) {
					switch (item.Type) {
					case(ItemType.Weapon):
						equippedWeapon = (Weapon)item;
						break;
					case(ItemType.Usable):
						if (usables.Count < maxUsables) {
							usables.Add (item);
						} else {
							userFeedbackEvent.Invoke ("Can only equip one usable unless you wear a belt");
							Debug.Log ("Can only equip one usable unless you wear a belt");
							return;
						}
						break;
					case(ItemType.Gear):
						Gear gear = (Gear)item;
						if (item.Name == ItemName.Belt) {
							maxUsables = gear.NumUsableCarry;
						} else if (item.Name == ItemName.Backpack) {
							maxEncumbrance += gear.AdditionalEncumbrance;
							encumbranceMaxValueEvent.Invoke (gear.AdditionalEncumbrance);
						} else if (item.Name == ItemName.Boots) {
							Gear boots = (Gear)item;
							actionsMaxValueEvent.Invoke (boots.AdditionalActions);
						}
						break;
					}
					inventory.Remove (item);
					equipped.Add (item);
					addToEquippedEvent.Invoke (item);
					encumbranceCurrentValueEvent.Invoke (-item.Encumbrance);
//					playerEquipEvent.Invoke (item);
					expendActionEvent.Invoke (1);
				} else {
					//AUDIOSOURCE
					userFeedbackEvent.Invoke("not enough actions");
					Debug.Log ("not enough actions");
				}
			} else {
				//AUDIOSOURCE
				userFeedbackEvent.Invoke("Can only equip one of those");
				Debug.Log("Can only equip one of those");
			}
		}
	}

	// InventoryParentClass.UnequipItem
	static public void Unequip(Item item){
		if (equipped.Contains(item)){
			if (item.Encumbrance <= maxEncumbrance - encumbrance) {
				switch (item.Name){
				case(ItemName.Backpack):
					Gear backPack = (Gear)item;
					if (CheckBackPack (backPack)) {
						encumbranceMaxValueEvent.Invoke (-backPack.AdditionalEncumbrance);
					} else {
						// AUDIOSOURCE 
						userFeedbackEvent.Invoke("BackPack is full, empty some stuff before unequipping");
						Debug.Log ("BackPack is full, empty some stuff before unequipping");
						return;
					}
					break;

				case (ItemName.Boots):
					Gear boots = (Gear)item;
					actionsMaxValueEvent.Invoke (-boots.AdditionalActions);
					break;

				case(ItemName.Belt): 
					if (usables.Count > 1) {
						//AUDIOSOURCE
						userFeedbackEvent.Invoke("Unequip some usable items before you can take the belt off");
						Debug.Log ("Unequip some usable items before you can take the belt off");
						return;
					} else {
						maxUsables = 1;
					}
					break;

				case(ItemName.MedKit):
				case(ItemName.AmmoPistol):
				case(ItemName.AmmoShotgun):
					usables.Remove (item);
					break;
				}
				encumbranceCurrentValueEvent.Invoke (item.Encumbrance);
				equipped.Remove(item);
				inventory.Add(item);
				unequipEvent.Invoke(item);
			} else {
				// AUDIOSOURCE 
				userFeedbackEvent.Invoke("Why can't I hold all these useful things.. hue hue");
				Debug.Log("Why can't I hold all these useful things.. hue hue");
				return;
			}
		}
	}

	// InventoryParentClass.RemoveItem
	static public void Remove(Item item){
		if (equipped.Contains (item)) {
			switch (item.Name) {
			case (ItemName.Backpack):
				Gear backPack = (Gear)item;
				if (CheckBackPack (backPack)) {
					maxEncumbrance -= backPack.AdditionalEncumbrance;
				} else {
					// AUDIOSOURCE 
					userFeedbackEvent.Invoke("BackPack is full, empty some stuff before unequipping");
					Debug.Log ("BackPack is full, empty some stuff before unequipping");
					return;
				}
				break;
			case(ItemName.Boots):
				actionsMaxValueEvent.Invoke (-1);
				break;
			}
			equipped.Remove (item);
			removeEvent.Invoke (item);
			if (item.Type == ItemType.Usable) {
				usables.Remove (item);
			}
		} else if (inventory.Contains (item)) {
			inventory.Remove (item);
			encumbranceCurrentValueEvent.Invoke (-item.Encumbrance);
			removeEvent.Invoke (item);
		}
	}

	static public void Use(Item item){
		if (!equipped.Contains (item)) {
			//AUDIOSOURCE
			userFeedbackEvent.Invoke("Can only use equipped usables");
			Debug.Log("Can only use equipped usables");
			return;
		}
		Usable usable = (Usable)item;
		switch(item.Name){
		case(ItemName.MedKit):
			// Player.GetHealed
			useMedKitEvent.Invoke (usable.HealthReturn);
			Remove(item);
//			removeEvent.Invoke (item);
//			equipped.Remove (item);
			break;
		case(ItemName.AmmoPistol): 
			if (equippedWeapon.Name == ItemName.Pistol) {
				if (equippedWeapon.CurrentAmmo < 6) {
					equippedWeapon.CurrentAmmo = equippedWeapon.MaxAmmo;
					Remove(item);
					ReloadEvent.Invoke (equippedWeapon);
//					equipped.Remove (item);
//					removeEvent.Invoke (item);
				} else {
					//AUDIOSOURCE
					userFeedbackEvent.Invoke("why you wasting clips chump?");
					Debug.Log ("why you wasting clips chump?");
				}
			} else {
				//AUDIOSOURCE
				userFeedbackEvent.Invoke("Can't load a pistol clip in that!");
				Debug.Log ("Can't load a pistol clip in that!");
			}
			break;
		case(ItemName.AmmoShotgun): 
			if (equippedWeapon.Name == ItemName.Shotgun) {
				if (equippedWeapon.CurrentAmmo < 2) {
					equippedWeapon.CurrentAmmo = equippedWeapon.MaxAmmo;
					ReloadEvent.Invoke (equippedWeapon);
					Remove(item);
//					equipped.Remove (item);
//					removeEvent.Invoke(item);
				}
			} else {
				userFeedbackEvent.Invoke ("Can't Load this with shotgun shells!");
				Debug.Log ("Can't Load this with shotgun shells!");
			}
			break;
		default:
			userFeedbackEvent.Invoke ("Can't use a " + item.Name.ToString () + " ...");
			Debug.Log ("Can't use a " + item.Name.ToString () + " ...");
			break;
		}
	}

	static public void AddToInventoryEventListener(UnityAction<Item> listener){
		addToInventoryEvent.AddListener (listener);
	}

	static public void AddToEquippedEventListener(UnityAction<Item> listener){
		addToEquippedEvent.AddListener (listener);
	}

	// InventoryParentClass.UnequipItem
	// Player.Unequip
	static public void AddUnequipEventListener(UnityAction<Item> listener){
		unequipEvent.AddListener (listener);
	}

	static public void AddRemoveEventListener(UnityAction<Item> listener){
		removeEvent.AddListener (listener);
	}

	static public void AddPutItBackEventListener(UnityAction<Item> listener){
		putItBackEvent.AddListener (listener);
	}

	static public void AddEncumbranceCurrentValueEventListener(UnityAction<int> listener){
		encumbranceCurrentValueEvent.AddListener (listener);
	}

	static public void AddEncumbranceMaxValueEventListener(UnityAction<int> listener){
		encumbranceMaxValueEvent.AddListener (listener);
	}

	static public void AddExpendActionEventListener(UnityAction<int> listener){
		expendActionEvent.AddListener (listener);
	}

	static public void AddActionsMaxValueEventListener(UnityAction<int> listener){
		actionsMaxValueEvent.AddListener (listener);
	}
		
	static public void AddUseMedKitListener(UnityAction<int> listener){
		useMedKitEvent.AddListener (listener);
	}

	static public void AddAmmoEventListener(UnityAction<Item> listener){
		ReloadEvent.AddListener (listener);
	}

	static public void AddUserFeedBackEventListener(UnityAction<string> listener){
		userFeedbackEvent.AddListener (listener);
	}

	static public void AddTurnOverEventListener(UnityAction listener){
		turnOverEvent.AddListener (listener);
	}
	static public void AddHudEventListener(UnityAction<bool> listener){
		hudEvent.AddListener (listener);
	}

//	static public void AddShowActionsEventListener(UnityAction<int> listener){
//		showActionsEvent.AddListener (listener);
//	}
//
	#endregion

	#region utils methods
	static void ChangeEncumbrance(int changeEncumbrance){
		encumbrance += changeEncumbrance;
	}

	static void ChangeMaxEncumbrance(int changeMaxEncumbrance){
		maxEncumbrance += changeMaxEncumbrance;
	}

	static void PlayerActions(int actions){
		playerActions = actions;
//		showActionsEvent.Invoke (actions);
		//userFeedbackEvent.Invoke ("Inventory actions: " + actions.ToString ());
		if (actions <= 0) {
			userFeedbackEvent.Invoke ("Enemy Turn");
			hudEvent.Invoke (true);
			turnOverEvent.Invoke ();
		}
	}

	static bool CanOnlyBeOne(Item item){
		Item unequipItem = null;
		ItemType itemType = item.Type;
		foreach (Item equippedItem in equipped) {
			if (equippedItem.Type == itemType) {
				switch(itemType){
				case(ItemType.Weapon):
					unequipItem = equippedItem;
					break;
				case(ItemType.Armor):
					if (item.Name == equippedItem.Name) {
						return false;
					}
					break;
				case(ItemType.Gear):
					if (item.Name == equippedItem.Name) {
						return false;
					}
					break;
				}
			}
		}
		if (unequipItem != null) {
			unequipEvent.Invoke (unequipItem);
			equipped.Remove (unequipItem);
		}
		return true;
	}


	static bool CheckBackPack(Gear backPack){
		if (encumbrance <= maxEncumbrance - backPack.AdditionalEncumbrance - backPack.Encumbrance) {
			return true;
		}
		return false;
	}
	#endregion
}
