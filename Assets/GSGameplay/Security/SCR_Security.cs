using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum SecurityState {
	STAND = 0,
	CHEER,
	FLY_UP,
	FLY_DOWN
}

public class SCR_Security : MonoBehaviour {
	// ==================================================
	// Const
	public const float SECURITY_START_X			= -350;
	public const float SECURITY_START_Y			= 230;
	public const float SECURITY_SCALE			= 0.8f;
	public const float SECURITY_SIZE			= 200;
	public const float SECURITY_RANGE			= 200;
	public const float SECURITY_SPEED			= 4500;
	public const float SECURITY_FORCE			= 10000;
	
	public const float SECURITY_SHADOW_OFFSET	= 0;
	public const float SECURITY_SHADOW_DISTANCE	= 1500;
	public const float SECURITY_SMOKE_OFFSET_Y	= -130;
	
	public const float SECURITY_LAUGH_DELAY		= 0.2f;
	
	public const int   SECURITY_MONEY			= 20;
	
	public const int   SECURITY_PROGRESS_STEPS	= 5;
	// ==================================================
	// Prefab
	public	GameObject	PFB_Shadow;
	public	GameObject	PFB_Trail;
	public	GameObject	PFB_Land;
	public	GameObject	PFB_PunchParticle;
	// ==================================================
	// Stuff
	private DragonBones.Animation	animation	= null;
	private SecurityState 			state		= SecurityState.STAND;
	private SCR_Boss				bossScript	= null;
	// ==================================================
	// More stuff
	[System.NonSerialized] public int	direction	= 1;
	[System.NonSerialized] public float	x			= 0;
	[System.NonSerialized] public float	y			= 0;
	[System.NonSerialized] public float	speedX		= 0;
	[System.NonSerialized] public float	speedY		= 0;
	
	private	GameObject	shadow			= null;
	private	GameObject	trail			= null;
	private	GameObject	punchParticle	= null;
	private	GameObject	landParticle	= null;
	private	float		laughDelay		= 0;
	// ==================================================
	
	
	
	
	// ==================================================
	// Private functions
	// ==================================================
	private void Start () {
		animation = transform.GetChild(0).gameObject.GetComponent<DragonBones.UnityArmatureComponent>().animation;
		
		x = SECURITY_START_X;
		y = SECURITY_START_Y;
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE, SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE);
		
		bossScript = SCR_Gameplay.instance.boss.GetComponent<SCR_Boss>();
		
		shadow = Instantiate (PFB_Shadow);
		shadow.transform.position 	= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y + SECURITY_SHADOW_OFFSET, shadow.transform.position.z);
		shadow.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE * (-direction), SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE, SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE);
		
		trail = Instantiate (PFB_Trail);
		trail.GetComponent<SCR_Trail>().TurnParticleOff();
		
		landParticle = Instantiate (PFB_Land);
		landParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE, SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE, SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE);
		landParticle.SetActive (false);
		
		punchParticle = Instantiate (PFB_PunchParticle);
		punchParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE);
		foreach(Transform child in punchParticle.transform) {
			child.gameObject.SetActive (false);
		}
		
		SwitchState (SecurityState.STAND);
	}
	// ==================================================
	private void SwitchState (SecurityState s) {
		state = s;
		animation.Play(SCR_Sameer.Security[(int)state]);
	}
	// ==================================================
	private void Update () {
		float dt = Time.deltaTime;
		
		if (state == SecurityState.STAND) {
			
		}
		else if (state == SecurityState.CHEER) {
			
		}
		else if (state == SecurityState.FLY_UP) {
			float flyAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, bossScript.x, bossScript.y);
			speedX = SECURITY_SPEED * SCR_Helper.Sin (flyAngle);
			speedY = SECURITY_SPEED * SCR_Helper.Cos (flyAngle);
			
			var distance = SCR_Helper.DistanceBetweenTwoPoint (x, y, bossScript.x, bossScript.y);
			if (distance <= SECURITY_RANGE * bossScript.currentScale.x / bossScript.originalScale.x) {
				speedY = 0;
				trail.GetComponent<SCR_Trail>().TurnParticleOff();
				Punch();
				SwitchState (SecurityState.FLY_DOWN);
			}
			
			if (laughDelay > 0) {
				laughDelay -= dt;
				if (laughDelay <= 0) {
					SCR_Audio.PlaySecurityLaughSound();
				}
			}
		}
		else if (state == SecurityState.FLY_DOWN) {
			SwitchState (SecurityState.FLY_DOWN);
			speedY -= SCR_Gameplay.GRAVITY * dt;
		}
		
		x += speedX * dt;
		y += speedY * dt;
		
		if (state == SecurityState.FLY_UP) {
			trail.GetComponent<SCR_Trail>().MoveTo (x, y);
		}
		else if (state == SecurityState.FLY_DOWN) {
			if (speedX < 0 && x <= -(SCR_Gameplay.SCREEN_W * 0.5f - SECURITY_SIZE * 0.5f)) {
				x = -(SCR_Gameplay.SCREEN_W * 0.5f - SECURITY_SIZE * 0.5f);
				speedX = -speedX;
			}
			else if (speedX > 0 && x >= (SCR_Gameplay.SCREEN_W * 0.5f - SECURITY_SIZE * 0.5f)) {
				x = (SCR_Gameplay.SCREEN_W * 0.5f - SECURITY_SIZE * 0.5f);
				speedX = -speedX;
			}
			if (y <= SCR_Gameplay.instance.cameraHeight - SECURITY_SIZE || y <= SECURITY_START_Y) {
				y = SECURITY_START_Y;
				speedX = 0;
				speedY = 0;
				SwitchState (SecurityState.STAND);
				
				landParticle.SetActive (true);
				landParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y + SECURITY_SMOKE_OFFSET_Y, landParticle.transform.position.z);
			}
		}
		
		transform.position 			= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localScale 		= new Vector3 (SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE, SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE);
		
		float shadowScale = 1 - (y - SECURITY_START_Y) / SECURITY_SHADOW_DISTANCE;
		if (shadowScale < 0) shadowScale = 0;
		shadow.transform.position 	= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, shadow.transform.position.y, shadow.transform.position.z);
		shadow.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE * shadowScale * (-direction), SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE * shadowScale, SCR_Gameplay.SCREEN_SCALE * SECURITY_SCALE);
	}
	
	private void Punch () {
		//SCR_Audio.PlayPunchNormalSound();
		SCR_Audio.PlayPunchDirectSound();
		
		float punchAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, bossScript.x, bossScript.y);
		float punchX = SECURITY_FORCE * SCR_Helper.Sin (punchAngle);
		float punchY = SECURITY_FORCE * (1 + SCR_Helper.Cos (punchAngle) * 0.33f);
		
		float particleX = (x + bossScript.x) * 0.5f;
		float particleY = (y + bossScript.y) * 0.5f;
		punchParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + particleX, particleY, punchParticle.transform.position.z);
		foreach(Transform child in punchParticle.transform) {
			child.gameObject.SetActive (true);
		}
		
		bossScript.Punch (punchX, Mathf.Abs(punchY), true);
		SCR_Gameplay.instance.SecurityPunchSuccess(bossScript.x + SCR_Gameplay.SCREEN_W * 0.5f, bossScript.y - SCR_Gameplay.instance.cameraHeight);
		
		//SCR_Gameplay.instance.AddMoneyAtPosition(SECURITY_MONEY, x + SCR_Gameplay.SCREEN_W * 0.5f, y - SCR_Gameplay.instance.cameraHeight);
		
		SCR_Gameplay.instance.AddMoneyAtPosition(
			SCR_Player.PUNCH_MONEY_START + SCR_Player.PUNCH_MONEY_STEP * (SCR_Gameplay.instance.comboCount - 1),
			x + SCR_Gameplay.SCREEN_W * 0.5f,
			y - SCR_Gameplay.instance.cameraHeight);
	}
	// ==================================================
	
	
	
	
	// ==================================================
	// Public functions
	// ==================================================
	public void StartCheer () {
		SwitchState (SecurityState.CHEER);
	}
	public void PerformPunch () {
		if (state == SecurityState.STAND || state == SecurityState.CHEER) {
			y = SCR_Gameplay.instance.cameraHeight - SECURITY_SIZE;
			if (y < SECURITY_START_Y) {
				y = SECURITY_START_Y;
			}
			
			trail.GetComponent<SCR_Trail>().JumpTo (x, y);
			trail.GetComponent<SCR_Trail>().TurnParticleOn();
			
			SwitchState (SecurityState.FLY_UP);
			
			SCR_Audio.PlaySecurityFlyUpSound();
			laughDelay = SECURITY_LAUGH_DELAY;
		}
	}
	public void AddDeltaCameraToSecurity (float amount) {
		if (state == SecurityState.FLY_UP) {
			y += amount * 0.5f;
		}
		
		punchParticle.transform.position = new Vector3 (punchParticle.transform.position.x, punchParticle.transform.position.y + amount, punchParticle.transform.position.z);
	}
	// ==================================================
}
