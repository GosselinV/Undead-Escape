using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
	// EVENTS
//	DictIntLinkedListEvent pathFoundEvent = new DictIntLinkedListEvent();
	LinkedListEvent pathFoundEvent = new LinkedListEvent();
	Vector2ListVector2Event assignStartingUnitsEvent = new Vector2ListVector2Event();
//	PathFoundEvent pathFoundEvent = new PathFoundEvent();
//	AssignStartingUnitsEvent assignStartingUnitsEvent = new AssignStartingUnitsEvent();

	/// The tile types.
	Dictionary<TilesName, TileTypes> tileTypes = new Dictionary<TilesName, TileTypes>();
	TileTypes normal;
	TileTypes impassable;
	TileTypes difficult;
	TileTypes finish;

	// mouse hovering support
	GameObject accessibleHoveredTile;
	GameObject unaccessibleHoveredTile;
	Dictionary<Vector2,LinkedList<Node>> paths; 

	//map init & pathfinding support
	TilesName[,] tileNames;
	Node[,] graph;
	List<GameObject> lineRenderers;
	bool playerTurn;
	int mapSizeX;
	int mapSizeY;

	// turn base support
	Creature turn;

	// Map occupation support
	LinkedListNode<Node> target;
	Tile[,] tiles;

	// GameManager
	UndeadEscape undeadEscape;

	void Awake(){
		undeadEscape = Camera.main.GetComponent<UndeadEscape> ();

		//Load prefabs and populate Tiletypes & UnitTypes
		normal = new TileTypes(TilesName.Normal, 1, 
			(GameObject)Resources.Load("prefabs/Tiles/normal"),
			(Material)Resources.Load("prefabs/Tiles/Materials/normalAccessible", typeof(Material)),
			(Material)Resources.Load("prefabs/Tiles/Materials/normal", typeof(Material)));
		difficult = new TileTypes(TilesName.Difficult, 2, 
			(GameObject)Resources.Load("prefabs/Tiles/difficult"),
			(Material)Resources.Load("prefabs/Tiles/Materials/difficultAccessible", typeof(Material)),
			(Material)Resources.Load("prefabs/Tiles/Materials/difficult", typeof(Material)));
		impassable = new TileTypes(TilesName.Impassable, float.MaxValue, 
			(GameObject)Resources.Load("prefabs/Tiles/impassable"),
			null, null);
		finish = new TileTypes (TilesName.Finish, 1,
			(GameObject)Resources.Load ("prefabs/Tiles/finish"),
			(Material)Resources.Load ("prefabs/Tiles/Materials/finishAccessible", typeof(Material)),
			(Material)Resources.Load ("prefabs/Tiles/Materials/finish", typeof(Material)));
		

		tileTypes [TilesName.Normal] = normal;
		tileTypes [TilesName.Difficult] = difficult;
		tileTypes [TilesName.Impassable] = impassable;
		tileTypes [TilesName.Finish] = finish;

		//Instantiate hoveredTile
		accessibleHoveredTile = (GameObject)Instantiate((GameObject)Resources.Load("prefabs/Tiles/accessibleHovered"), new Vector3(0, 0, 0), Quaternion.identity);
		unaccessibleHoveredTile = (GameObject)Instantiate ((GameObject)Resources.Load ("prefabs/Tiles/unaccessibleHovered"), new Vector3 (0, 0, 0), Quaternion.identity);
		accessibleHoveredTile.SetActive (false);
		unaccessibleHoveredTile.SetActive (false);
		paths = new Dictionary<Vector2, LinkedList<Node>> ();
		lineRenderers = new List<GameObject> ();

		//OnMouseOverTileEvent
		EventManager.AddMouseOverTileListener(HoveredTileEvent);
		EventManager.AddMouseUpTileListener (FindPathTo);
		EventManager.AddEnemyAIEventListener (FindPathTo);
		EventManager.AddOccupyEventListener (TileOccupationHandle);
		EventManager.AddChangeTurnListener (ChangeTurnEvent);
		EventManager.AddMoveTowardsLootCrateEventListener (FindPathTo);

		//Invoker for assigning units and pathfinding
		EventManager.SetMapManager(this);

		//Remaining actions support
		EventManager.AddRemainingActionListener(AccessibleTilesHandle);

	}
		
	void Start()
	{
		//initialize tiles
		MapConfiguration map = new MapConfiguration ("map0.csv");
		mapSizeX = GameConstants.CurrentMapSizeX;
		mapSizeY = GameConstants.CurrentMapSizeY;

		tileNames = map.MapArray;
		tiles = new Tile[mapSizeX, mapSizeY];
		graph = new Graph(mapSizeX,mapSizeY).graph;

		// Instantiate map
		for (int i = 0; i < mapSizeX; i++) {
			for (int j = 0; j < mapSizeY; j++) {
				GameObject tile = (GameObject)Instantiate (tileTypes [tileNames[i,j]].TilePrefab, MapCoords.hexCoords (i, j, 0), Quaternion.identity);
				Tile hex = tile.GetComponent<Tile> ();
				hex.TileType = tileTypes [tileNames [i, j]];
				hex.XPOS = i;
				hex.YPOS = j;
				tiles [i, j] = hex;
				if (tileNames [i, j] == TilesName.Finish) {
					GameConstants.FinishPos = new Vector2 (i, j);
				}
			}
		}
		//assign starting player + enemies starting positions
		assignStartingUnitsEvent.Invoke(map.PlayerStart, map.EnemyStart);
		SpawnLootCrates (map.LootCrates);
	}

	void SpawnLootCrates(List<Vector2> lootCrates){
		foreach (Vector2 lootCrate in lootCrates) {
			GameObject loot = (GameObject)GameObject.Instantiate (Resources.Load ("prefabs/Inventory/LootCrate"), MapCoords.hexCoords((int)lootCrate.x, (int)lootCrate.y,GameConstants.creatureZ), Quaternion.identity);
			LootCrate lc = loot.GetComponent<LootCrate> ();
			lc.XPOS = (int)lootCrate.x;
			lc.YPOS = (int)lootCrate.y;
			loot.transform.Find ("LootCrateUI").gameObject.transform.Find ("Panel").gameObject.SetActive (false);
			TileOccupationHandle (lc.XPOS, lc.YPOS, true);
			undeadEscape.AddLootCrate (loot);
		}
	}

	// Handle for new turn
	void ChangeTurnEvent(Creature turn){
		accessibleHoveredTile.SetActive (false);
		unaccessibleHoveredTile.SetActive (false);
		//Debug.Log (turn.Name.ToString () + "'s turn");
		Debug.Log ("new turn");
		this.turn = turn;
		if (turn.Name == CreatureNames.player) {
			AccessibleTilesHandle (turn.XPOS, turn.YPOS);
		}
	}

	// Handle to set occupied bool on tiles (set cost to maxValue)
	void TileOccupationHandle(int x, int y, bool isOccupied){
		tiles [x, y].Occupied = isOccupied;
	}
		
	// Handle for activating hovered tile
	void HoveredTileEvent (int xpos, int ypos){
		DestroyEdgeRender ();
		if (tiles [xpos, ypos].Accessible) {
			EdgeRender (xpos, ypos);
			accessibleHoveredTile.transform.position = MapCoords.hexCoords (xpos, ypos, -1);
			accessibleHoveredTile.SetActive (true);
			unaccessibleHoveredTile.SetActive (false);
		} else {
			unaccessibleHoveredTile.transform.position = MapCoords.hexCoords (xpos, ypos, -1);
			unaccessibleHoveredTile.SetActive (true);
			accessibleHoveredTile.SetActive (false);
		}
	}

	void AccessibleTilesHandle(int xpos, int ypos){
		ResetTiles ();
		if (xpos < 0 || ypos < 0) {
			DestroyEdgeRender ();
			return;
		} else {
			List<Vector2> tileList = new List<Vector2> ();
			for (int i = 0; i <= turn.Actions; i++) {
				for (int j = 0; j <= turn.Actions; j++) {
					if (i == 0 && j == 0) {
						tileList.Add (new Vector2 (xpos, ypos));
					} else if (i == 0 && j != 0) {
						tileList.Add (new Vector2 (xpos, ypos + j));
						tileList.Add (new Vector2 (xpos, ypos - j));
					} else if (i != 0 && j == 0) {
						tileList.Add (new Vector2 (xpos + i, ypos));
						tileList.Add (new Vector2 (xpos - i, ypos));
					} else {
						tileList.Add (new Vector2 (xpos - i, ypos - j));
						tileList.Add (new Vector2 (xpos - i, ypos + j));
						tileList.Add (new Vector2 (xpos + i, ypos + j));
						tileList.Add (new Vector2 (xpos + i, ypos - j));
					}
				}
			}
			foreach (Vector2 coords in tileList) {
				int x = (int)coords [0];
				int y = (int)coords [1];
				if (x >= 0 && x < mapSizeX && y >= 0 && y < mapSizeY) {

					LinkedList<Node> path = GetPath (x, y);
					//if (pathDict.Count != 0){
					if (path.Count > 0){
						int pathCost = 0;
						target = path.First.Next;
						while (target != null) {
							pathCost += target.Value.Cost;	
							target = target.Next;
						}
						if (pathCost <= turn.Actions) {
							tiles [x, y].Accessible = true;
							paths [new Vector2 (x, y)] = path;
						}
					}
				}
			}
		}
	}

	void ResetTiles(){
		foreach (Vector2 key in paths.Keys) {
			tiles [(int)key [0], (int)key [1]].Accessible = false;
		}
		paths.Clear ();
	}

	// Draw Edges while hovering Tiles
	// 
	void EdgeRender(int xpos, int ypos){

//		Dictionary<int, LinkedList<Node>> pathDict = GetPath (xpos, ypos);
		LinkedList<Node> path = GetPath(xpos, ypos);
//		LinkedListNode<Node> targetNode = pathDict.Values.First ().First;
		LinkedListNode<Node> targetNode = path.First;

		while(targetNode.Next != null) {

			GameObject lineObject = new GameObject ("LineObj");
			LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer> ();
			lineRenderer.useWorldSpace = false;
			lineRenderer.material = new Material (Shader.Find ("Hidden/Internal-Colored"));
			lineRenderers.Add (lineObject);
			lineRenderer.startColor = Color.blue;
			lineRenderer.endColor = Color.blue;
			lineRenderer.startWidth = 0.05f;
			lineRenderer.endWidth = 0.05f;
			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition (0, MapCoords.hexCoords ((int)targetNode.Value.XPOS, (int)targetNode.Value.YPOS, -1));
			lineRenderer.SetPosition (1, MapCoords.hexCoords ((int)targetNode.Next.Value.XPOS, (int)targetNode.Next.Value.YPOS, -1));
			targetNode = targetNode.Next;
		}
	}

	void DestroyEdgeRender(){
		foreach (GameObject line in lineRenderers) {
			Destroy (line);
		}
	}

	// Pathfinder.
	public void FindPathTo(int xpos, int ypos)
	{
//		Dictionary<int,LinkedList<Node>> path = GetPath (xpos, ypos);
		LinkedList<Node> path = GetPath (xpos, ypos);
		//Creature MoveUnit();
		pathFoundEvent.Invoke (path);
	}

//	Dictionary<int, LinkedList<Node>> GetPath(int xpos, int ypos){
	LinkedList<Node> GetPath(int xpos, int ypos){
		// Define target and source nodes. 
		Node target = graph[xpos, ypos];
		Node source = graph[turn.XPOS, turn.YPOS];

		// Ignore if target is impassable or occupied
//		if (tiles[target.XPOS, target.YPOS].TileType.Name == TilesName.Impassable)
		if (tiles[target.XPOS, target.YPOS].Cost > GameConstants.OccupiedTileCost)
		{
			if (turn.Name == CreatureNames.enemy) {
				Debug.Log ("Impassable tile target");
			}
//			return new Dictionary<int,LinkedList<Node>> ();
			return new LinkedList<Node>();
		}

		// work Lists
		LinkedList<Node> tmpPath = new LinkedList<Node>();
		List<Node> queue = new List<Node>();

		// Action cost
		//int pathCost = 0;

		// Dictionnary that list distance to source
		Dictionary<Node, float> dist = new Dictionary<Node, float>();

		//Dictionnary that list previous node in optimal path
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		// initialize source
		dist[source] = 0;
		prev[source] = null;

		//initialize all other nodes
		foreach (Node v in graph)
		{
			if (v != source)
			{
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			queue.Add(v);
		}

		// main loop
		while (queue.Count > 0)
		{
			// u := node with min dist
			Node u = null;
			foreach (Node U in queue)
			{
				// if we still don't have a u, grab whatever. 
				// else grab the node we find that is smaller than the u we're holding; 
				if (u == null || dist[U] < dist[u])
				{
					u = U;
				}
			}
			// break out of main loop if we found the target. 
			if (u == target)
			{
				break;
			}

			// calculate and assign distances for all edges of u
			// u becomes previous node to lowest distance edge. 
			foreach (Node v in u.Edge)
			{
				float alt = dist[u] + tiles[v.XPOS, v.YPOS].Cost + u.DistanceTo(v);
				if (alt < dist[v])
				{
					dist[v] = alt;
					prev[v] = u;
				}
			}
			// we have explored all u's directions. remove from queue. 
			queue.Remove(u);
		}

		if (prev[target] == null)
		{
			// No path have been found between target and source. 
			Debug.Log("No path found");
			return new LinkedList<Node>();
		}

		// Go through prev chain and build the path backwards
		Node tmp = target;
		while (tmp != null)
		{

			//if (tiles [tmp.XPOS, tmp.YPOS].Cost != float.MaxValue && tiles[tmp.XPOS, tmp.YPOS].Cost != GameConstants.OccupiedTileCost) {
			//if (tiles[tmp.XPOS, tmp.YPOS].Cost != GameConstants.OccupiedTileCost) {
				tmp.Cost = (int)tiles [tmp.XPOS, tmp.YPOS].Cost;
//				pathCost += (int)tiles [tmp.XPOS, tmp.YPOS].Cost;
				tmpPath.AddFirst(tmp);
			//}
//			if (tmp.XPOS == turn.XPOS && tmp.YPOS == turn.YPOS) {
//				tmpPath.AddFirst (tmp);
//			}
			tmp = prev[tmp];

		}

		//Dictionary<int, LinkedList<Node>> path = new Dictionary<int, LinkedList<Node>> ();
		LinkedList<Node> path = new LinkedList<Node> ();
//		if (tmpPath.Count == 1) {
//			return path;
//		} else {	
		if (tmpPath.Count > 1) {
			path = tmpPath;
		}
		return path;
	}

//	public void AddPathFoundEventListener(UnityAction<Dictionary<int,LinkedList<Node>>> listener){
	public void AddPathFoundEventListener(UnityAction<LinkedList<Node>> listener){
		pathFoundEvent.AddListener (listener);
	}

	public void AddStartingUnitsEventListener(UnityAction<Vector2, List<Vector2>> listener){
		assignStartingUnitsEvent.AddListener (listener);
	}

}


