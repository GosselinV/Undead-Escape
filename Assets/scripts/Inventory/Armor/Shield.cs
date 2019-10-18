using UnityEngine;
using System.Collections;

public class Shield : Armor
{
	public Shield(){
		itemType = ItemType.Armor;
		itemName = ItemName.Shield;
		ac = 2;
		encumbrance = 2;
		toolTipString = "Armor\n\n"+
			" Armor Class Bonus: " + ac.ToString() + "\n" +
			" Encumbrance =: " + encumbrance.ToString();
	}
}

