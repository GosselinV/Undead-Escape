using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AudioManager
{
	static bool initialized = false;
	static AudioSource audioSource;
	static public Dictionary<AudioClipName, AudioClip> audioClips = new Dictionary<AudioClipName, AudioClip>();

	public static bool Initialized
	{
		get { return initialized; }
	}

	public static void Initialize(AudioSource source){
		initialized = true;
		audioSource = source;
		audioClips.Add (AudioClipName.gamePlay, (AudioClip)Resources.Load("audioClips/gamePlay"));
	}

	public static void Play(AudioClipName name){
		audioSource.PlayOneShot (audioClips [name]);
	}

}

