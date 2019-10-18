using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour {

	GameObject toolTip;

	void Awake(){
		toolTip = gameObject.transform.Find ("ToolTip").gameObject;
	}

	public void ShowToolTip(){
		toolTip.SetActive (true);
	}

	public void HidToolTip(){
		toolTip.SetActive (false);
	}
}
