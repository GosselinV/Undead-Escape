using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackHandle : MonoBehaviour {
	protected GameObject feedback;
	Animator feedbackAnimator;
	Text feedbackText;
	AnimatorClipInfo[] clipInfo;


	protected virtual void Awake(){
		feedbackAnimator = feedback.GetComponent<Animator> ();
		feedbackText = feedback.GetComponent<Text> ();
		clipInfo = feedbackAnimator.GetCurrentAnimatorClipInfo(0);
	}

	void Start(){
		Destroy (gameObject, clipInfo [0].clip.length);
	}

	public void newFeedback(string message){
		feedbackText.text = message;
	}

}