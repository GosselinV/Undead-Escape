using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	#region fields
	List<Node> edges;
	int xpos;
	int ypos;
	int cost;
	#endregion

	#region properties
	public List<Node> Edge
	{
		get {return edges;}
	}

	public int XPOS{
		get { return xpos; }
	}

	public int YPOS{
		get { return ypos; }
	}

	public int Cost{
		set { cost = value; }
		get { return cost; }
	}

	#endregion

	#region constructor
	public Node(int i, int j)
	{
		edges = new List<Node>();
		xpos = i;
		ypos = j;
	}
	#endregion

	#region methods
	public float DistanceTo(Node n)
	{
		return Vector2.Distance(new Vector2(xpos, ypos), new Vector2(n.XPOS, n.YPOS));
	}

	public void AddEdgeNode(Node n){
		edges.Add (n);
	}
	#endregion

}
