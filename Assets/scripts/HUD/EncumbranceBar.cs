using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncumbranceBar : BarParentClass {
	[SerializeField]
	bool awake = false;

	protected override void Awake(){
		awake = true;
		InventoryManager.AddEncumbranceCurrentValueEventListener (SetValue);
		InventoryManager.AddEncumbranceMaxValueEventListener (ChangeMaxValue);		
//		EventManager.AddEncumbranceCurrentValueEventListener (SetValue);
//		EventManager.AddEncumbranceMaxValueEventListener (ChangeMaxValue);
		value = GameConstants.DefaultEncumbranceMax;
		maxValue = value;
		iconPrefab = "prefabs/Inventory/EncumbranceUnit";
		missingIconPrefab = "prefabs/Inventory/MissingEncumbranceUnit";
		parent = gameObject.transform;
		base.Awake ();
		SetValue (-maxValue);
	}

}
