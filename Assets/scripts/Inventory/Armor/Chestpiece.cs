using UnityEngine;
using System.Collections;

public class Chestpiece : Armor
{
	public Chestpiece(){
		itemType = ItemType.Armor;
		itemName = ItemName.Chestpiece;
		ac = 3;
		encumbrance = 3;
		toolTipString = "Armor\n\n"+
			" Armor Class Bonus: " + ac.ToString() + "\n" +
			" Encumbrance =: " + encumbrance.ToString();
	}
}

