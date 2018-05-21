using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PunchMusic : MonoBehaviour {
	public static float	MUSIC_FADE_OUT_SPEED	= 1.5f;
	public static float	MUSIC_FADE_IN_SPEED		= 1.75f;
	
	private static SCR_PunchMusic 	instance 	= null;
	private static AudioSource 		source		= null;
	private static float	 		targetVol	= 0;
	private static float	 		volume		= 0;
	
	private void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}
		
	private void Start() {
		source 	= GetComponent<AudioSource>();
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
	
	public static bool IsAm() {
		if (source.time > source.clip.length * 0.25f && source.time < source.clip.length * 0.5f) {
			return true;
		}
		else if (source.time > source.clip.length * 0.75f) {
			return true;
		}
		return false;
	}
}
