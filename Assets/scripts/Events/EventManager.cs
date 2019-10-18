using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

static public class EventManager
{

	static MapManager mapManager;
	static UnitManager unitManager;
	static Player player;
	static List<Creature> creatureInvokers = new List<Creature>();
	static List<Enemy> enemyInvokers = new List<Enemy> ();
	//Mouse Over / Up tiles Events
	static List<Tile> tilesInvokers = new List<Tile>();
	static List<UnityAction<int, int>> mouseOverTileListeners = new List<UnityAction<int,int>> ();
	static List<UnityAction<int, int>> mouseUpTileListeners = new List<UnityAction<int, int>> ();
	// PathFinding Events
	static List<UnityAction<LinkedList<Node>>> pathFoundListeners = new List<UnityAction<LinkedList<Node>>> ();
	static List<UnityAction<Status>> moveButtonListeners = new List<UnityAction<Status>> ();
	static List<MoveButton> moveButtonInvokers = new List<MoveButton> ();
	// Turn base events
	static List<EndTurnButton> endTurnInvokers = new List<EndTurnButton>();
	static List<UnityAction> endTurnListeners = new List<UnityAction> ();
	static List<UnityAction> endTurnButtonListeners = new List<UnityAction>();
	static List<UnityAction<bool>> endTurnBoolListeners = new List<UnityAction<bool>> ();
	static List<UnityAction<Creature>> changeTurnListeners = new List<UnityAction<Creature>>();
	static List<UnityAction> enemyAIListeners = new List<UnityAction>();
	static List<UnityAction<int,int>> remainingActionsListeners = new List<UnityAction<int,int>>();
	// Player Attacks Event
	static List<UnityAction<int,int>> enemyClickListeners = new List<UnityAction<int,int>>();
	static List<UnityAction<int,int>> playerAttackListeners = new List<UnityAction<int,int>> ();
	static List<AttackButton> attackButtonInvokers = new List<AttackButton> ();
	static List<UnityAction<Status>> attackButtonListeners = new List<UnityAction<Status>>();
	static List<UnityAction<int>> hurtEnemyListeners = new List<UnityAction<int>>();
	static List<UnityAction<string, Transform>> attackFeedbackListeners = new List<UnityAction<string, Transform>>();
	// Starting Units Events
	static List<UnityAction<Vector2, List<Vector2>>> startingUnitsListeners = new List<UnityAction<Vector2, List<Vector2>>>();
	// Enemy AI 
	static List<UnityAction<int,int>> enemyAIEventListeners = new List<UnityAction<int,int>> ();
	static List<UnityAction<int,int>> enemyAttacksListeners = new List<UnityAction<int,int>>();
	// Occupation Events
	static List<UnityAction<int,int,bool>> occupyEventListeners = new List<UnityAction<int,int,bool>>();
	// UI Events
	static List<UnityAction<string>> userFeedbackListeners = new List<UnityAction<string>>();
	static List<UnityAction> uiEventListeners = new List<UnityAction>();
	static List<UnityAction<int>> hudHealthEventListeners = new List<UnityAction<int>>();
	static List<UnityAction<int>> hudActionsEventListeners = new List<UnityAction<int>>();
	static List<UnityAction<Item>> hudEquipWeaponListeners = new List<UnityAction<Item>> ();
	static List<UnityAction<int>> encumbranceCurrentValueEventListeners = new List<UnityAction<int>> ();
	static List<UnityAction<int>> encumbranceMaxValueEventListeners = new List<UnityAction<int>> ();
	static List<UnityAction<int>> actionsMaxValueEventListeners = new List<UnityAction<int>> ();
	static List<UnityAction<int>> armorClassEventListeners = new List<UnityAction<int>> ();
	// Inventory Events
	static List<LootCrate> lootCrateInvokers = new List<LootCrate> ();
	static List<UnityAction<int>> expendActionListeners = new List<UnityAction<int>> ();
	static List<MenuButtons> menuButtonsInvokers = new List<MenuButtons> ();
	static List<UnityAction<bool>> halfHudEventListeners = new List<UnityAction<bool>> ();
	static List<UnityAction<bool>> hudEventListeners = new List<UnityAction<bool>> ();
	static List<UnityAction<Item>> ammoEventListeners = new List<UnityAction<Item>> ();
	static List<UnityAction<int,int>> moveTowardsLootCrateListeners = new List<UnityAction<int,int>> ();
	static List<UnityAction> lootCrateClickListeners = new List<UnityAction> ();
	static List<UnityAction> closeInventoryListeners = new List<UnityAction> ();
	static List<UnityAction<int>> inventoryHealthListeners = new List<UnityAction<int>> ();
	static List<UnityAction<int>> inventoryActionsListeners = new List<UnityAction<int>>();
	// ========================================================================
	// 									INVOKERS
	// ========================================================================
	// MAP MANAGER
	static public void SetMapManager(MapManager mapMngr){
		mapManager = mapMngr;
		foreach (UnityAction<Vector2, List<Vector2>> listener in startingUnitsListeners) {
			mapManager.AddStartingUnitsEventListener (listener);
		}
		//foreach (UnityAction<Dictionary<int,LinkedList<Node>>> listener in pathFoundListeners) {
		foreach (UnityAction<LinkedList<Node>> listener in pathFoundListeners){
			mapManager.AddPathFoundEventListener (listener);
		}
	}

	// UNIT MANAGER
	static public void SetUnitManager(UnitManager unitMngr){
		unitManager = unitMngr;
		foreach (UnityAction<Creature> listener in changeTurnListeners) {
			unitManager.AddChangeTurnEventListener (listener);
		}
		foreach (UnityAction listener in enemyAIListeners) {
			unitManager.AddEnemyAIEventListener (listener);
		}
		foreach (UnityAction<int,int,bool> listener in occupyEventListeners) {
			unitManager.AddCreatureOccupyEventListener (listener);
		}
	}
		
	// CREATURES 
	static public void AddCreatureInvoker(Creature invoker){
		creatureInvokers.Add (invoker);
		foreach (UnityAction<int,int> listener in remainingActionsListeners) {
			invoker.AddRemainingActionEventListener(listener);
		}
		foreach (UnityAction<int,int,bool> listener in occupyEventListeners) {
			invoker.AddOccupyEventListener(listener);
		}
		foreach (UnityAction<bool> listener in endTurnBoolListeners) {
			invoker.AddEndTurnBoolListener (listener);
		}
		foreach (UnityAction listener in endTurnListeners) {
			invoker.AddEndTurnListener (listener);
		}
		foreach (UnityAction<string, Transform> listener in attackFeedbackListeners) {
			invoker.AddAttackFeedbackEventListener (listener);
		}
		foreach (UnityAction<string> listener in userFeedbackListeners) {
			invoker.AddUserFeedbackEventListener (listener);
		}
	}

	// ENEMIES 
	static public void AddEnemyInvoker(Enemy invoker){
		enemyInvokers.Add (invoker);
		foreach (UnityAction<int,int> listener  in enemyAttacksListeners) {
			invoker.AddAttackListener (listener);
		}
		foreach (UnityAction<int,int> listener in enemyAIEventListeners) {
			invoker.AddEnemyAIEventListener (listener);
		}
		foreach (UnityAction<int,int> listener in enemyClickListeners) {
			invoker.AddEnemyClickListener (listener);
		}
		foreach (UnityAction<int> listener in hurtEnemyListeners) {
			invoker.AddGetHurtEventListener (listener);
		}
	}

	// PLAYER
	static public void AddPlayerInvoker(Player plyr){
		player = plyr;
		foreach (UnityAction<int> listener in hudHealthEventListeners) {
			player.AddHudHealthListener (listener);
		}
		foreach (UnityAction<int> listener in hudActionsEventListeners) {
			player.AddHudActionsListener (listener);
		}
		foreach (UnityAction listener in uiEventListeners) {
			player.AddUiGameOverListener (listener);
		}
		foreach (UnityAction<int,int> listener in playerAttackListeners) {
			player.AddAttackEventListener (listener);
		}
		foreach (UnityAction<int,int> listener in mouseOverTileListeners) {
			player.AddHoveredTileListener (listener);
		}
		foreach (UnityAction<Item> listener in hudEquipWeaponListeners) {
			player.AddEquipWeaponListener (listener);
		}
		foreach (UnityAction<Item> listener in ammoEventListeners) {
			player.AddAmmoEventListener (listener);
		} 
		foreach (UnityAction<int> listener in armorClassEventListeners) {
			player.AddArmorClassEventListener (listener);
		}
		foreach (UnityAction listener in lootCrateClickListeners) {
			player.AddLootCrateClickEvent (listener);
		}
		foreach (UnityAction<int> listener in inventoryHealthListeners) {
			player.AddInventoryHealthEventListener (listener);
		}
		foreach (UnityAction<int> listener in inventoryActionsListeners) {
			player.AddInventoryActionsEventListener (listener);
		}
	}

	// LOOTCRATE
	static public void AddLootCrateInvoker(LootCrate invoker){
		lootCrateInvokers.Add (invoker);
		foreach (UnityAction<int> listener in expendActionListeners) {
			invoker.AddOpeningLootCrateEvent (listener);
		}
		foreach (UnityAction<int,int> listener in moveTowardsLootCrateListeners) {
			invoker.AddMovingTowardsLootCrateEvent (listener);
		}
		foreach (UnityAction<bool> listener in hudEventListeners) {
			invoker.AddHudEventListener (listener);
		}
		foreach (UnityAction<bool> listener in halfHudEventListeners) {
			invoker.AddHalfHudEventListener (listener);
		}
		foreach (UnityAction<string> listener in userFeedbackListeners) {
			invoker.AddUserFeedbackEventListener (listener);
		}

	}

	// TILES
	static public void AddTilesInvokers(Tile invoker){
		tilesInvokers.Add (invoker);
		foreach (UnityAction<int,int> listener in mouseOverTileListeners) {
			invoker.AddTileMouseOverListener (listener);
		}
		foreach (UnityAction<int, int> listener in mouseUpTileListeners) {
			invoker.AddTileMouseUpListener (listener);
		} 
	}

	// ATTACK BUTTON (hud)
	static public void AddAttackButtonInvoker(AttackButton invoker){
		attackButtonInvokers.Add (invoker);
		foreach (UnityAction<Status> listener in attackButtonListeners) {
			invoker.AddStateEventListener (listener);
		}
	}

	// MOVE BUTTON (hud)
	static public void AddMoveButtonInvoker(MoveButton invoker){
		moveButtonInvokers.Add (invoker);
		foreach (UnityAction<Status> listener in moveButtonListeners) {
			invoker.AddStateEventListener (listener);
		}
	}

	// END TURN BUTTON (hud)
	static public void AddEndTurnInvoker(EndTurnButton invoker){
		endTurnInvokers.Add (invoker);
		foreach (UnityAction listener in endTurnButtonListeners) {
			invoker.AddEndTurnListener (listener);
		}
        foreach(UnityAction<string> listener in userFeedbackListeners)
        {
            invoker.AddEndTurnUserFeedbackListener(listener);
        }
	}

	// MENU BUTTONS
	static public void AddMenuButtonInvoker(MenuButtons invoker){
		menuButtonsInvokers.Add (invoker);
		foreach (UnityAction<bool> listener in halfHudEventListeners) {
			invoker.AddHalfHudEventListener (listener);
		}
		foreach (UnityAction<bool> listener in hudEventListeners) {
			invoker.AddHudEventListener (listener);
		}
		foreach (UnityAction listener in closeInventoryListeners) {
			invoker.AddCloseInventoryEventListener (listener);
		}
	}


	// ========================================================================
	// 									LISTENERS
	// ========================================================================

	// ------------------------------------------------------------------------
	// 								   MAP MANAGER
	// ------------------------------------------------------------------------
	// Move Units 
	static public void AddPathFoundListener(UnityAction<LinkedList<Node>> listener){
		pathFoundListeners.Add (listener);
		if (mapManager != null) {
			mapManager.AddPathFoundEventListener (listener);
		}
	}
		
	// UnitsManager.AssignStartingPosition
	static public void AddStartingUnitsListener(UnityAction<Vector2, List<Vector2>> listener){
		startingUnitsListeners.Add (listener);
		if (mapManager != null) {
			mapManager.AddStartingUnitsEventListener (listener);
		}
	}


	// ------------------------------------------------------------------------
	// 								  UNIT MANAGER
	// ------------------------------------------------------------------------
	// Tile Occupation Handle (set cost)
	static public void AddOccupyEventListener(UnityAction<int,int,bool> listener){
		occupyEventListeners.Add (listener);
		if (unitManager != null) {
			unitManager.AddCreatureOccupyEventListener (listener);
		}
		foreach (Creature invoker in creatureInvokers) {
			invoker.AddOccupyEventListener (listener);
		}
	}

	// Cycle through Turn LinkedList
	static public void AddChangeTurnListener(UnityAction<Creature> listener){
		changeTurnListeners.Add (listener);
		if (unitManager != null) {
			unitManager.AddChangeTurnEventListener (listener);
		}
	}


	// Enemy AI Events
	static public void AddEnemyAIListener(UnityAction listener){
		enemyAIListeners.Add (listener);
		if (unitManager != null) {
			unitManager.AddEnemyAIEventListener (listener);
		}
	}


	// ------------------------------------------------------------------------
	// 								CREATURES
	// ------------------------------------------------------------------------
	// UnitManager.EndTurnEvent
	static public void AddEndTurnListener(UnityAction listener){
		endTurnListeners.Add (listener);
		foreach (Creature invoker in creatureInvokers) {
			invoker.AddEndTurnListener (listener);
		}
	}

	// UndeadEscape.ContinueButtonSetActive
	static public void AddEndTurnBoolListener(UnityAction<bool> listener){
		endTurnBoolListeners.Add (listener);
		foreach (Creature invoker in creatureInvokers) {
			invoker.AddEndTurnBoolListener (listener);
		}
	}

	// MapManager.AccessibleTileHandles
	static public void AddRemainingActionListener(UnityAction<int,int> listener){
		remainingActionsListeners.Add (listener);
		foreach (Creature invoker in creatureInvokers) {
			invoker.AddRemainingActionEventListener (listener);
		}
	}

	// ##################
	// 		PLAYER
	// ##################
	//InventoryParentClass.HealthText
	static public void AddInventoryHealthListener(UnityAction<int> listener){
		inventoryHealthListeners.Add (listener);
		if (player != null) {
			player.AddInventoryHealthEventListener (listener);
		}
	}

	//InventoryParentClass.ActionsText
	//InventoryManager.PlayerActions
	static public void AddInventoryActionsListener(UnityAction<int> listener){
		inventoryActionsListeners.Add (listener);
		if (player != null) {
			player.AddInventoryActionsEventListener (listener);
		}
	}

	// LootCrate.OpenLootCrate
	static public void AddLootCrateClickListener(UnityAction listener){
		lootCrateClickListeners.Add (listener);
		if (player != null) {
			player.AddLootCrateClickEvent (listener);
		}
	}

	// Enemy.GetHurt
	static public void AddPlayerAttackListener(UnityAction<int,int> listener){
		playerAttackListeners.Add (listener);
		if (player != null) {
			player.AddAttackEventListener (listener);
		}
	}

	// InventoryParentClass.
	static public void AddInventoryAmmoEventListener(UnityAction<Item> listener){
		ammoEventListeners.Add (listener);
		if (player != null) {
			player.AddAmmoEventListener (listener);
		}
	}

	// AttackButton.EquipNewWeapon
	static public void AddHudEquipWeaponListener(UnityAction<Item> listener){
		hudEquipWeaponListeners.Add (listener);
		if (player != null) {
			player.AddEquipWeaponListener (listener);
		}
	}

	//HealthBar.SetValue
	static public void AddHudHealthEventListener(UnityAction<int> listener){
		hudHealthEventListeners.Add (listener);
		if (player != null) {
			player.AddHudHealthListener (listener);
		}
	}

	// InventoryParentClass.PlayerActions
	// ActionsBar.SetValue
	static public void AddHudActionsEventListener(UnityAction<int> listener){
		hudActionsEventListeners.Add (listener);
		if (player != null) {
			player.AddHudActionsListener (listener);
		}
	}

//	// InventoryParentCalss.PlayerActions	
//	static public void AddInventoryExpendActionsEventListener(UnityAction<int> listener){
//		inventoryActionsListeners.Add (listener);
//		if (player != null) {
//			player.AddInventoryActionsEventListener (listener);
//		}
//	}

	// GameOverMenu.GameOver
	static public void AddUiEventListener(UnityAction listener){
		uiEventListeners.Add (listener);
		if (player != null) {
			player.AddUiGameOverListener (listener);
		}
	}

	// InventoryParentClass.
	static public void AddArmorClassEventListener(UnityAction<int> listener){
		armorClassEventListeners.Add (listener);
		if (player != null) {
			player.AddArmorClassEventListener (listener);
		}
	}


	// ##################
	// 		ENEMY
	// ##################
	// Player.GetHurt
	static public void AddAttacksListeners(UnityAction<int,int> listener){
		enemyAttacksListeners.Add (listener);
		foreach (Enemy invoker in enemyInvokers) {
			invoker.AddAttackListener(listener);
		}
	}

	// Player.Attack
	static public void AddEnemyClickListener(UnityAction<int,int> listener){
		enemyClickListeners.Add (listener);
		foreach (Enemy invoker in enemyInvokers){
			invoker.AddEnemyClickListener (listener);
		}
	}

	// EnemyHealthBar.SetValue
	static public void AddHurtEnemyListeners(UnityAction<int> listener){
		hurtEnemyListeners.Add (listener);
		foreach (Enemy invoker in enemyInvokers) {
			invoker.AddGetHurtEventListener (listener);
		}
	}

	// Enemy AI Events
	static public void AddEnemyAIEventListener(UnityAction<int,int> listener){
		enemyAIEventListeners.Add (listener);
		foreach (Enemy invoker in enemyInvokers) {
			invoker.AddEnemyAIEventListener (listener);
		}
	}


	// ------------------------------------------------------------------------
	// 								TILES
	// ------------------------------------------------------------------------
	// Tile hover
	static public void AddMouseOverTileListener(UnityAction<int,int> listener){
		mouseOverTileListeners.Add (listener);
		foreach (Tile invoker in tilesInvokers) {
			invoker.AddTileMouseOverListener (listener);
		}
		if (player != null) {
			player.AddHoveredTileListener (listener);
		}
	}

	// Tile Click
	static public void AddMouseUpTileListener(UnityAction<int,int> listener){
		mouseUpTileListeners.Add (listener);
		foreach (Tile invoker in tilesInvokers) {
			invoker.AddTileMouseUpListener (listener);
		}
	}


	// ------------------------------------------------------------------------
	// 								BUTTONS
	// ------------------------------------------------------------------------
	// Enemy.HideHealthBar
	// Player.SetState
	static public void AddMoveButtonListener(UnityAction<Status> listener){
		moveButtonListeners.Add (listener);
		foreach (MoveButton invoker in moveButtonInvokers) {
			invoker.AddStateEventListener (listener);
		}
	}

	// Enemy.HideHealthBar
	// Player.SetState
	static public void AddAttackButtonListener(UnityAction<Status> listener){
		attackButtonListeners.Add (listener);
		foreach (AttackButton invoker in attackButtonInvokers) {
			invoker.AddStateEventListener (listener);
		}
	}

	// Player.EndTurn
	static public void AddEndTurnButtonListener(UnityAction listener){
		endTurnButtonListeners.Add (listener);
		foreach (EndTurnButton invoker in endTurnInvokers) {
			invoker.AddEndTurnListener (listener);
		}
	}
	// InventoryParentClass.CloseInventory
	static public void AddCloseInventoryEventListener(UnityAction listener){
		closeInventoryListeners.Add (listener);
		foreach (MenuButtons invoker in menuButtonsInvokers) {
			invoker.AddCloseInventoryEventListener (listener);
		}
	}

	// UndeadEscape.SetHalfHudActive
	static public void AddHalfHudEventListener(UnityAction<bool> listener){
		halfHudEventListeners.Add (listener);
		foreach (MenuButtons invoker in menuButtonsInvokers) {
			invoker.AddHalfHudEventListener (listener);
		} 
		foreach (LootCrate invoker in lootCrateInvokers) {
			invoker.AddHalfHudEventListener (listener);
		}

	}

	// UndeadEscape.SetHudActive
	static public void AddHudEventListener(UnityAction<bool> listener){
		hudEventListeners.Add (listener);
		foreach (MenuButtons invoker in menuButtonsInvokers) {
			invoker.AddHudEventListener (listener);
		}
		foreach (LootCrate invoker in lootCrateInvokers) {
			invoker.AddHudEventListener (listener);
		}
	}



	// ------------------------------------------------------------------------
	// 								INVENTORY
	// ------------------------------------------------------------------------

	// ##################
	// 		LOOTCRATE
	// ##################

	// Player.ExpendAction
	static public void AddexpendActionListener(UnityAction<int> listener){
		expendActionListeners.Add (listener);
		foreach (LootCrate invoker in lootCrateInvokers) {
			invoker.AddOpeningLootCrateEvent (listener);
		}
	}


	// MapManager.FindPathTo
	static public void AddMoveTowardsLootCrateEventListener(UnityAction<int,int> listener){
		moveTowardsLootCrateListeners.Add (listener);
		foreach (LootCrate invoker in lootCrateInvokers) {
			invoker.AddMovingTowardsLootCrateEvent (listener);
		}
	}

	// ------------------------------------------------------------------------
	// 						USER INTERFACE (hud and whatnot)
	// ------------------------------------------------------------------------
	// FeedbackController.GenerateAttackFeedback
	static public void AddAttackFeedBackListener(UnityAction<string, Transform> listener){
		attackFeedbackListeners.Add (listener);
		foreach (Creature invoker in creatureInvokers) {
			invoker.AddAttackFeedbackEventListener (listener);
		}
	}

	// FeedbackController.GenerateUserFeedback
	static public void AddUserFeedbackListener(UnityAction<string> listener){
		userFeedbackListeners.Add (listener);
		foreach (Creature invoker in creatureInvokers) {
			invoker.AddUserFeedbackEventListener (listener);
		}
		foreach (LootCrate invoker in lootCrateInvokers) {
			invoker.AddUserFeedbackEventListener (listener);
		}
        foreach (EndTurnButton invoker in endTurnInvokers)
        {
            invoker.AddEndTurnUserFeedbackListener(listener);
        }
	}





	// ##################
	// 		STARTINGGEAR
	// ##################
	// 
	// 
//	static public void AddStartingGearlistener(UnityAction<Item> listener){
//		startingGearListeners.Add (listener);
//		if (startingGearInvoker != null) {
//			startingGearInvoker.AddTakeItemEventListener (listener);
//		}
//	}
}

