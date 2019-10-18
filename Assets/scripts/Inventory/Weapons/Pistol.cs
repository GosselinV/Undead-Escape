using UnityEngine;
using System.Collections;

public class Pistol : Weapon
{
	public Pistol(){
		itemType = ItemType.Weapon;
		itemName = ItemName.Pistol;
		attackRange = 4f;
		attackModifier = 3;
		attackDamage = 2;
		encumbrance = 1;
		currentAmmo = 6;
		maxAmmo = 6;
		toolTipString = "Weapon\n\n" +
			"Damage = \t" + attackDamage.ToString () + "\n" +
			"Range = \t" + attackRange.ToString () + "\n" +
			"Modifier = \t" + attackModifier.ToString() + "\n"+
			"Ecumbrance = \t" + encumbrance.ToString () + "\n" +
			"Ammo = \t" + currentAmmo.ToString ();
	}
}

