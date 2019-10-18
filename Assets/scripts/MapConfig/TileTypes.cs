using UnityEngine;
using System.Collections;

public class TileTypes
{
	#region fields
	TilesName name;
	float cost;
	GameObject tilePrefab;
	Material accessibleMaterial;
	Material defaultMaterial;
	#endregion

	#region properties
	public TilesName Name
	{
		get { return name; }
	}
		
	public float Cost
	{
		get { return cost; }
	}

	public GameObject TilePrefab
	{
		get { return tilePrefab; }
	}

	public Material AccessibleMaterial{
		get { return accessibleMaterial; }
	}

	public Material DefaultMaterial{
		get { return defaultMaterial; }
	}
	#endregion

	#region methods
	/// <summary>
	/// Initializes a new instance of the <see cref="TilesType"/> class.
	/// </summary>
	public TileTypes(TilesName name, float cost, GameObject tilePrefab, Material accessibleMaterial, Material defaultMaterial)
	{
		this.name = name;
		this.cost = cost;
		this.tilePrefab = tilePrefab;
		this.accessibleMaterial = accessibleMaterial;
		this.defaultMaterial = defaultMaterial;
	}
	#endregion
}

