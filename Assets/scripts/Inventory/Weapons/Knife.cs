using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
	{
		public Knife(){
			itemType = ItemType.Weapon;
			itemName = ItemName.Knife;
			attackRange = 1f;
		 	attackModifier = 8;
			attackDamage = 2;
			encumbrance = 1;
			currentAmmo = -1;
			maxAmmo = -1;
			toolTipString = "Weapon\n\n" +
				"Damage = \t" + attackDamage.ToString () + "\n" +
				"Range = \t" + attackRange.ToString () + "\n" +
				"Modifier = \t" + attackModifier.ToString() + "\n"+
				"Ecumbrance = \t" + encumbrance.ToString () + "\n";		
		}
	}

