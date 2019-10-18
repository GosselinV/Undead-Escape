using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Creature {



	#region fields
	// EVENTS
	IntIntEvent enemyMoveEvent = new IntIntEvent();
	IntIntEvent hurtPlayer = new IntIntEvent();
	IntEvent getHurt = new IntEvent ();
	IntIntEvent enemyClick = new IntIntEvent();

	// Player Attacks support
	[SerializeField]
	bool isClicked = false;


	// AI support
	//[SerializeField]
	//Status state;
	Player player;
	Timer aiTimer;
	protected int maxPassiveActions;
	protected int maxAggroActions;
	protected int attackRange;
	int playerPosX;
	int playerPosY;

	int x;
	int y;

	// State UI
	Material passiveMaterial; 
	Material passiveHoverMaterial; 
	Material aggroMaterial; 
	Material aggroHoverMaterial;
	Material myTurnMaterial;
	Material currentMaterial;

	#endregion

	#region properties
	public Status State{
		get { return state; }
	}

	Material Mat{
		set { GetComponent<Renderer> ().material = value;}
		get { return GetComponent<Renderer> ().material; }
	}

	#endregion

	#region monobehavior Methods
	protected void Awake(){
		base.Awake ();

		name = CreatureNames.enemy;
		actions = 0;
		aiTimer = gameObject.AddComponent<Timer> ();
		aiTimer.Duration = 0.1f;
		aiTimer.AddTimerFinishedListener (AISwitch);
		EventManager.AddEnemyInvoker (this);
		EventManager.AddEnemyAIListener(EnemyAI);
		EventManager.AddPlayerAttackListener (GetHurt);
		EventManager.AddAttackButtonListener (ShowHealthBar);
		EventManager.AddMoveButtonListener (HideHealthBar);
		passiveMaterial = (Material)Resources.Load("prefabs/Units/Material/Enemies", typeof(Material));
		passiveHoverMaterial = (Material)Resources.Load("prefabs/Units/Material/EnemiesHover", typeof(Material));
		aggroMaterial = (Material)Resources.Load ("prefabs/Units/Material/EnemiesAggro", typeof(Material));
		aggroHoverMaterial = (Material)Resources.Load ("prefabs/Units/Material/EnemiesAggroHover", typeof(Material));
		myTurnMaterial = (Material)Resources.Load ("prefabs/Units/Material/EnemiesTurn", typeof(Material));
		currentMaterial = passiveMaterial;
		armorClass = 10;
		isClicked = false;
	}

	void Start(){
		base.Start ();
		state = Status.passive;
		endTurnTimer.AddTimerFinishedListener (endTurn);
		player = GameObject.FindGameObjectWithTag ("player").GetComponent<Player>();
	}

	#endregion

	#region AI methods
	void EnemyAI(){
		if (myTurn) {
			Mat = myTurnMaterial;
			SetState ();
			aiTimer.Run ();
		}
	}
		
	void SetState(){
		Collider2D[] cols = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), GameConstants.EnemyAggroRange, 1, -Mathf.Infinity, GameConstants.creatureZ);
		if (state == Status.attack) {
			state = Status.aggro;
		}
		foreach (Collider2D col in cols) {
			if (col.gameObject.tag == "player") {
				Debug.Log ("player detected");
				//playerPosX = col.gameObject.GetComponent<Player> ().XPOS;
				//playerPosY = col.gameObject.GetComponent<Player> ().YPOS;
				if (state == Status.passive) {
					currentMaterial = aggroMaterial;
					Mat = aggroMaterial;
				}
				state = Status.aggro;
				if (actions == 0) {
					actions = maxAggroActions;
					Debug.Log ("I HAVE ACTIONS!!! :D");
				}
//				if (isNeighbor (playerPosX, playerPosY)) {
//					state = Status.attack;
//				if (isInRange (player)) {
//					state = Status.attack;
//				} else {
//					state = Status.aggro;
//				}
//
				return;
			}
		}
		state = Status.passive;
		Mat = passiveMaterial;
		currentMaterial = passiveMaterial;
		if (actions == 0) {
			actions = maxPassiveActions;
		}
	}

	void AISwitch(){
		switch (state){
		case (Status.passive):
			RandomMotion ();

			// Make sure we remain on the map
			if (CheckMoveValidity ()) {
				//MapManager.FindPathTo(x,y)
				enemyMoveEvent.Invoke (x, y);
			} else {
				expendAction (1);
			}
			break;
		case (Status.aggro):
			Debug.Log ("MoveTowardsPlayer()");
			MoveTowardsPlayer ();
			break;
		case (Status.attack):
			Attack (0,0);
			break;
		}
	}

	protected override void Attack(int dummy, int dummer){
		int attackRoll = AttackRoll(0);
		hurtPlayer.Invoke (attackRoll,1);
		expendAction (1);
	}

	void MoveTowardsPlayer(){
		// MapManager.FindPathTo();
		Debug.Log(player.XPOS.ToString() + " " + player.YPOS.ToString());
		enemyMoveEvent.Invoke (player.XPOS, player.YPOS);
	}

	protected override void MoveUnit(LinkedList<Node> tmPath){
		if (myTurn) {
			LinkedListNode<Node> lastTarget;
			LinkedListNode<Node> nextTarget;

			lastTarget = null;
			if (tmPath.Count <= 0) {
				expendAction (1);
				return;
			}

			bool playerLastTarget = false;
			lastTarget = tmPath.Last;
			//Debug.Log (tmPath.Last.Value.XPOS.ToString () + " " + tmPath.Last.Value.YPOS.ToString ());
			nextTarget = tmPath.First.Next;
			pathCost = 0;

			while (nextTarget != null) {
				pathCost += nextTarget.Value.Cost;
				nextTarget = nextTarget.Next;
			}

			if (tmPath.Count > 1) {
				if (lastTarget.Value.XPOS == player.XPOS && lastTarget.Value.YPOS == player.YPOS) {
					playerLastTarget = true;
					if (tmPath.Count <= attackRange + 1) {
						Debug.Log ("count <= attackrange");
						state = Status.attack;
						AISwitch ();
						return;
					} else {
						Debug.Log ("count > attackrange");
						for (int i = 0; i < attackRange; i++) {
							pathCost -= lastTarget.Value.Cost;
							lastTarget = lastTarget.Previous;
							tmPath.RemoveLast ();
						}
					}
				}

				while (pathCost > actions && tmPath.Count > 1) {
					pathCost -= lastTarget.Value.Cost;
					lastTarget = lastTarget.Previous;
					tmPath.RemoveLast ();
				}

				if (tmPath.Count > 1) {
	
					pathCost = 0;
					targetNode = tmPath.First.Next;
					isMoving = true;
					// MapManager.TileOccupationHandle();
					creatureOccupyEvent.Invoke (xpos, ypos, false);
				} else {
					expendAction (1);
				}
			} else {
				expendAction (1);
			}
		}
//		} else {
//			expendAction (1);
//
//		}
	}

	void RandomMotion(){
		x = xpos;
		y = ypos;

		//Randomly pick a direction
		while (!isNeighbor (x, y)) {
			x = xpos;
			y = ypos;
			switch (Random.Range (1, 8)) {
			case(1):
				x += 1;
				break;
			case(2):
				x -= 1;
				break;
			case(3):
				y -= 1;
				break;
			case(4):
				y += 1;
				break;
			case(5):
				x -= 1;
				y -= 1;
				break;
			case(6):
				x += 1;
				y -= 1;
				break;
			case(7):
				x -= 1;
				y += 1;
				break;
			case(8):
				x += 1;
				y += 1;
				break;
			}	
		}
	}

//	bool isInRange(Player player){
//
//	}
	#endregion

	#region utils methods
	protected override void GetHurt(int attackRoll, int damageRoll){
		if (isClicked) {
			if (attackRoll >= armorClass) {
				health -= damageRoll;
				if (attackRoll == 100) {
					attackFeedbackEvent.Invoke ("Crit!", transform);
				} else {
					attackFeedbackEvent.Invoke ("Hit", transform);
				}
				if (health <= 0) {
					gameObject.GetComponent<CircleDraw> ().DestroyLine ();
					creatureOccupyEvent.Invoke (xpos, ypos, false);
					GameObject deadEnemy = (GameObject)GameObject.Instantiate (Resources.Load ("prefabs/Units/enemyDead"));
					deadEnemy.transform.position = MapCoords.hexCoords (xpos, ypos, GameConstants.creatureZ - 1); //, Quaternion.identity);
					unitManager.RemoveCreature (this);
					Destroy (gameObject);
					return;
				}
				gameObject.GetComponent<EnemyHealthBar> ().SetValue (-damageRoll);
			} else {
				attackFeedbackEvent.Invoke ("Miss", transform);
			}
			isClicked = false;
		}
	}

	bool CheckMoveValidity(){
		if (x < 0) x = 0;
		if (x > GameConstants.CurrentMapSizeX -1) x = GameConstants.CurrentMapSizeX-1;
		if (y < 0) y = 0;
		if (y > GameConstants.CurrentMapSizeY-1) x = GameConstants.CurrentMapSizeY-1;
		if (x != xpos || y != ypos) {
			return true;
		}
		return false;
	}
				
	protected override void expendAction(int cost){
		actions -= cost;
		if (actions <= 0) {
			Mat = currentMaterial;
			actions = 0;
			endTurnTimer.Run ();
		} else {
			EnemyAI ();
		}
	}

	void ShowHealthBar(Status dummy){
		gameObject.transform.Find ("HealthBar").gameObject.SetActive (true);
	}

	void HideHealthBar(Status dummy){
		gameObject.transform.Find ("HealthBar").gameObject.SetActive (false);
	}

	public override void MyTurn(){
	}

	protected override void endTurn(){
		base.endTurn ();
	}
	#endregion

	#region EventSystem Methods
	void OnMouseEnter(){

		gameObject.GetComponent<CircleDraw> ().Draw (Color.red, GameConstants.EnemyAggroRange);
		if (currentMaterial == passiveMaterial) {
			Mat = passiveHoverMaterial;
		} else if (currentMaterial == aggroMaterial) {
			Mat = aggroHoverMaterial;
		}
	}

	void OnMouseExit(){
		isClicked = false;
		gameObject.GetComponent<CircleDraw> ().DestroyLine ();
		Mat = currentMaterial;
	}

	void OnMouseUp(){
		isClicked = true;
		// Player.Attack()
		enemyClick.Invoke (xpos, ypos);
	}
		
	public void AddEnemyAIEventListener(UnityAction<int,int> listener){
		// MapManager.FindPathTo
		enemyMoveEvent.AddListener (listener);
	}

	public void AddAttackListener(UnityAction<int,int> listener){
		hurtPlayer.AddListener (listener);
	}

	public void AddEnemyClickListener(UnityAction<int,int> listener){
		enemyClick.AddListener (listener);
	}

	public void AddGetHurtEventListener(UnityAction<int> listener){
		getHurt.AddListener (listener);
	}
	#endregion
}
