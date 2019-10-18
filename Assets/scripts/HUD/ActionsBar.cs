using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActionsBar : BarParentClass
{
	protected override void Awake(){
		
		EventManager.AddHudActionsEventListener(SetValue);
		InventoryManager.AddActionsMaxValueEventListener (ChangeMaxValue);
//		EventManager.AddActionsMaxValueEventListener (ChangeMaxValue);
		value = GameConstants.DefaultPlayerActions;
		maxValue = value;
		parent = gameObject.transform;
		iconPrefab = "prefabs/ActionsBar/ActionsUnitImg";
		missingIconPrefab = "prefabs/ActionsBar/MissingActionsUnitImg";
		base.Awake ();
	}

}