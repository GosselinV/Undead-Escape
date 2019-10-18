using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : BarParentClass{

	bool isClicked = false;

	protected override void Awake(){
		EventManager.AddHurtEnemyListeners (SetValue);

		value = GameConstants.DefaultEnemyHealth;
		maxValue = value;
		parent = gameObject.transform;
		iconPrefab = "prefabs/HealthBar/HealthEnemyImg";
		missingIconPrefab = "prefabs/HealthBar/MissingHealthEnemyImg";

		icons = new List<GameObject> ();
		missingIcons = new List<GameObject> ();

		float offset = 0;
		for (int i = 0; i < maxValue; i++) {
			icon = (GameObject)GameObject.Instantiate (Resources.Load ("prefabs/HealthBar/HealthEnemyImg"));
			missingIcon = (GameObject)GameObject.Instantiate (Resources.Load ("prefabs/HealthBar/MissingHealthEnemyImg"));
			icon.transform.SetParent(this.gameObject.transform.Find("HealthBar"), false);
			missingIcon.transform.SetParent(this.gameObject.transform.Find("HealthBar"), false);
			RectTransform rt = (RectTransform)icon.transform;

			if (maxValue % 2 != 0) {
				if (i % 2 != 0) {
					offset += rt.rect.width;
				}
			} else {
				if (i % 2 == 0) {
					offset += rt.rect.width / 2f;
				}
			}

			offset = -offset;

			icon.transform.position += new Vector3 (offset, 0, 0);
			missingIcon.transform.position += new Vector3 (offset, 0, 0);

			icons.Add (icon);
		}

		icons.Sort (SortByXpos);
	}
		
	int SortByXpos(GameObject a, GameObject b){
		float apos = a.transform.position.x;
		float bpos = b.transform.position.x;

		if (apos >= bpos) {
			return 1;
		} else {
			return -1;
		}
	}
	void OnMouseExit(){
		isClicked = false;
	}

	void OnMouseUp(){
		isClicked = true;
	}

}
