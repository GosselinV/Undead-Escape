using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;


/// <summary>
/// A container for the configuration data
/// </summary>
public class MapConfiguration
{
	#region Fields
	StreamReader input;
	string mapConfigurationDataFileName;
	int mapSizeX;
	int mapSizeY;
	Vector2 playerStart;
	List<Vector2> enemyStart;
	List<Vector2> lootCrates;
	TilesName[,] map;
	Dictionary<TilesName, List<Vector2>> mapTemp;

	#endregion

	#region Properties
	public TilesName[,] MapArray{
		get { return map; }
	}

	public int MapSizeX{
		get { return mapSizeX; }
	}

	public int MapSizeY{
		get { return mapSizeY; }
	}

	public Vector2 PlayerStart{
		get { return playerStart; } 
	}

	public List<Vector2> EnemyStart{
		get { return enemyStart; }
	}

	public List<Vector2> LootCrates{
		get {return lootCrates;}
	}

	#endregion

	#region Constructor
	/// <summary>
	/// Constructor
	/// Reads configuration data from a file. If the file
	/// read fails, the object contains default values for
	/// the configuration data
	/// </summary>
	public MapConfiguration(string csvFile)
	{
		//initialize fields
		mapConfigurationDataFileName = csvFile;
		mapTemp = new Dictionary<TilesName, List<Vector2>> ();
		playerStart = new Vector2 ();
		enemyStart = new List<Vector2> ();
		lootCrates = new List<Vector2> ();

		map0.init ();

		mapTemp = map0.dict;
		playerStart = map0.PlayerStart;
		lootCrates = map0.LootCrates;

		//enemyStart = map0.EnemyStart;
		GameConstants.CurrentMapSizeX = (int)map0.MapSize [0];
		GameConstants.CurrentMapSizeY = (int)map0.MapSize [1];

		Camera.main.transform.position = MapCoords.hexCoords ((int)playerStart.x, (int)playerStart.y, (int)Camera.main.transform.position.z);

		// Open and read map data csv
		// assigns mapSizeX, mapSizeY, PlayerStart, EnemyStart, mapTemp
		try{
			input = File.OpenText (Path.Combine (Application.streamingAssetsPath, mapConfigurationDataFileName));
			readInput();
		} catch (Exception e){			
			Debug.Log(e);
		} finally {
			if (input != null) {
				input.Close ();
			}
		}
		mapSizeX = GameConstants.CurrentMapSizeX;
		mapSizeY = GameConstants.CurrentMapSizeY;

		map = new TilesName[mapSizeX,mapSizeY];

		for (int i = 0; i < mapSizeX; i++) {

			for (int j = 0; j < mapSizeY; j++) {
				map [i, j] = TilesName.Normal;				
			}
		}

		foreach (KeyValuePair<TilesName, List<Vector2>> dict in mapTemp) {
			foreach (Vector2 vec in dict.Value) {
				map [(int)vec[0], (int)vec[1]] = dict.Key;
			}
		}

		enemyStart = PopulateEnemies ();
	}

	#endregion

	#region method
	void readInput(){
		mapTemp = new Dictionary<TilesName, List<Vector2>> ();
		playerStart = new Vector2 ();
		enemyStart = new List<Vector2> ();

		string currentLine = input.ReadLine ();

		while (currentLine != null) {
			string[] tokens = currentLine.Split (',');

			//ConfigEnum : enum of all expected keywords.
			ConfigEnum valueName = (ConfigEnum)Enum.Parse (typeof(ConfigEnum), tokens [0]);
			//Get MapSize
			if (valueName == ConfigEnum.MapSize) {
				GameConstants.CurrentMapSizeX = int.Parse (tokens [1]);
				GameConstants.CurrentMapSizeY = int.Parse (tokens [2]);
			//Get player starting location
			} else if (valueName == ConfigEnum.PlayerStart) {
				playerStart = new Vector2 (int.Parse (tokens [1]), int.Parse (tokens [2]));		
			//Get Enemies starting location
			} else if (valueName == ConfigEnum.EnemyStart) {
				enemyStart.Add(new Vector2(int.Parse (tokens [1]), int.Parse (tokens [2])));		
			//Get location of all special tiles
			} else {
				TilesName key = (TilesName)Enum.Parse (typeof(TilesName), tokens [0]);
				if (!mapTemp.ContainsKey (key)) {
					mapTemp [key] = new List<Vector2> ();
				}
				mapTemp [key].Add(new Vector2 (int.Parse (tokens [1]), int.Parse (tokens [2])));
			}
			currentLine = input.ReadLine ();
		}
	}

	List<Vector2> PopulateEnemies(){
		List<Vector2> enemies = new List<Vector2> ();
		List<Vector2> availableTiles = new List<Vector2> ();

		//////////////////////////
		// COMMENT FOR TESTING //

		float enemyDensity = 0.00f;
		switch (GameConstants.Difficulty) {
		case(0):
			enemyDensity = 0.01f;
			break;
		case(1):
			enemyDensity = 0.02f;
			break;
		case(2):
			enemyDensity = 0.03f;
			break;
		case(3):
			enemyDensity = 0.04f;
			break;
		}
			
		int numEnemies = (int)Mathf.Floor(enemyDensity * mapSizeX * mapSizeY);
		for (int i = 0; i < GameConstants.CurrentMapSizeX; i++) {
			for (int j = 0; j < GameConstants.CurrentMapSizeY; j++) {
				Vector2 pos = new Vector2 (i, j);
				if (map [i, j] != TilesName.Impassable && pos != playerStart && !lootCrates.Contains(pos)) {
					availableTiles.Add (new Vector2 (i, j));
				}
			}
		}
	
		while (numEnemies > 0) {
			enemies.Add (availableTiles [UnityEngine.Random.Range (0, availableTiles.Count)]);
			numEnemies--;
		}

		////////////////////////////////////////////////

		//// UNCOMMENT FOR TESTING ////
//		Vector2 testEnemy = new Vector2(4,3);
//		if (!enemies.Contains (testEnemy)) {
//			enemies.Add (testEnemy);
//		}
		/////////////////////////////////

		return enemies;
	}
	#endregion
}
