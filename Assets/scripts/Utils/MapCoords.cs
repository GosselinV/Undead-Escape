using UnityEngine;
using System.Collections;

static public class MapCoords
{

	static public Vector3 hexCoords(int i, int j, int z)
	{

		float xpos = (float)i;
		if (j % 2 == 0)
		{
			xpos += 0.5f;
		}

		float ypos = (float)j * 0.87f;

		return new Vector3(xpos - (float)GameConstants.CurrentMapSizeX / 2f, ypos - (float)GameConstants.CurrentMapSizeY / 2f + 1, (float)z);
	}
		
	static public Vector3 CreaturesCoords(int i, int j, int z)
	{
		Vector3 offset = new Vector3((float)0.45, (float)0.15, 0);
		return hexCoords (i, j, z);//- offset;
	}
}

