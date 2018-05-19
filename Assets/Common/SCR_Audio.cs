using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Audio : MonoBehaviour {
	private static SCR_Audio	 	instance 	= null;
	private static AudioSource 		source		= null;
	
	public AudioClip[] screamEm;
	public AudioClip[] screamAm;
	public AudioClip[] talk;
	
	public AudioClip   flyUp;
	public AudioClip[] punch;
	
	public AudioClip   fall;
	public AudioClip   click;
	
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
	
	public static void PlayScreamSound () {
		if (SCR_PunchMusic.IsAm()) {
			int choose = Random.Range(0, instance.screamAm.Length);
			source.PlayOneShot(instance.screamAm[choose]);
		}
		else {
			int choose = Random.Range(0, instance.screamEm.Length);
			source.PlayOneShot(instance.screamEm[choose]);
		}
	}
	
	public static void PlayTalkSound () {
		int choose = Random.Range(0, instance.talk.Length);
		source.PlayOneShot(instance.talk[choose]);
	}
	
	
	public static void PlayFlyUpSound () {
		source.PlayOneShot(instance.flyUp);
	}
	
	public static void PlayPunchSound () {
		int choose = Random.Range(0, instance.punch.Length);
		source.PlayOneShot(instance.punch[choose]);
	}
	
	public static void PlayFallSound () {
		source.PlayOneShot(instance.fall);
	}
	
	public static void PlayClickSound () {
		source.PlayOneShot(instance.click);
	}
}
