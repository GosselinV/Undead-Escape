using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

abstract public class Creature : MonoBehaviour
{

	#region fields

	// EVENTS
	BoolEvent EndTurnButtonActive = new BoolEvent();
	Event endTurnEvent = new Event();
	protected StringTransformEvent attackFeedbackEvent = new StringTransformEvent();
	protected StringEvent userFeedbackEvent = new StringEvent();
	protected IntIntEvent remainingActionEvent = new IntIntEvent();
	protected IntIntBoolEvent creatureOccupyEvent = new IntIntBoolEvent ();

	// Creature Stats
	[SerializeField]
	protected Status state;
	[SerializeField]
	protected int xpos;
	[SerializeField]
	protected int ypos;
	protected int speed;
	[SerializeField]
	protected int health;
	[SerializeField]
	protected int actions;
	protected int maxActions;
	[SerializeField]
	protected int armorClass;
	protected CreatureNames name;

	// PathFinding support
	MapManager mapManager;
	protected LinkedList<Node> path;
	protected LinkedListNode<Node> targetNode;
	[SerializeField]
	protected int pathCost;
	[SerializeField]
	protected bool isMoving;

	// Turn Order support
	protected Timer endTurnTimer;
	GameObject continueButton;
	[SerializeField]
	protected bool myTurn;
	protected UnitManager unitManager;

	// AI support
	Vector3 targetPosition;

	#endregion

	#region properties

	public Status State{
		get { return state; }
	}

	public bool Turn{
		set { 
			myTurn = value;
			if (myTurn) MyTurn ();
			if (name == CreatureNames.player) {
				EndTurnButtonActive.Invoke (true);
			}
		}
		get { return myTurn; }
	}

	public int Actions{
		get { return actions; }
		set { actions = value; }
	}
	public int MaxActions{
		get { return maxActions; }
	}
	public int XPOS{
		get { return xpos;}
		set { xpos = value; }
	}

	public int YPOS{
		get { return ypos; }
		set { ypos = value; }
	}

	public Vector3 Position{
		get { return transform.position; }
	}

	public CreatureNames Name{
		get { return name; }
	}


	public UnitManager UntMngr{
		set { unitManager = value; }
	}

	#endregion

	#region Methods

	protected void Awake(){
		endTurnTimer = gameObject.AddComponent<Timer> ();
	}

	protected void Start(){

		endTurnTimer.Duration = 0.5f;
		mapManager = Camera.main.GetComponent<MapManager> ();
		EventManager.AddPathFoundListener (MoveUnit);
		EventManager.AddCreatureInvoker(this);
		isMoving = false;
		targetPosition = new Vector3 ();
		speed = GameConstants.creatureSpeed;
	}

	protected void Update(){
		if (isMoving && myTurn) {
			if (targetNode != null) {
				targetPosition = MapCoords.hexCoords (targetNode.Value.XPOS, targetNode.Value.YPOS, GameConstants.creatureZ);
				transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);
			} else {
				Debug.Log ("null targetNode");
				expendAction (1);
			}
			if (transform.position == targetPosition) {
				xpos = targetNode.Value.XPOS;
				ypos = targetNode.Value.YPOS;
				pathCost += targetNode.Value.Cost;
				targetNode = targetNode.Next;
				if (targetNode != null) {
					targetPosition = MapCoords.hexCoords (targetNode.Value.XPOS, targetNode.Value.YPOS, GameConstants.creatureZ);
				} else {
					isMoving = false;
					creatureOccupyEvent.Invoke (xpos, ypos, true);
					expendAction (pathCost);
					CheckLootCrate ();
					//MapManager.TileOccupationHandle();
					pathCost = 0;
				}

			}
		}
	}

	protected virtual void CheckLootCrate (){
	}


	// Check whether coord x,y is hex neighbor. 
	protected bool isNeighbor(int x, int y){
		if (x >= 0 && x < GameConstants.CurrentMapSizeX && y >= 0 && y < GameConstants.CurrentMapSizeY) {
			if (ypos == y) {
				if (x == xpos + 1 || x == xpos - 1) {
					return true;
				}
			} else if (y == ypos + 1 || y == ypos - 1) {
				if (ypos % 2 == 0) {
					if (x == xpos || x == xpos + 1) {
						return true;
					}
				} else {
					if (x == xpos || x == xpos - 1) {
						return true;
					}
				}
			}
		}
		return false;
	}

	protected int AttackRoll(int attackModifier){
		return Random.Range (1, 21) + attackModifier;
	}

	protected abstract void MoveUnit (LinkedList<Node> tmPath);
	protected abstract void expendAction(int cost);
	protected abstract void GetHurt (int attackRoll, int damageRoll);
	protected abstract void Attack(int x, int y);
	public abstract void MyTurn ();


	protected virtual void endTurn(){
		// UnitManager.EndTurnEvent()
		endTurnEvent.Invoke ();
	}
		
	public void AddEndTurnBoolListener(UnityAction<bool> listener){
		EndTurnButtonActive.AddListener (listener);
	}

	public void AddEndTurnListener(UnityAction listener){
		endTurnEvent.AddListener (listener);
	}

	public void AddOccupyEventListener(UnityAction<int,int,bool> listener){
		creatureOccupyEvent.AddListener (listener);
	}

	public void AddRemainingActionEventListener(UnityAction<int,int> listener){
		remainingActionEvent.AddListener (listener);
	}

	public void AddAttackFeedbackEventListener(UnityAction<string, Transform> listener){
		attackFeedbackEvent.AddListener (listener);
	}

	public void AddUserFeedbackEventListener(UnityAction<string> listener){
		userFeedbackEvent.AddListener (listener);
	}

	#endregion

}

