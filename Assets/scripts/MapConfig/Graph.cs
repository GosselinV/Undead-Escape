using UnityEngine;
using System.Collections;

public class Graph
{
	public Node[,] graph;

	public Graph(int mapSizeX, int mapSizeY){
		graph = new Node[mapSizeX, mapSizeY];
		// Initialize all nodes
		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				graph[i, j] = new Node(i,j);
			}
		}
		// Populate the edges. 
		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				if (i > 0)
				{
					graph[i, j].AddEdgeNode(graph[i - 1, j]);
					if (j % 2 != 0)
					{
						if (j > 0)
						{
							graph[i, j].AddEdgeNode(graph[i - 1, j - 1]);
						}
						if (j < mapSizeY- 1)
						{
							graph[i, j].AddEdgeNode(graph[i - 1, j + 1]);
						}
					}
				}
				if (i < mapSizeX - 1)
				{
					graph[i, j].AddEdgeNode(graph[i + 1, j]);
					if (j % 2 == 0)
					{
						if (j > 0)
						{
							graph[i, j].AddEdgeNode(graph[i + 1, j - 1]);
						}
						if (j < mapSizeY - 1)
						{
							graph[i, j].AddEdgeNode(graph[i + 1, j + 1]);
						}
					}
				}
				if (j > 0)
				{
					graph[i, j].AddEdgeNode(graph[i, j - 1]);
				}
				if (j < mapSizeY - 1)
				{
					graph[i, j].AddEdgeNode(graph[i, j + 1]);
				}
			}
		}
	}
}

