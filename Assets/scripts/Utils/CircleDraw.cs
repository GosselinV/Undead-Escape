using UnityEngine;
using System.Collections;

public class CircleDraw : MonoBehaviour
{
	float thetaScale = 0.01f;
	int size;
//	float radius = GameConstants.EnemyAggroRange;
	LineRenderer lineRenderer;
	GameObject lineObject;

	// Use this for initialization
	void Awake ()
	{
		float sizeValue = (2.0f * Mathf.PI) / thetaScale;
		size = (int)sizeValue;
		size++;

	}
	
	// Update is called once per frame
	public void Draw (Color color, float radius)
	{
		lineObject = new GameObject ("CircleObj");
		lineRenderer = lineObject.AddComponent<LineRenderer> ();
		lineRenderer.material = new Material (Shader.Find ("Hidden/Internal-Colored"));
		lineRenderer.startWidth = 0.04f;
		lineRenderer.endWidth = 0.04f;
		lineRenderer.positionCount = size;
		lineRenderer.startColor = color;
		lineRenderer.endColor = color;

		Vector3 pos = new Vector3(0,0,GameConstants.creatureZ);
		float theta = 0f;
		for (int i = 0; i < size; i++) {
			theta += (2.0f * Mathf.PI * thetaScale);
			float x = radius * Mathf.Cos (theta);
			float y = radius * Mathf.Sin (theta);
			x += gameObject.transform.position.x;
			y += gameObject.transform.position.y;
			pos.x = x;
			pos.y = y;
			lineRenderer.SetPosition (i, pos);
		}
	}

	public void DestroyLine(){
		Destroy (lineObject);
	}
}


