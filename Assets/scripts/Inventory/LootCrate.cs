using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootCrate : MonoBehaviour {

	#region fields
	StringEvent userFeedbackEvent = new StringEvent();
	BoolEvent hudEvent = new BoolEvent();
	BoolEvent halfHudEvent = new BoolEvent ();
	IntEvent openingLootCrateEvent = new IntEvent();
	IntIntEvent movingTowardsLootCrateEvent = new IntIntEvent();
	CrateState state;
	Transform panel;
	Player player;
	Material defaultMat;
	Material hoverMat;
	Material openedMat;

	[SerializeField]
	bool isClicked = false;
	int xpos;
	int ypos;
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

	Material Mat{
		set { GetComponent<Renderer> ().material = value; }
		get { return GetComponent<Renderer> ().material; }
	}

	public enum CrateState{
		closed, 
		opened
	}
	#endregion

	#region methods
	void Awake(){
		EventManager.AddLootCrateInvoker (this);
		EventManager.AddLootCrateClickListener (CheckLootCrate);
		InventoryManager.AddTurnOverEventListener (CloseLootCrate);
		ExitLootCrateButton exitButton = gameObject.transform.Find ("LootCrateUI").Find ("Panel").Find ("Exit").GetComponent<ExitLootCrateButton> ();
		exitButton.AddCloseLootCrateEventListener (CloseLootCrate);
		Camera.main.GetComponent<UndeadEscape> ().AddCloseLootCrateEventListener (CloseLootCrate);
	}

	void Start(){
		panel = gameObject.transform.Find ("LootCrateUI").gameObject.transform.Find ("Panel");
		state = CrateState.closed;
		player = GameObject.FindGameObjectWithTag ("player").GetComponent<Player> ();
	}

	void OnMouseUp(){
		isClicked = true;
		if (player.Turn) {
			if (isNeighbor (player.XPOS, player.YPOS) ){
				if (player.Actions > 1) {
					OpenLootCrate ();
				} else {
					userFeedbackEvent.Invoke ("At least 2 actions required");
					isClicked = false;
				}
			} else {
				//MapManager.FindPathTo ();
				movingTowardsLootCrateEvent.Invoke (xpos, ypos);
			}
		}
	}

	void CheckLootCrate(){
		if (player.Actions > 1 && isNeighbor (player.XPOS, player.YPOS)) {
			OpenLootCrate ();
		} else {
			isClicked = false;
		}
	}

	void OpenLootCrate(){
		if (isClicked) {
			if (state == CrateState.closed) {
				//invoke spending action;
				openingLootCrateEvent.Invoke (1);
			} 
			state = CrateState.opened;
			panel.gameObject.SetActive (true);
			//Time.timeScale = 0;
			GameConstants.GamePause = true;
			halfHudEvent.Invoke (true);
			//hudEvent.Invoke(false);
			//Camera.main.GetComponent<UndeadEscape> ().hud.SetActive (false);
		}
		isClicked = false;
	}

	void CloseLootCrate(){
		if (state == CrateState.opened) {
			state = CrateState.closed;
		}
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
	#endregion

	#region EventManager
	public void AddOpeningLootCrateEvent(UnityAction<int> listener){
		openingLootCrateEvent.AddListener (listener);
	}

	public void AddMovingTowardsLootCrateEvent(UnityAction<int,int> listener){
		movingTowardsLootCrateEvent.AddListener (listener);
	}

	public void AddHudEventListener(UnityAction<bool> listener){
		hudEvent.AddListener (listener);
	}

	public void AddHalfHudEventListener(UnityAction<bool> listener){
		halfHudEvent.AddListener (listener);
	}

	public void AddUserFeedbackEventListener(UnityAction<string> listener){
		userFeedbackEvent.AddListener (listener);
	}
	#endregion
}
