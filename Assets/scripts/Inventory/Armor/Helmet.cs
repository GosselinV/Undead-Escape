using UnityEngine;
using System.Collections;

public class Helmet : Armor
{
	public Helmet(){
		itemType = ItemType.Armor;
		itemName = ItemName.Helmet;
		ac = 2;
		encumbrance = 1;
		toolTipString = "Armor\n\n"+
			" Armor Class Bonus: " + ac.ToString() + "\n" +
			" Encumbrance =: " + encumbrance.ToString();
	}
}

