using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GrabbedSound : MonoBehaviour {
	private static SCR_GrabbedSound 	instance 	= null;
	private static AudioSource 			source		= null;
	
	
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
		WWW www = new WWW("file:///" + Application.dataPath + "/../Sound/VO/pdb_vo_grabbed_panic_v1.wav");
		yield return www;
		source.clip = www.GetAudioClip(false, false);
	}
	
	public static void Play () {
		if (SCR_Profile.soundOn == 1) {
			source.Play();
		}
	}
	
	public static void Stop () {
		source.Stop();
	}
}
