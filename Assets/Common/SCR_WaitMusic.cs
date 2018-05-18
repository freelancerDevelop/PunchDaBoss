using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WaitMusic : MonoBehaviour {
	public static float	MUSIC_FADE_OUT_SPEED	= 0.5f;
	public static float	MUSIC_FADE_IN_SPEED		= 0.75f;
	
	private static SCR_WaitMusic 	instance 	= null;
	private static AudioSource 		source		= null;
	private static float	 		targetVol	= 1;
	private static float	 		volume		= 1;
	
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
		source = GetComponent<AudioSource>();
	}
	
	public static void Play () {
		source.Play();
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
