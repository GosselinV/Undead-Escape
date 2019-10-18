using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : Creature
{
	// EVENTS
	IntEvent HudHeatlh = new IntEvent();
	IntEvent HudActions = new IntEvent();
	IntEvent inventoryHealthEvent = new IntEvent ();
	IntEvent inventoryActionsEvent = new IntEvent();
	IntEvent armorClassEvent = new IntEvent();
	IntIntEvent attackEvent = new IntIntEvent ();
	IntIntEvent hoveredTileEvent = new IntIntEvent ();
	ItemEvent equippinGearEvent = new ItemEvent();
	ItemEvent equippingWeaponEvent = new ItemEvent();
	ItemEvent AmmoEvent = new ItemEvent ();
	Event lootCrateClickEvent = new Event ();
	Event gameOver = new Event();

	// inventory Support
	[SerializeField]
	int maxEncumbrance;
	[SerializeField]
	int numUsableCarry = GameConstants.DefaultNumUsableCarry;
	[SerializeField]
	int defaultEncumbranceMax = GameConstants.DefaultEncumbranceMax;

	// Fighting Support
	Weapon weapon;
	Weapon fist;
	List<Armor> armors = new List<Armor>();
	[SerializeField]
	List<Gear> gears = new List<Gear>();
	List<Usable> usables = new List<Usable>();
	CircleDraw drawAttackRange;



	void Awake(){
		base.Awake ();
		Debug.Log ("player awake");
		EventManager.AddPlayerInvoker (this);
		EventManager.AddAttacksListeners(GetHurt);
		EventManager.AddEnemyClickListener (Attack);
		EventManager.AddEndTurnButtonListener (endTurn);
		EventManager.AddAttackButtonListener (SetState);
		EventManager.AddMoveButtonListener (SetState);
//		EventManager.AddPlayerEquipListener (Equip);
//		EventManager.AddPlayerUnequipListener (Unequip);
//		EventManager.AddActionsMaxValueEventListener (AddMaxActions);
		EventManager.AddexpendActionListener (expendAction);
		//EventManager.AddUseMedKitListener (GetHealed);

		InventoryManager.AddActionsMaxValueEventListener (AddMaxActions);
		InventoryManager.AddToEquippedEventListener (Equip);
		InventoryManager.AddUnequipEventListener (Unequip);
		InventoryManager.AddUseMedKitListener (GetHealed);
		//		InventoryManager.AddPlayerEquipItemEventListener (Equip);
		InventoryManager.AddExpendActionEventListener (expendAction);

		health = GameConstants.DefaultPlayerHealth;
		maxActions = GameConstants.DefaultPlayerActions;
		name = CreatureNames.player;
		actions = maxActions;
		armorClass = GameConstants.DefaultPlayerAC;

		state = Status.move;
		drawAttackRange = GetComponent<CircleDraw> ();
	}

	void Start(){
		Debug.Log ("player start");
		base.Start ();
		inventoryHealthEvent.Invoke (health);
		inventoryActionsEvent.Invoke (actions);
		fist = new Fist ();
		Equip (fist);
		armorClassEvent.Invoke (armorClass);
		endTurnTimer.AddTimerFinishedListener (endTurn);
	}

	void Update(){
		base.Update ();
		if (myTurn) {
			if (new Vector2 (xpos, ypos) == GameConstants.FinishPos) {
				gameOver.Invoke ();
			}
		}
	}

	void Equip(Item item){
		switch (item.Type) {
		case(ItemType.Weapon):
			weapon = (Weapon)item;
			// AttackButton.EquipNewWeapon;
			equippingWeaponEvent.Invoke (weapon);
			break;
		case(ItemType.Usable):
			usables.Add ((Usable)item);
			break;
		case(ItemType.Armor):
			armors.Add ((Armor)item);
			Armor equippingArmor = (Armor)item;
			armorClass += equippingArmor.AC;
			armorClassEvent.Invoke (armorClass);
			break;
		case(ItemType.Gear):
			gears.Add ((Gear)item);
			Gear equippingGear = (Gear)item;
			maxActions += equippingGear.AdditionalActions;
			maxEncumbrance += equippingGear.AdditionalEncumbrance;
			numUsableCarry -= equippingGear.NumUsableCarry;
			break;
		}
		SetState (state);
	}

	void Unequip(Item item){
		switch (item.Type) {
		case (ItemType.Weapon):
			Debug.Log ("unequipping weapon");
			Equip (fist);
			SetState (state);
			break;
		case(ItemType.Usable):
			usables.Remove ((Usable)item);
			break;
		case(ItemType.Armor):
			armors.Remove ((Armor)item);
			Armor unequippingArmor = (Armor)item;
			armorClass -= unequippingArmor.AC;
			armorClassEvent.Invoke (armorClass);
			break;
		case(ItemType.Gear):
			gears.Remove ((Gear)item);
			Gear unequippingGear = (Gear)item;
			maxActions -= unequippingGear.AdditionalActions;
			maxEncumbrance -= unequippingGear.AdditionalEncumbrance;
			numUsableCarry = GameConstants.DefaultNumUsableCarry;
			break;
		}
	}

	protected override void GetHurt(int attackRoll, int damageRoll){
		if (attackRoll >= armorClass) {
//			health -= damageRoll;
//			HudHeatlh.Invoke (-damageRoll);
			HealthCHange(-damageRoll);
			attackFeedbackEvent.Invoke ("Hit", transform);


			if (health <= 0) {
				gameOver.Invoke ();
			}
		} else {
			attackFeedbackEvent.Invoke ("Miss", transform);
		}
	}

	protected void GetHealed(int hp){
		if (health == GameConstants.DefaultPlayerHealth) {
			//AUDIOSOURCE
			Debug.Log ("I guess I'll just put this decorative plaster on.. ");
		} else {
			//AUDIOSOURCE
			Debug.Log ("feelsgoodman");
			HealthCHange (hp);
			inventoryHealthEvent.Invoke (health);
//			health += hp;
//			HudHeatlh.Invoke (hp);
		}
	}

	protected override void Attack(int enemyX, int enemyY){
		if (myTurn) {

			if (state == Status.attack) {
				Collider2D[] cols = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), weapon.AttackRange, 1, -Mathf.Infinity, GameConstants.creatureZ);
				foreach (Collider2D col in cols) {
					if (col.tag == "creature") {
						Enemy potentialTarget = col.gameObject.GetComponent<Enemy> ();
						if (potentialTarget.XPOS == enemyX && potentialTarget.YPOS == enemyY) {
							if (actions >= 2) {
								if (weapon.CurrentAmmo == 0) {
									userFeedbackEvent.Invoke ("Out of ammo!");
									Debug.Log ("Out of ammo!");
								} else {
									int damage;
									int attackRoll = AttackRoll(weapon.AttackModifier);
									if (attackRoll == 20+weapon.AttackModifier){
										damage = weapon.AttackDamage * 2;
										attackRoll = 100;
									} else {
										damage = weapon.AttackDamage;
									}
									// Enemy.GetHurt();
									attackEvent.Invoke (attackRoll, weapon.AttackDamage);
									expendAction (2);
									if (weapon.CurrentAmmo > 0) {
										weapon.CurrentAmmo -= 1;
										// InventoryParentClass.AmmoEvent(Item);
										AmmoEvent.Invoke (weapon);
										// AttackButton.EquipNewWeapon;
										equippingWeaponEvent.Invoke (weapon);
									}
								}
							} else {
								//AUDIOSOURCE;
								userFeedbackEvent.Invoke ("Not enough actions for that!");
								Debug.Log ("Not enough actions for that");
							}
							return;
						}
					}
				}
				// AUDIOSOURCE
				userFeedbackEvent.Invoke ("Out of range");
				Debug.Log ("out of range");
			} else if (state == Status.move) {
			// AUDIOSOURCE
				userFeedbackEvent.Invoke ("Huh.. there's a zomber there buddy.. what you want from me?");
				Debug.Log("Huh.. there's a zomber there buddy.. what you want from me?");
				return;
			}
		}
	}
		
	protected override void MoveUnit(LinkedList<Node> tmPath){
		if (myTurn){
			if (state == Status.move) {
				if (tmPath.Count != 0) {
					pathCost = 0;
					targetNode = tmPath.First.Next;
					while (targetNode != null) {
						pathCost += targetNode.Value.Cost;
						targetNode = targetNode.Next;
					}
					targetNode = tmPath.Last;
					while (pathCost > actions && tmPath.Count > 1) {
						pathCost -= targetNode.Value.Cost;
						targetNode = targetNode.Previous;
						tmPath.RemoveLast ();
					}
					if (tmPath.Count > 1) {
						pathCost = 0;
						targetNode = tmPath.First.Next;
						isMoving = true;
						remainingActionEvent.Invoke (-1, -1);
						creatureOccupyEvent.Invoke (xpos, ypos, false);
					}
				} else {
					expendAction (1);
				}
			} else if (state == Status.attack) {
				// AUDIOSOURCE
				Debug.Log ("can't attack that");
			}
		} 
	}

	protected override void CheckLootCrate(){
		lootCrateClickEvent.Invoke ();
	}

	protected override void expendAction(int cost){
		actions -= cost;
		HudActions.Invoke (-cost);
		inventoryActionsEvent.Invoke (actions);
		if (actions == 0) {
			endTurnTimer.Run ();
		} else if (!isMoving) {
			// MapManager.AccessibleTilesHandle()
			remainingActionEvent.Invoke (xpos, ypos);
		}
	}

	void HealthCHange(int healthChange){
		health += healthChange;
		HudHeatlh.Invoke (healthChange);
		inventoryHealthEvent.Invoke (health);
	}

	protected override void endTurn(){
		remainingActionEvent.Invoke (-1, -1);
		base.endTurn ();
	}

	public override void MyTurn(){
		actions = maxActions;
		HudActions.Invoke (maxActions);
		inventoryActionsEvent.Invoke (actions);
	}

	public void SetState(Status state){
		this.state = state;
		if (state == Status.attack) {
			drawAttackRange.DestroyLine ();
			drawAttackRange.Draw (new Color32(70,145,40,255), weapon.AttackRange);
			//MapManager.HoverTileEvent()
			hoveredTileEvent.Invoke(xpos,ypos);
		} else if (state == Status.move) {
			drawAttackRange.DestroyLine ();
		}
	}

	void AddMaxActions(int actionChange){
		maxActions += actionChange;
		if (actions > maxActions) {
			actions = maxActions;
		}
	}

	public void AddAttackEventListener(UnityAction<int, int> listener){
		attackEvent.AddListener (listener);
	}

	public void AddHudHealthListener(UnityAction<int> listener){
		HudHeatlh.AddListener (listener);
	}

	public void AddHudActionsListener(UnityAction<int> listener){
		HudActions.AddListener (listener);
	}

	public void AddInventoryHealthEventListener(UnityAction<int> listener){
		inventoryHealthEvent.AddListener (listener);
	}

	public void AddInventoryActionsEventListener(UnityAction<int> listener){
		inventoryActionsEvent.AddListener (listener);
	}

	public void AddHoveredTileListener(UnityAction<int,int> listener){
		hoveredTileEvent.AddListener (listener);
	}

	public void AddUiGameOverListener(UnityAction listener){
		gameOver.AddListener (listener);
	}

	public void AddEquipWeaponListener(UnityAction<Item> listener){
		equippingWeaponEvent.AddListener (listener);
	}

	public void AddAmmoEventListener(UnityAction<Item> listener){
		AmmoEvent.AddListener (listener);
	}

	public void AddLootCrateClickEvent(UnityAction listener){
		lootCrateClickEvent.AddListener (listener);
	}

	public void AddArmorClassEventListener(UnityAction<int> listener){
		armorClassEvent.AddListener (listener);
	}

//	public void AddInventoryActionsEventListener(UnityAction<int> listener){
//		inventoryActionsEvent.AddListener (listener);
//	}
}

