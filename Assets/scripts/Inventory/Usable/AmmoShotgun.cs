using UnityEngine;
using System.Collections;

public class AmmoShotgun : Usable
{
	public AmmoShotgun(){
		itemType = ItemType.Usable;
		itemName = ItemName.AmmoShotgun;
		ammoAmount = 2;
		encumbrance = 1;
		toolTipString = "Usable\n\n"+
			"A pair of shotgun shells." +
			"Empty your current barell to\n" +
			"put those babies in.";

	}
}

