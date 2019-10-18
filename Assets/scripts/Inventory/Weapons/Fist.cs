using UnityEngine;
using System.Collections;

public class Fist : Weapon
{
	public Fist(){
		itemType = ItemType.Weapon;
		itemName = ItemName.Fist;
		attackRange = 1f;
		attackModifier = 0;
		attackDamage = 1;
		encumbrance = 0;
		currentAmmo = -1;
		maxAmmo = -1;
		toolTipString = "Weapon\n\n" +
			"Damage = \t" + attackDamage.ToString () + "\n" +
			"Range = \t" + attackRange.ToString () + "\n" +
			"Modifier = \t" + attackModifier.ToString() + "\n"+
			"Ecumbrance = \t" + encumbrance.ToString () + "\n";		
	}
}

