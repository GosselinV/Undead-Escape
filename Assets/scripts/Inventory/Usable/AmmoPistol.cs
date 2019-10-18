using UnityEngine;
using System.Collections;

public class AmmoPistol : Usable
{
	public AmmoPistol(){
		itemType = ItemType.Usable;
		itemName = ItemName.AmmoPistol;
		ammoAmount = 6;
		encumbrance = 1;
		toolTipString = "Usable\n\n" +
			"A pistol Clip contains 6 bullets.\n"+
			"Lose any remaining bullets in your\n" +
			"current clip";

	}
}

