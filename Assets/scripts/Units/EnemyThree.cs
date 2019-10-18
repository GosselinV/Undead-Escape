using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThree : Enemy {
	protected void Awake(){
		health = GameConstants.DefaultEnemyHealth+2;
		maxPassiveActions = GameConstants.PassiveEnemyActions;
		maxAggroActions = GameConstants.AggroEnemyActions-1;
		attackRange = GameConstants.EnemyAttackRange;
		base.Awake ();
	}
}
