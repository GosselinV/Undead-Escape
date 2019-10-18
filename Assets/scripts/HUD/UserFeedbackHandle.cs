using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserFeedbackHandle : FeedbackHandle {


	protected override void Awake(){
		feedback = gameObject.transform.Find ("UserText").gameObject;
		base.Awake ();
	}

}