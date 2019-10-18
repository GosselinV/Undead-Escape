using UnityEngine;
using System.Collections;

static public class GameConstants
{
	// 2D support
	public static int creatureZ = -5;

	// map Config support
	static int currentMapSizeX;
	static int currentMapSizeY;
	public static Vector2 finishPos;

	//difficulty support
	static int difficulty = 0;
	static int defaultPlayerAC = 10;
	static int defaultPlayerHeatlh = 10;
	static int defaultPlayerActions = 8;
	static int defaultEnemyHealth = 5;
	static int passiveEnemyActions = 1;
	static int aggroEnemyActions = 2;
	static float enemyAggroRange = 5f;
	static int enemyAttackRange = 1;

	// path Finding support
	public static float OccupiedTileCost = 100f;
	public static int creatureSpeed = 10;
	static bool gamePause = false;

	// Other
	static int defaultNumUsableCarry = 1;
	static int defaultEncumbranceMax = 3;


	public static bool GamePause{
		set { gamePause = value; }
		get { return gamePause; }
	}

	static public int DefaultPlayerHealth{
		get {return defaultPlayerHeatlh;}
	}

	static public int DefaultPlayerActions{
		get { return defaultPlayerActions; }
	}

	static public int DefaultPlayerAC{
		get {return defaultPlayerAC;}
	}
		
	static public int DefaultEnemyHealth{
		get {return defaultEnemyHealth;}
	}

	static public int PassiveEnemyActions{
		get { return passiveEnemyActions; }
	}

	static public int AggroEnemyActions{
		get { return aggroEnemyActions; }
	}

	static public float EnemyAggroRange{
		get { return enemyAggroRange; }
	}

	static public int EnemyAttackRange{
		get { return enemyAttackRange; }
	}

	static public int CurrentMapSizeX{
		set { currentMapSizeX = value; }
		get { return currentMapSizeX; }
	}

	static public int CurrentMapSizeY{
		set { currentMapSizeY = value; }
		get { return currentMapSizeY; }
	}

	static public Vector2 FinishPos{
		set { finishPos = value; }
		get { return finishPos; }
	}

	static public int DefaultNumUsableCarry{
		get { return defaultNumUsableCarry; }
	}

	static public int DefaultEncumbranceMax{
		get { return defaultEncumbranceMax; }
	}

	static public int Difficulty{
		set { difficulty = value;
				switch (value) {
				case(0):
					defaultPlayerHeatlh = 10;
					defaultPlayerActions = 8;
					passiveEnemyActions = 1;
					aggroEnemyActions = 2;
					enemyAggroRange = 2f;
					break;
				case(1):
					defaultPlayerHeatlh = 8;
					defaultPlayerActions = 7;
					passiveEnemyActions = 1;
					aggroEnemyActions = 2;
					enemyAggroRange = 3f;
					break;
				case(2):
					defaultPlayerHeatlh = 7;
					defaultPlayerActions = 6;
					passiveEnemyActions = 1;
					aggroEnemyActions = 3;
					enemyAggroRange = 4f;
					break;
				case(3):
					defaultPlayerHeatlh = 5;
					defaultPlayerActions = 5;
					passiveEnemyActions = 2;
					aggroEnemyActions = 4;
					enemyAggroRange = 5f;
					break;
				}
			}
		get { return difficulty;}
	}
}

