using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FlyingObject : MonoBehaviour {
	// ==================================================
	// Const
	public const float OBJECT_SIZE 				= 300;
	public const float OBJECT_SPEED_X 			= 500;
	public const float OBJECT_SPEED_Y 			= 1000;
	public const int   DESTRUCTION_MONEY 		= 50;
	
	public const float	SOUND_FADE_OUT_SPEED	= 1.5f;
	public const float	SOUND_FADE_IN_SPEED		= 1.75f;
	
	// ==================================================
	[System.NonSerialized] public float	x		= 0;
	[System.NonSerialized] public float	y		= 0;
	[System.NonSerialized] public float	speedX	= 0;
	[System.NonSerialized] public float	speedY	= 0;
	[System.NonSerialized] public bool	broken	= false;
	
	// ==================================================
	public		GameObject	PFB_MoneyBagEffect;
	
	protected	GameObject	moneyBagParticle	= null;
	protected	AudioSource	source				= null;
	protected	float		targetVol			= 0;
	protected	float		volume				= 0;
	
	public virtual void Start() {
		source = GetComponent<AudioSource>();
		if (source != null) {
			source.volume = 0;
		}
	}
	
	public void Spawn (float px, float py) {
		x = px;
		y = py;
		
		speedX = Random.Range (-OBJECT_SPEED_X, OBJECT_SPEED_X);
		speedY = OBJECT_SPEED_Y;
		
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		
		broken = false;
		
		moneyBagParticle = Instantiate(PFB_MoneyBagEffect);
		moneyBagParticle.transform.localScale = new Vector3(SCR_Gameplay.SCREEN_SCALE * SCR_Boss.BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * SCR_Boss.BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * SCR_Boss.BOSS_SCALE);
		moneyBagParticle.SetActive(false);
	}
	
	public virtual void AddDeltaCameraToObject (float deltaCamera) {
		
	}
	
	public virtual void Break() {
		broken = true;
		SCR_Gameplay.instance.ShowDestruction (x + SCR_Gameplay.SCREEN_W * 0.5f, y - SCR_Gameplay.instance.cameraHeight);
		SCR_Gameplay.instance.AddMoneyAtPosition (DESTRUCTION_MONEY, x + SCR_Gameplay.SCREEN_W * 0.5f, y - SCR_Gameplay.instance.cameraHeight);
		
		moneyBagParticle.SetActive(true);
	}
	
	public virtual void PlayLoopSound() {
	}
	
	protected virtual void Update () {
		float dt = Time.deltaTime;
		
		// fade loop sound
		if (source != null) {
			if (y >= SCR_Gameplay.instance.cameraHeight && y <= SCR_Gameplay.instance.cameraHeight + SCR_Gameplay.SCREEN_H) {
				if (!source.isPlaying) {
					PlayLoopSound();
				}
				targetVol = 1;
			}
			else {
				targetVol = 0;
			}
			
			if (volume < targetVol) {
				volume += SOUND_FADE_IN_SPEED * dt;
				if (volume > targetVol) volume = targetVol;
			}
			else if (volume > targetVol) {
				volume -= SOUND_FADE_OUT_SPEED * dt;
				if (volume < targetVol) volume = targetVol;
			}
			source.volume = volume;
		}
		
		// -- //
		x += speedX * dt;
		if (x <= -((SCR_Gameplay.SCREEN_W - OBJECT_SIZE) * 0.5f)) {
			x = -((SCR_Gameplay.SCREEN_W - OBJECT_SIZE) * 0.5f);
			speedX = -speedX;
		}
		else if (x >= (SCR_Gameplay.SCREEN_W - OBJECT_SIZE) * 0.5f) {
			x = (SCR_Gameplay.SCREEN_W - OBJECT_SIZE) * 0.5f;
			speedX = -speedX;
		}
		
		y += speedY * dt;
		
		if (y <= SCR_Gameplay.instance.cameraHeight - OBJECT_SIZE && volume <= 0.01f) {
			gameObject.SetActive (false);
			SCR_Gameplay.instance.flyingObject = null;
			SCR_Gameplay.instance.objectCounter = SCR_Gameplay.OBJECT_SPAWN_TIME * 0.5f;
		}
		else if (y >= SCR_Gameplay.instance.cameraHeight + SCR_Gameplay.SCREEN_H * 2 + OBJECT_SIZE && volume <= 0.01f) {
			gameObject.SetActive (false);
			SCR_Gameplay.instance.flyingObject = null;
			SCR_Gameplay.instance.objectCounter = SCR_Gameplay.OBJECT_SPAWN_TIME * 0.5f;
		}
			
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		
		moneyBagParticle.transform.position = transform.position;
	}
}
