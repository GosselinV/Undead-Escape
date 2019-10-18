using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class Tile : MonoBehaviour
{

	#region fields

	// EVENTS
	IntIntEvent onTileMouseOverEvent = new IntIntEvent();
	IntIntEvent onTileMouseUpEvent = new IntIntEvent();

//	OnTileMouseOverEvent onTileMouserOverEvent = new OnTileMouseOverEvent();
//	OnTileMouseUpEvent onTileMouseUpEvent = new OnTileMouseUpEvent();

	// Tile description
	TileTypes tileType;
	[SerializeField]
	int xpos;
	[SerializeField]
	int ypos;
	[SerializeField]
	float cost;

	// Path finding support
	bool occupied = false;
	bool accessible = false;
	Material defaultMaterial;
	Material accessibleMaterial;

	// turn Base support
	Creature turn;
	#endregion

	#region properties
	public int XPOS{
		get { return xpos; }
		set { xpos = value; }
	}

	public int YPOS{
		get { return ypos; }
		set { ypos = value; }
	}

	public bool Occupied {
		set { 
			occupied = value;
			if (value == true) {
				cost = GameConstants.OccupiedTileCost;
			} else {
				cost = tileType.Cost;
			}
		}
		get {return occupied;}
	}

	public bool Accessible{
		get { return accessible; }
		set { 
			accessible = value;
			if (accessible) {
				GetComponent<Renderer> ().material = accessibleMaterial;
			} else {
				GetComponent<Renderer> ().material = defaultMaterial;
			}
		}
	}

	public TileTypes TileType{
		get { return tileType; }
		set {
			tileType = value;
			cost = value.Cost;
			defaultMaterial = value.DefaultMaterial;
			accessibleMaterial = value.AccessibleMaterial;
		}
	}

	public float Cost{
		get { return cost; }
	}

	#endregion

	#region methods
	void Awake(){
		EventManager.AddTilesInvokers (this);
		EventManager.AddChangeTurnListener (ChangeTurnEvent);
	}

	void OnMouseEnter()
	{
		if (!EventSystem.current.IsPointerOverGameObject	()) {
			if (turn.Name == CreatureNames.player && cost != float.MaxValue && !GameConstants.GamePause && turn.State == Status.move) {
				//MapManager.HoveredTileEvent()
				onTileMouseOverEvent.Invoke (xpos, ypos);
			}
		}
	}

	void OnMouseUp()
	{
		if (!EventSystem.current.IsPointerOverGameObject	()) {
			if (turn.Name == CreatureNames.player && cost != float.MaxValue && accessible && !GameConstants.GamePause && turn.State == Status.move) {
				//MapManager.FindPathTo()
				onTileMouseUpEvent.Invoke (xpos, ypos);
			}
		}
	}
		
	//public void SetOccupied(){
	//	occupied = !occupied;
	//}

	public void AddTileMouseOverListener(UnityAction<int, int> listener){
		onTileMouseOverEvent.AddListener (listener);
	}

	public void AddTileMouseUpListener(UnityAction<int, int> listener){
		onTileMouseUpEvent.AddListener (listener);
	}

	void ChangeTurnEvent(Creature turn){
		this.turn = turn;
	}
	#endregion

}
