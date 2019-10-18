using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy {
	protected void Awake(){
		health = GameConstants.DefaultEnemyHealth;
		maxPassiveActions = GameConstants.PassiveEnemyActions;
		maxAggroActions = GameConstants.AggroEnemyActions;
		attackRange = GameConstants.EnemyAttackRange;
		base.Awake ();
	}

}
