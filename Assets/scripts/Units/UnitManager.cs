using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
	#region fields
	// EVENTS
	CreatureEvent changeTurnEvent = new CreatureEvent();
	Event enemyAIEvent = new Event();
	IntIntBoolEvent creatureOccupyEvent = new IntIntBoolEvent ();
//	ChangeTurnEvent changeTurnEvent = new ChangeTurnEvent();
//	EnemyAIEvent enemyAIEvent = new EnemyAIEvent();
//	CreatureOccupyEvent creatureOccupyEvent = new CreatureOccupyEvent();

	// Turn Base support
	LinkedList<Creature> creatures;
	LinkedListNode<Creature> turn;
	Player player;

	// Spawn Units support
	int x;
	int y;
	#endregion

	#region Properties

	public LinkedListNode<Creature> Turn{
		get { return turn; }
		set { turn = value; }
	}
	#endregion

	#region Methods

	void Awake(){
		EventManager.SetUnitManager (this);
		EventManager.AddStartingUnitsListener (AssignStartingPositions);
		EventManager.AddEndTurnListener (EndTurnEvent);
		//EventManager.AddEnemyAIEventInvoker (this);

		//enemyMoveEvent = new EnemyMoveEvent ();
		creatures = new LinkedList<Creature> ();

	}

	public void AssignStartingPositions(Vector2 playerStart, List<Vector2> enemiesStart){
		x = (int)playerStart [0];
		y = (int)playerStart [1];
		List<GameObject> enemyList = new List<GameObject> ();
		GameObject plyr = (GameObject)Instantiate (Resources.Load ("prefabs/Units/player"), MapCoords.CreaturesCoords(x, y, GameConstants.creatureZ), Quaternion.identity);
		plyr.GetComponent<Player> ().XPOS = x;
		plyr.GetComponent<Player> ().YPOS = y;
		creatureOccupyEvent.Invoke(x,y,true);
		foreach (Vector2 enemyStart in enemiesStart) {
			x = (int)enemyStart [0];
			y = (int)enemyStart [1];
			GameObject enemy = null;
			int whichEnemy = Random.Range (0, 3);
			switch(whichEnemy){
			case(0):
				enemy = (GameObject)Instantiate (Resources.Load ("prefabs/Enemies/enemyOne"));
				break;
			case(1):
				enemy = (GameObject)Instantiate (Resources.Load ("prefabs/Enemies/enemyTwo"));
				break;
			case(2):
				enemy = (GameObject)Instantiate (Resources.Load ("prefabs/Enemies/enemyThree"));
				break;
			default:
				enemy = (GameObject)Instantiate (Resources.Load ("prefabs/Enemies/enemyOne"));
				break;
			}
			foreach (GameObject otherEnemy in enemyList){
				Physics2D.IgnoreCollision (enemy.GetComponent<CircleCollider2D> (), otherEnemy.GetComponent<CircleCollider2D> ());
			}
			enemyList.Add (enemy);
			enemy.transform.position = MapCoords.CreaturesCoords(x, y, GameConstants.creatureZ);
			enemy.GetComponent<Enemy> ().XPOS = x;
			enemy.GetComponent<Enemy> ().YPOS = y;

			// MapManager.TileOccupationHandle()
			creatureOccupyEvent.Invoke (x, y, true);
		}
		PopulateCreatureList ();
	}
		
	void PopulateCreatureList(){
		List<GameObject> tileList = new List<GameObject> (GameObject.FindGameObjectsWithTag ("tile"));
		List<GameObject> creatureList = new List<GameObject>(GameObject.FindGameObjectsWithTag ("creature"));
		foreach (GameObject creatureGameObject in creatureList) {
			foreach (GameObject tile in tileList) {
				Physics2D.IgnoreCollision (creatureGameObject.GetComponent<CircleCollider2D> (), tile.GetComponent<PolygonCollider2D> ());
			}
			Creature creature = creatureGameObject.GetComponent<Enemy> ();
			creature.UntMngr = this;
			creatures.AddFirst (creature);
			creature.Turn = false;
		}
		player = GameObject.FindGameObjectWithTag ("player").GetComponent<Player> ();
		player.UntMngr = this;
		creatures.AddFirst (player);
		turn = creatures.First;
		turn.Value.Turn = true;
		changeTurnEvent.Invoke (turn.Value);
	}

	public void RemoveCreature(Creature creature){
		LinkedListNode<Creature> node = creatures.First;
		while (node != null) {
			if (node.Value == creature) {
				creatures.Remove (node);
				return;
			} else {
				node = node.Next;
			}
		}
	}

	void EndTurnEvent(){
		turn.Value.Turn = false;
		if (turn != creatures.Last) {
			turn = turn.Next;
		} else {
			turn = creatures.First;
		}
		turn.Value.Turn = true;
		changeTurnEvent.Invoke (turn.Value);
		enemyAIEvent.Invoke ();
	}

	public void AddChangeTurnEventListener(UnityAction<Creature> listener){
		changeTurnEvent.AddListener (listener);
	}

	public void AddEnemyAIEventListener(UnityAction listener){
		enemyAIEvent.AddListener (listener);
	}

	public void AddCreatureOccupyEventListener(UnityAction<int,int,bool> listener){
		creatureOccupyEvent.AddListener (listener);
	}
	#endregion
}

