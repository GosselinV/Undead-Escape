using UnityEngine;
using System.Collections;

public class Boots : Gear
{
	public Boots(){
		itemType = ItemType.Gear;
		itemName = ItemName.Boots;
		additionalActions = 1;
		encumbrance = 1;
		toolTipString = "Gear\n\n" +
			"Boots makes it easier for you to move around granting\n"
			+ additionalActions.ToString() +
			" additional action per turn.";

	}
}

