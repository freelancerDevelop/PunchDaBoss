using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FlyingObject : MonoBehaviour {
	// ==================================================
	// Const
	public const float OBJECT_SIZE 			= 300;
	public const float OBJECT_SPEED_X 		= 500;
	public const float OBJECT_SPEED_Y 		= 1000;
	public const int   DESTRUCTION_MONEY 	= 500;
	
	// ==================================================
	[System.NonSerialized] public float	x			= 0;
	[System.NonSerialized] public float	y			= 0;
	[System.NonSerialized] public float	speedX		= 0;
	[System.NonSerialized] public float	speedY		= 0;
	[System.NonSerialized] public bool	broken		= false;
	
	// ==================================================
	public  GameObject PFB_MoneyBagEffect;
	
	private GameObject moneyBagParticle	= null;
	
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
		
	
	protected virtual void Update () {
		float dt = Time.deltaTime;
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
		
		if (y <= SCR_Gameplay.instance.cameraHeight - OBJECT_SIZE) {
			gameObject.SetActive (false);
			SCR_Gameplay.instance.flyingObject = null;
			SCR_Gameplay.instance.objectCounter = SCR_Gameplay.OBJECT_SPAWN_TIME * 0.5f;
			Debug.Log("Nullified");
		}
		else if (y >= SCR_Gameplay.instance.cameraHeight + SCR_Gameplay.SCREEN_H * 2 + OBJECT_SIZE) {
			gameObject.SetActive (false);
			SCR_Gameplay.instance.flyingObject = null;
			SCR_Gameplay.instance.objectCounter = SCR_Gameplay.OBJECT_SPAWN_TIME * 0.5f;
			Debug.Log("Nullified2");
		}
			
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		
		moneyBagParticle.transform.position = transform.position;
	}
}
