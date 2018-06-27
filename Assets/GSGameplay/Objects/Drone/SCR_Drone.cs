using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Drone : SCR_FlyingObject {
	// ==================================================
	// Const
	public const float 	DRONE_SCALE 	= 1.4f;
	public const float 	EXPLOSION_SCALE = 1.0f;
	public const float 	BROKEN_SPEED 	= 2000.0f;
	public const float 	BROKEN_TIME 	= 1.0f;
	// Prefab
	public GameObject 	PFB_Fragment 	= null;
	public GameObject	PFB_Smoke		= null;
	public GameObject	PFB_Particle	= null;
	// Stuff
	public Sprite[] 	sprFragment 	= null;
	private GameObject 	smokeParticle 	= null;
	private GameObject 	breakParticle 	= null;
	private GameObject 	crashParticle 	= null;
	
	private float 		brokenCountdown	= 0;
	private float		angle			= 0;
	// ==================================================
	
	private void Start () {
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * DRONE_SCALE, SCR_Gameplay.SCREEN_SCALE * DRONE_SCALE, 1);
		
		smokeParticle = Instantiate (PFB_Smoke);
		smokeParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * DRONE_SCALE, SCR_Gameplay.SCREEN_SCALE * DRONE_SCALE, SCR_Gameplay.SCREEN_SCALE * DRONE_SCALE);
		smokeParticle.SetActive (false);
		
		breakParticle = Instantiate (PFB_Particle);
		breakParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * EXPLOSION_SCALE, SCR_Gameplay.SCREEN_SCALE * EXPLOSION_SCALE, SCR_Gameplay.SCREEN_SCALE * EXPLOSION_SCALE);
		foreach(Transform child in breakParticle.transform) {
			child.gameObject.SetActive (false);
		}
		
		crashParticle = Instantiate (PFB_Particle);
		crashParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * EXPLOSION_SCALE, SCR_Gameplay.SCREEN_SCALE * EXPLOSION_SCALE, SCR_Gameplay.SCREEN_SCALE * EXPLOSION_SCALE);
		foreach(Transform child in crashParticle.transform) {
			child.gameObject.SetActive (false);
		}
		
		SCR_Audio.PlayDroneLoopSound(GetComponent<AudioSource>());
	}
	
	public override void Break () {
		base.Break();
		
		broken = true;
		angle = Random.Range (30, 60);
		if (Random.Range(0, 10) > 5) angle = -angle;
		speedX = SCR_Helper.Sin (angle) * BROKEN_SPEED;
		speedY = SCR_Helper.Cos (angle) * BROKEN_SPEED;
		
		smokeParticle.SetActive (true);
		transform.localEulerAngles 	= new Vector3 (0, 0, angle);
		
		breakParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, breakParticle.transform.position.z);
		foreach(Transform child in breakParticle.transform) {
			child.gameObject.SetActive (true);
		}
		
		GetComponent<AudioSource>().Stop();
		SCR_Audio.PlayObjectHitSound();
		SCR_Audio.PlayDroneCollisionSound();
	}
	
	private void Crash() {
		for (int i=0; i<sprFragment.Length; i++) {
			GameObject frag = SCR_Pool.GetFreeObject (PFB_Fragment);
			frag.GetComponent<SCR_Fragment>().Spawn (x, y, sprFragment[i], 100, DRONE_SCALE);
		}
		
		gameObject.SetActive (false);
		SCR_Gameplay.instance.flyingObject = null;
		//SCR_Gameplay.instance.FlashWhite();
		//Time.timeScale = 0.1f;
		
		smokeParticle.SetActive (false);
		crashParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, crashParticle.transform.position.z);
		foreach(Transform child in crashParticle.transform) {
			child.gameObject.SetActive (true);
		}
		
		broken = false;
		brokenCountdown = 0;
		angle = 0;
		transform.localEulerAngles 	= new Vector3 (0, 0, angle);
		
		SCR_Gameplay.instance.ShakeCamera (0.3f);
		SCR_Gameplay.instance.FlashWhite();
		
		SCR_Audio.PlayDroneExplosionSound();
	}
	
	public override void AddDeltaCameraToObject (float deltaCamera) {
		y += deltaCamera * 0.5f;
		breakParticle.transform.position = new Vector3 (breakParticle.transform.position.x, breakParticle.transform.position.y + deltaCamera, breakParticle.transform.position.z);
	}
	
	protected override void Update () {
		base.Update();
		
		if (broken) {
			brokenCountdown += Time.deltaTime;
			if (brokenCountdown > BROKEN_TIME) {
				Crash();
			}
			
			smokeParticle.transform.position = transform.position;
		}
		else {
			angle = 0;
			transform.localEulerAngles 	= new Vector3 (0, 0, angle);
		}
	}
}
