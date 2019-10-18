using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioSource : MonoBehaviour {


	// Use this for initialization
	void Awake() {
		if (!AudioManager.Initialized) {
			AudioSource audioSource = gameObject.AddComponent<AudioSource> ();
			AudioManager.Initialize (audioSource);
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}
}