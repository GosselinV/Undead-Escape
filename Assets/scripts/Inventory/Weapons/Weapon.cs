using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {

//	protected ItemName itemName;
//	protected ItemType itemType;
	protected int maxAmmo;
	protected int currentAmmo;
	protected float attackRange;
	protected int attackModifier;
	protected int attackDamage;

	public float AttackRange{
		get { return attackRange; }
	}

	public int AttackModifier{
		get { return attackModifier; }
	}

	public int AttackDamage{
		get { return attackDamage; }
	}

	public int CurrentAmmo{
		get { return currentAmmo; }
		set {	currentAmmo = value; 
				if (currentAmmo < 0) {
					currentAmmo = -1;
				}
			}
	}

	public int MaxAmmo{
		get { return maxAmmo; }
	}
}
