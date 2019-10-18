using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackFeedbackHandle : FeedbackHandle {

	protected override void Awake(){
		feedback = gameObject.transform.Find ("AttackText").gameObject;
		base.Awake ();
	}
}
