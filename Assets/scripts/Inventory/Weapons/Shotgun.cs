using UnityEngine;
using System.Collections;

public class Shotgun : Weapon
{
	public Shotgun(){
		itemType = ItemType.Weapon;
		itemName = ItemName.Shotgun;
		attackRange = 3f;
		attackModifier = 5;
		attackDamage = 3;
		encumbrance = 2;
		currentAmmo = 2;
		maxAmmo = 2;
		toolTipString = "Weapon\n\n" +
			"Damage = \t" + attackDamage.ToString () + "\n" +
			"Range = \t" + attackRange.ToString () + "\n" +
			"Modifier = \t" + attackModifier.ToString() + "\n"+
			"Ecumbrance = \t" + encumbrance.ToString () + "\n";
		
	}
}

