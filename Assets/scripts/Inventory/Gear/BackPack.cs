using UnityEngine;
using System.Collections;

public class BackPack : Gear
{
	public BackPack(){
		itemType = ItemType.Gear;
		itemName = ItemName.Backpack;
		additionalEncumbrance = 3;
		encumbrance = 2;
		toolTipString = "Gear\n\n" +
			"A backpack allows you to carry\n" +
			"a lot more stuff. Double your\n" +
			" encumbrance with this item.";

	}
}

