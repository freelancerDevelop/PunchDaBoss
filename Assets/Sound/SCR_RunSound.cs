using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_RunSound : MonoBehaviour {
	public static float	RUN_FADE_SPEED		= 0.5f;
	
	private static SCR_RunSound 	instance 	= null;
	private static AudioSource 		source		= null;
	
	private static float	 		volume		= 0;
	private static bool	 			fading		= false;
	
	
	private void Awake() {
		if (instance == null) {
			instance = this;
			source 	= GetComponent<AudioSource>();
			DontDestroyOnLoad(gameObject);
			
			if (SCR_Audio.LOAD_FROM_FILE == true) {
				instance.StartCoroutine(LoadFromFile());
			}
		}
		else {
			Destroy(gameObject);
		}
	}
		
	private static IEnumerator LoadFromFile() {
		WWW www = new WWW("file:///" + Application.dataPath + "/../Sound/Gameplay/pdb_lose_run_fs_loop.wav");
		yield return www;
		source.clip = www.GetAudioClip(false, false);
	}
	
	public static void Play () {
		if (SCR_Profile.soundOn == 1) {
			volume = 1;
			source.volume = 1;
			fading = false;
			source.Play();
		}
	}
	
	public static void Fade () {
		fading = true;
	}
	
	public static void Stop () {
		source.Stop();
	}
	
	private void Update() {
		float dt = Time.deltaTime;
		
		if (fading) {
			volume -= RUN_FADE_SPEED * dt;
			
			if (volume < 0) {
				volume = 0;
				fading = false;
			}
			source.volume = volume;
		}
	}
}
