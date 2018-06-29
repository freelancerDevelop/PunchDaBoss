using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Balloon : SCR_FlyingObject {
	// ==================================================
	// Const
	public const float 	BALLOON_SCALE 	= 1.0f;
	public const float 	EXPLOSION_SCALE = 1.5f;
	// Prefab
	public GameObject 	PFB_Fragment 	= null;
	public GameObject	PFB_Particle	= null;
	// Stuff
	public Sprite[] 	sprFragment 	= null;
	private GameObject 	destroyParticle = null;
	
	private bool 		broken			= false;
	// ==================================================
	
	public override void Start () {
		base.Start();
		
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BALLOON_SCALE, SCR_Gameplay.SCREEN_SCALE * BALLOON_SCALE, 1);
		
		destroyParticle = Instantiate (PFB_Particle);
		destroyParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * EXPLOSION_SCALE, SCR_Gameplay.SCREEN_SCALE * EXPLOSION_SCALE, SCR_Gameplay.SCREEN_SCALE * EXPLOSION_SCALE);
		foreach(Transform child in destroyParticle.transform) {
			child.gameObject.SetActive (false);
		}
	}
	
	public override void Break () {
		base.Break();
		
		for (int i=0; i<sprFragment.Length; i++) {
			GameObject frag = SCR_Pool.GetFreeObject (PFB_Fragment);
			frag.GetComponent<SCR_Fragment>().Spawn (x, y, sprFragment[i], 200, BALLOON_SCALE);
		}
		
		destroyParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, destroyParticle.transform.position.z);
		foreach(Transform child in destroyParticle.transform) {
			child.gameObject.SetActive (true);
		}
		
		gameObject.SetActive (false);
		SCR_Gameplay.instance.flyingObject = null;
		
		SCR_Gameplay.instance.ShakeCamera (0.3f);
		SCR_Gameplay.instance.FlashWhite();
		
		SCR_Audio.PlayBalloonExplosionSound();
	}
	
	public override void AddDeltaCameraToObject (float deltaCamera) {
		y += deltaCamera * 0.5f;
		destroyParticle.transform.position = new Vector3 (destroyParticle.transform.position.x, destroyParticle.transform.position.y + deltaCamera, destroyParticle.transform.position.z);
	}
	
	protected override void Update () {
		base.Update();
	}
}
