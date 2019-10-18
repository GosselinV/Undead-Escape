using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar: BarParentClass
{

	protected override void Awake(){
		EventManager.AddHudHealthEventListener (SetValue);
		value = GameConstants.DefaultPlayerHealth;
		maxValue = GameConstants.DefaultPlayerHealth;
		iconPrefab = "prefabs/HealthBar/HealthUnitImg";
		missingIconPrefab = "prefabs/HealthBar/MissingHealthUnitImg";
		parent = gameObject.transform;
		base.Awake ();

	}

}