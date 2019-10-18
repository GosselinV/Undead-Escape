using UnityEngine;
using System.Collections;

public class Bat : Weapon
{
	public Bat(){
		itemType = ItemType.Weapon;
		itemName = ItemName.Bat;
		attackRange = 2f;
		attackModifier = 5;
		attackDamage = 1;
		encumbrance = 2;
		currentAmmo = -1;
		maxAmmo = -1;
		toolTipString = "Weapon\n\n" +
			"Damage = \t" + attackDamage.ToString () + "\n" +
			"Range = \t" + attackRange.ToString () + "\n" +
			"Modifier = \t" + attackModifier.ToString() + "\n"+
			"Ecumbrance = \t" + encumbrance.ToString () + "\n";		
	}
}

