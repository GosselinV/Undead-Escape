using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarParentClass : MonoBehaviour {

	protected List<GameObject> icons;
	protected List<GameObject> missingIcons;

	protected GameObject icon;
	protected string iconPrefab;
	protected GameObject missingIcon;
	protected string missingIconPrefab;
	protected Transform parent;
	protected int value;
	protected int maxValue;

	protected virtual void Awake(){
		icons = new List<GameObject> ();
		missingIcons = new List<GameObject> ();
		for (int i = 0; i < maxValue; i++) {
			SpawnNewIcon (i);
		}
	}
		
	void SpawnNewIcon(int index){
		icon = (GameObject)GameObject.Instantiate (Resources.Load (iconPrefab));
		missingIcon = (GameObject)GameObject.Instantiate (Resources.Load (missingIconPrefab));
		icon.transform.SetParent(parent, false);
		missingIcon.transform.SetParent(parent, false);
		RectTransform rt = (RectTransform)icon.transform;
		icon.transform.position += new Vector3 (index*rt.rect.width, 0, 0);
		missingIcon.transform.position += new Vector3 (index*rt.rect.width, 0, 0);
		icons.Add (icon);
		missingIcons.Add (missingIcon);

	}

 	public void SetValue (int valueChange){
		if (value + valueChange >= maxValue) {
			valueChange = maxValue - value;
		} else if (value + valueChange < 0) {
			valueChange = value;
		}
		if (valueChange < 0) {
			for (int i = 0; i < Mathf.Abs(valueChange); i++) {
				icons [value - 1 - i].SetActive (false);
			}
		}
		if (valueChange > 0) {
			for (int i = 0; i < valueChange; i++) {
				icons [value + i].SetActive (true);
			}
		}
		value += valueChange;
	}

	protected void ChangeMaxValue(int maxValueChange){
		if (maxValueChange > 0) {
			for (int i = maxValue; i < maxValue + maxValueChange; i++) {
				SpawnNewIcon (i);
				icon.SetActive (false);
			}
		} else {
			for (int i = maxValue-1; i >= maxValue + maxValueChange; i--) {
				Destroy (icons [i]);
				Destroy (missingIcons [i]);
				icons.RemoveAt (i);
				missingIcons.RemoveAt (i);
			}
		}
		maxValue += maxValueChange;
		if (value > maxValue) {
			value = maxValue;
		}
	}
}
