﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WaitMusic : MonoBehaviour {
	public static float	MUSIC_FADE_OUT_SPEED	= 1.5f;
	public static float	MUSIC_FADE_IN_SPEED		= 1.75f;
	
	private static SCR_WaitMusic 	instance 	= null;
	private static AudioSource 		source		= null;
	private static float	 		targetVol	= 1;
	private static float	 		volume		= 1;
	public  static bool		 		ready		= false;
	
	private void Awake() {
		if (instance == null) {
			instance = this;
			source 	= GetComponent<AudioSource>();
			DontDestroyOnLoad(gameObject);
			
			if (SCR_Audio.LOAD_FROM_FILE == true) {
				instance.StartCoroutine(LoadFromFile());
			}
			else {
				ready = true;
			}
		}
		else {
			Destroy(gameObject);
		}
	}
		
	private static IEnumerator LoadFromFile() {
		WWW www = new WWW("file:///" + Application.dataPath + "/../Sound/Music/MainMenu.wav");
		yield return www;
		source.clip = www.GetAudioClip(false, false);
		ready = true;
	}
	
	public static void Play () {
		if (SCR_Profile.soundOn == 1) {
			source.Play();
		}
	}
	public static void Stop () {
		source.Stop();
	}
	
	public static void FadeIn () {
		targetVol = 1;
	}
	public static void FadeOut () {
		targetVol = 0;
	}
	
	private void Update() {
		float dt = Time.deltaTime;
		if (volume < targetVol) {
			volume += MUSIC_FADE_IN_SPEED * dt;
			if (volume > targetVol) volume = targetVol;
		}
		else if (volume > targetVol) {
			volume -= MUSIC_FADE_OUT_SPEED * dt;
			if (volume < targetVol) volume = targetVol;
		}
		source.volume = volume;
	}
}
