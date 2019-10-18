using UnityEngine;
using System.Collections;

public class Belt : Gear
{
	public Belt(){
		itemType = ItemType.Gear;
		itemName = ItemName.Belt;
		numUsableCarry = 3;
		encumbrance = 1;
		toolTipString = "Gear\n\n" +
			"A belt makes it so you can equip up\n" +
			"to three usable at a time, allowing\n" +
			"you to use them as needed without expending\n" +
			"actions.";

	}
}

