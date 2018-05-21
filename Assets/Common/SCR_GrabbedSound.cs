using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GrabbedSound : MonoBehaviour {
	private static SCR_GrabbedSound 	instance 	= null;
	private static AudioSource 			source		= null;
	
	
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
}
