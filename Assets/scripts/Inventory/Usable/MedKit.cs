using UnityEngine;
using System.Collections;

public class MedKit : Usable
{
	public MedKit(){
		itemType = ItemType.Usable;
		itemName = ItemName.MedKit;
		healthReturn = 1;
		encumbrance = 1;
		toolTipString = "Usable\n\n" +
			"A rudimentary first aid kit.\n" +
			"Patch your wounds to regain 1 health point.";
	}
}

