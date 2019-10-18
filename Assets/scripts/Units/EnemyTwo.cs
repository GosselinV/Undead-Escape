using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : Enemy {
	protected void Awake(){
		health = GameConstants.DefaultEnemyHealth-2;
		maxPassiveActions = GameConstants.PassiveEnemyActions+1;
		maxAggroActions = GameConstants.AggroEnemyActions+1;
		attackRange = GameConstants.EnemyAttackRange+2;
		base.Awake ();
	}
}
