using UnityEngine;
using System.Collections;

public class Shirt : Armor
{
	public Shirt(){
		itemType = ItemType.Armor;
		itemName = ItemName.Shirt;
		ac = 1;
		encumbrance = 0;
		toolTipString = "Armor\n\n"+
			" Armor Class Bonus: " + ac.ToString() + "\n" +
			" Encumbrance =: " + encumbrance.ToString();
	}
}

