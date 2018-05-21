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
	
	
	private int lastScreamSound = -1;
	private int lastTalkSound = -1;
	
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
		if (SCR_Profile.soundOn == 1) {
			if (SCR_PunchMusic.IsAm()) {
				int choose = instance.lastScreamSound;
				while (choose == instance.lastScreamSound) {
					choose = Random.Range(0, instance.screamAm.Length);
				}
				instance.lastScreamSound = choose;
				source.PlayOneShot(instance.screamAm[choose]);
			}
			else {
				int choose = instance.lastScreamSound;
				while (choose == instance.lastScreamSound) {
					choose = Random.Range(0, instance.screamEm.Length);
				}
				instance.lastScreamSound = choose;
				source.PlayOneShot(instance.screamEm[choose]);
			}
		}
	}
	
	public static void PlayTalkSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = instance.lastTalkSound;
			while (choose == instance.lastTalkSound) {
				choose = Random.Range(0, instance.talk.Length);
			}
			instance.lastTalkSound = choose;
			source.PlayOneShot(instance.talk[choose]);
		}
	}
	
	
	public static void PlayFlyUpSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.flyUp);
		}
	}
	
	public static void PlayPunchSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = Random.Range(0, instance.punch.Length);
			source.PlayOneShot(instance.punch[choose]);
		}
	}
	
	public static void PlayFallSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.fall);
		}
	}
	
	public static void PlayClickSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.click);
		}
	}
}
