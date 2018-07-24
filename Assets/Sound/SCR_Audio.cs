using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Audio : MonoBehaviour {
	public static bool 	LOAD_FROM_FILE = false;
	public static float	SCREAM_COOLDOWN = 3.0f;
	
	private static SCR_Audio	 	instance 				= null;
	private static AudioSource 		source					= null;
	
	public AudioSource 				talkSource				= null;

	
	public AudioClip[] scream;
	public AudioClip[] talk;
	
	public AudioClip[] flyUp;
	public AudioClip[] punchNormal;
	public AudioClip[] punchDirect;
	public AudioClip[] punchRicochet;
	public AudioClip[] bounce;
	
	public AudioClip[] securityLaugh;
	public AudioClip[] securityFlyUp;
	
	public AudioClip[] breakSound;
	public AudioClip   transform;
	public AudioClip   firstPunch;
	public AudioClip   fall;
	public AudioClip   click;
	public AudioClip   back;
	public AudioClip   buy;
	
	public AudioClip[] objectHit;
	
	public AudioClip   droneLoop;
	public AudioClip   droneCollision;
	public AudioClip   droneExplosion;
	
	public AudioClip   balloonExplosion;
	
	public AudioClip   ufoLoop;
	public AudioClip   ufoCollision;
	public AudioClip   ufoExplosion;
	
	private float 	screamCooldown = 0;
	private int 	lastScreamSound = -1;
	private int 	lastTalkSound = -1;
	private int 	lastPunchSound = -1;
	
	
	private void Awake() {
		if (instance == null) {
			instance = this;
			source = GetComponent<AudioSource>();
			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(talkSource);
			
			if (LOAD_FROM_FILE == true) {
				scream = new AudioClip[8];
				for (int i=0; i<8; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/VO/pdb_vo_hurt_v1_0" + (i+1).ToString() + ".wav", scream, i));
				}
				
				talk = new AudioClip[6];
				for (int i=0; i<6; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/VO/pdb_vo_talk_v1_0" + (i+1).ToString() + ".wav", talk, i));
				}
				
				punchNormal = new AudioClip[5];
				for (int i=0; i<5; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/Gameplay/pdb_punch_norm_0" + (i+1).ToString() + ".wav", punchNormal, i));
				}
				
				punchDirect = new AudioClip[5];
				for (int i=0; i<5; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/Gameplay/pdb_punch_direct_hit_0" + (i+1).ToString() + ".wav", punchDirect, i));
				}
				
				punchRicochet = new AudioClip[5];
				for (int i=0; i<5; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/Gameplay/pdb_punch_ricochet_0" + (i+1).ToString() + ".wav", punchRicochet, i));
				}
				
				flyUp = new AudioClip[5];
				for (int i=0; i<5; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/Gameplay/pdb_flyup_0" + (i+1).ToString() + ".wav", flyUp, i));
				}
				
				bounce = new AudioClip[5];
				for (int i=0; i<5; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/Gameplay/pdb_wall_bounce_0" + (i+1).ToString() + ".wav", bounce, i));
				}
				
				
				securityLaugh = new AudioClip[19];
				for (int i=0; i<8; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/VO/pdb_security_guy_vo_0" + (i+1).ToString() + ".wav", securityLaugh, i));
				}
				for (int i=9; i<19; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/VO/pdb_security_guy_vo_" + (i+1).ToString() + ".wav", securityLaugh, i));
				}
				
				securityFlyUp = new AudioClip[5];
				for (int i=0; i<5; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/Gameplay/pdb_security_flyup_0" + (i+1).ToString() + ".wav", securityFlyUp, i));
				}
				
				objectHit = new AudioClip[3];
				for (int i=0; i<3; i++) {
					instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/Gameplay/pdb_obj_hit_0" + (i+1).ToString() + ".wav", objectHit, i));
				}
				
				breakSound = new AudioClip[2];
				instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/Gameplay/pdb_start_break.wav", breakSound, 0));
				instance.StartCoroutine(LoadFromFileAndPutToArray("Sound/Gameplay/pdb_start_tree.wav", breakSound, 1));
				
				
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_start_transformation.wav", (x) => transform = x));
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_start_first_punch.wav", (x) => firstPunch = x));
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_lose.wav", (x) => fall = x));
				instance.StartCoroutine(LoadFromFile("Sound/UI/pdb_ui_select.wav", (x) => click = x));
				instance.StartCoroutine(LoadFromFile("Sound/UI/pdb_ui_back.wav", (x) => back = x));
				instance.StartCoroutine(LoadFromFile("Sound/UI/pdb_ui_purchase.wav", (x) => buy = x));
				
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_obj_drone_collision.wav", (x) => droneCollision = x));
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_obj_drone_loop.wav", (x) => droneLoop = x));
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_obj_drone_xplo.wav", (x) => droneExplosion = x));
				
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_obj_balloon_xplo.wav", (x) => balloonExplosion = x));
				
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_obj_ufo_collision.wav", (x) => ufoCollision = x));
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_obj_ufo_loop.wav", (x) => ufoLoop = x));
				instance.StartCoroutine(LoadFromFile("Sound/Gameplay/pdb_obj_ufo_xplo.wav", (x) => ufoExplosion = x));
			}
		}
		else {
			Destroy(gameObject);
		}
	}
	
	private static IEnumerator LoadFromFile(string path, System.Action<AudioClip> Assign) {
		WWW www = new WWW("file:///" + Application.dataPath + "/../" + path);
		yield return www;
		Assign(www.GetAudioClip(false, false));
	}
	
	private static IEnumerator LoadFromFileAndPutToArray(string path, AudioClip[] array, int index) {
		WWW www = new WWW("file:///" + Application.dataPath + "/../" + path);
		yield return www;
		array[index] = www.GetAudioClip(false, false);
	}
	
	
	
	
	private void Update () {
		float dt = Time.deltaTime;
		if (screamCooldown > 0) {
			screamCooldown -= dt;
		}
	}
	
	public static void PlayScreamSound () {
		if (SCR_Profile.soundOn == 1 && instance.screamCooldown <= 0) {
			instance.screamCooldown = SCREAM_COOLDOWN;
			
			int choose = instance.lastScreamSound;
			while (choose == instance.lastScreamSound) {
				choose = Random.Range(0, instance.scream.Length);
			}
			instance.lastScreamSound = choose;
			source.PlayOneShot(instance.scream[choose]);
		}
	}
	
	public static void PlayTalkSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = instance.lastTalkSound;
			while (choose == instance.lastTalkSound) {
				choose = Random.Range(0, instance.talk.Length);
			}
			instance.lastTalkSound = choose;
			instance.talkSource.clip = instance.talk[choose];
			instance.talkSource.Play();
		}
	}
	
	public static void StopTalkSound () {
		instance.talkSource.Stop();
	}
	
	
	public static void PlayFlyUpSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = Random.Range(0, instance.flyUp.Length);
			source.PlayOneShot(instance.flyUp[choose]);
		}
	}
	
	public static void PlayPunchNormalSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = instance.lastPunchSound;
			while (choose == instance.lastPunchSound) {
				choose = Random.Range(0, instance.punchNormal.Length);
			}
			instance.lastPunchSound = choose;
			source.PlayOneShot(instance.punchNormal[choose]);
		}
	}
	
	public static void PlayPunchDirectSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = Random.Range(0, instance.punchDirect.Length);
			source.PlayOneShot(instance.punchDirect[choose]);
		}
	}
	
	public static void PlayPunchRicochetSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = instance.lastPunchSound;
			while (choose == instance.lastPunchSound) {
				choose = Random.Range(0, instance.punchRicochet.Length);
			}
			instance.lastPunchSound = choose;
			source.PlayOneShot(instance.punchRicochet[choose]);
		}
	}
	
	public static void PlayBounceSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = Random.Range(0, instance.bounce.Length);
			source.PlayOneShot(instance.bounce[choose]);
		}
	}
	
	
	public static void PlaySecurityFlyUpSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = Random.Range(0, instance.securityFlyUp.Length);
			source.PlayOneShot(instance.securityFlyUp[choose]);
		}
	}
	
	public static void PlaySecurityLaughSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = Random.Range(0, instance.securityLaugh.Length);
			source.PlayOneShot(instance.securityLaugh[choose]);
		}
	}
	
	public static void PlayTransformSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.transform);
		}
	}
	public static void PlayFirstPunchSound() {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.firstPunch);
		}
	}
	public static void PlayBreakSound (int index) {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.breakSound[index]);
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
	public static void PlayBackSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.back);
		}
	}
	public static void PlayBuySound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.buy);
		}
	}
	
	public static void PlayObjectHitSound () {
		if (SCR_Profile.soundOn == 1) {
			int choose = Random.Range(0, instance.objectHit.Length);
			source.PlayOneShot(instance.objectHit[choose]);
		}
	}
	
	public static void PlayDroneCollisionSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.droneCollision);
		}
	}
	public static void PlayDroneExplosionSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.droneExplosion);
		}
	}
	
	public static void PlayBalloonExplosionSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.balloonExplosion);
		}
	}
	
	public static void PlayUFOCollisionSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.ufoCollision);
		}
	}
	public static void PlayUFOExplosionSound () {
		if (SCR_Profile.soundOn == 1) {
			source.PlayOneShot(instance.ufoExplosion);
		}
	}
}
