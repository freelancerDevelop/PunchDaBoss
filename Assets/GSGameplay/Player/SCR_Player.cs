using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum PlayerState {
	TALK = 0,
	TRANSFORM,
	WALK,
	GRAB,
	CHARGE,
	THROW,
	FLY_UP,
	PUNCH,
	FLY_DOWN,
	LAND,
	WATCH
}

public class SCR_Player : MonoBehaviour {
	// ==================================================
	// Const
	public const float PLAYER_START_X			= 250;
	public const float PLAYER_START_Y			= 300;
	public const float PLAYER_SCALE				= 0.84f;
	public const float PLAYER_WALK_SPEED		= 400;
	public const float PLAYER_GRAB_RANGE		= 105;
	public const float PLAYER_GRAB_HEIGHT		= 9;
	public const float PLAYER_REVERSE_X			= 100;
	public const float PLAYER_PUNCH_RANGE		= 200;
	public const float PLAYER_PUNCH_FORCE		= 5000;
	public const float PLAYER_FLY_SPEED			= 4500;
	public const float PLAYER_TUTORIAL_RANGE	= 100;
	
	public const float PLAYER_TEAR_TIME			= 0.1f;
	public const float PLAYER_TRANSFORM_TIME	= 0.5f;
	public const float PLAYER_CHARGE_TIME		= 1.15f;
	public const float PLAYER_PUNCH_TIME		= 0.3f;
	public const float PLAYER_THROW_TIME		= 0.4f;
	public const float PLAYER_LAND_TIME			= 0.5f;
	public const float PLAYER_SIZE				= 200;
	public const float PLAYER_UP_FRICTION		= 5000;
	public const float PLAYER_SLAV_RANDOM		= 200;
	public const float PLAYER_MAX_SPEED_BONUS	= 1.45f;
	
	public const float PLAYER_SHADOW_OFFSET		= -120;
	public const float PLAYER_SHADOW_DISTANCE	= 1500;
	public const float PLAYER_SHADOW_SCALE		= 0.8f;
	public const float PLAYER_SMOKE_SCALE		= 0.72f;
	public const float PLAYER_SMOKE_OFFSET_Y	= -100;
	
	public const float PLAYER_MARKER_SCALE		= 0.5f;
	
	public readonly int[]	PUNCH_MONEY		= new int []{10, 15, 20, 25, 30};
	public const int 		RICOCHET_MONEY	= 100;
	
	// ==================================================
	// Prefab
	public	GameObject	PFB_Target;
	public	GameObject	PFB_Marker;
	public	GameObject	PFB_Shadow;
	public	GameObject	PFB_Trail;
	public	GameObject	PFB_Land;
	public	GameObject	PFB_ClothTear;
	public	GameObject	PFB_PunchParticle;
	// ==================================================
	// Stuff
	private DragonBones.Animation	animation		= null;
	private PlayerState 			state			= PlayerState.TALK;
	private SCR_Boss				bossScript		= null;
	private SCR_Security			securityScript	= null;
	// ==================================================
	// More stuff
	[System.NonSerialized] public float	x				= 0;
	[System.NonSerialized] public float	y				= 0;
	[System.NonSerialized] public int	direction		= -1;
	[System.NonSerialized] public float	trasnformCount	= 0;
	[System.NonSerialized] public float	chargeCount		= 0;
	[System.NonSerialized] public float	punchCount		= 0;
	[System.NonSerialized] public float	landCount		= 0;
	[System.NonSerialized] public float	flyAngle		= 0;
	[System.NonSerialized] public float	speedX			= 0;
	[System.NonSerialized] public float	speedY			= 0;
	
	private	GameObject	target			= null;
	private	GameObject	shadow			= null;
	private	GameObject	marker			= null;
	private	GameObject	trail			= null;
	private	GameObject	punchParticle	= null;
	private	GameObject	landParticle	= null;
	private	GameObject	tearParticle	= null;
	private	bool		ricocheted		= false;
	private	float		currentAimX		= 0;
	private	float		currentAimY		= 0;
	private	float		targetX			= 0;
	private	float		targetY			= 0;
	
	
	// ==================================================
	
	
	
	
	// ==================================================
	// Private functions
	// ==================================================
	private void Start () {
		animation = transform.GetChild(0).gameObject.GetComponent<DragonBones.UnityArmatureComponent>().animation;
		
		x = PLAYER_START_X;
		y = PLAYER_START_Y;
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE * (-direction), SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE, SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE);
		
		bossScript 		= SCR_Gameplay.instance.boss.GetComponent<SCR_Boss>();
		securityScript	= SCR_Gameplay.instance.security.GetComponent<SCR_Security>();
		
		target = Instantiate (PFB_Target);
		target.SetActive (false);
		
		shadow = Instantiate (PFB_Shadow);
		shadow.transform.position 	= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y + PLAYER_SHADOW_OFFSET, shadow.transform.position.z);
		shadow.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SHADOW_SCALE * (-direction), SCR_Gameplay.SCREEN_SCALE * PLAYER_SHADOW_SCALE, SCR_Gameplay.SCREEN_SCALE * PLAYER_SHADOW_SCALE);
		
		marker = Instantiate (PFB_Marker);
		marker.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_MARKER_SCALE, SCR_Gameplay.SCREEN_SCALE * PLAYER_MARKER_SCALE, SCR_Gameplay.SCREEN_SCALE * PLAYER_MARKER_SCALE);
		marker.SetActive (false);
		
		landParticle = Instantiate (PFB_Land);
		landParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SMOKE_SCALE, SCR_Gameplay.SCREEN_SCALE * PLAYER_SMOKE_SCALE, SCR_Gameplay.SCREEN_SCALE * PLAYER_SMOKE_SCALE);
		landParticle.SetActive (false);
		
		chargeCount = 0;
		
		trail = Instantiate (PFB_Trail);
		trail.GetComponent<SCR_Trail>().TurnParticleOff();
		
		punchParticle = Instantiate (PFB_PunchParticle);
		punchParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE);
		foreach(Transform child in punchParticle.transform) {
			child.gameObject.SetActive (false);
		}
		
		tearParticle = Instantiate (PFB_ClothTear);
		tearParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE);
		foreach(Transform child in tearParticle.transform) {
			child.gameObject.SetActive (false);
		}
		
		SwitchState (PlayerState.TALK);
	}
	// ==================================================
	private void SwitchState (PlayerState s) {
		state = s;
		animation.Play(SCR_Sameer.Player[(int)state]);
	}
	// ==================================================
	private void Update () {
		float dt = Time.deltaTime;
		
		if (SCR_Gameplay.instance.gameState == GameState.BOSS_FALLING) {
			dt = 0;
		}
		
		if (state == PlayerState.TRANSFORM) {
			trasnformCount += dt;
			if (trasnformCount >= PLAYER_TRANSFORM_TIME) {
				SwitchState (PlayerState.WALK);
			}
			
			if (trasnformCount >= PLAYER_TEAR_TIME && trasnformCount - dt < PLAYER_TEAR_TIME) {
				tearParticle.transform.position = new Vector3 (transform.position.x, transform.position.y, tearParticle.transform.position.z);
				foreach(Transform child in tearParticle.transform) {
					child.gameObject.SetActive (true);
				}
			}
			
		}
		else if (state == PlayerState.WALK || state == PlayerState.GRAB) {
			x += direction * PLAYER_WALK_SPEED * dt;
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X)) {
				x = -(SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X);
				direction = 1;
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X)) {
				x = (SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X);
				direction = -1;
			}
			
			if (state == PlayerState.WALK) {
				if (bossScript.IsTalking()) {
					if (Mathf.Abs(x - bossScript.x) < PLAYER_GRAB_RANGE) {
						bossScript.Grabbed();
						SCR_WaitMusic.FadeOut();
						SCR_PunchMusic.FadeIn();
						SwitchState (PlayerState.GRAB);
						SCR_Gameplay.instance.TriggerTutorial (TutorialStep.THROW);
					}
				}
			}
			if (state == PlayerState.GRAB) {
				bossScript.x = x + direction * PLAYER_GRAB_RANGE;
				bossScript.y = y + PLAYER_GRAB_HEIGHT;
				bossScript.direction = -direction;
			}
			
			if (SCR_Gameplay.instance.gameState == GameState.BOSS_FALLING || SCR_Gameplay.instance.gameState == GameState.BOSS_RUNNING) {
				SwitchState (PlayerState.WATCH);
			}
		}
		else if (state == PlayerState.CHARGE) {
			chargeCount += dt;
			if (chargeCount >= PLAYER_CHARGE_TIME && chargeCount - dt < PLAYER_CHARGE_TIME) {
				bossScript.Thrown();
				
				float particleX = (x + bossScript.x) * 0.5f;
				float particleY = (y + bossScript.y) * 0.5f;
				punchParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + particleX, particleY, punchParticle.transform.position.z);
				foreach(Transform child in punchParticle.transform) {
					child.gameObject.SetActive (true);
				}
			}
			else if (chargeCount >= PLAYER_CHARGE_TIME + PLAYER_THROW_TIME) {
				SwitchState (PlayerState.WATCH);
				chargeCount = 0;
			}
		}
		else if (state == PlayerState.THROW) {
			chargeCount += dt;
			if (chargeCount >= PLAYER_THROW_TIME) {
				chargeCount = 0;
				SwitchState (PlayerState.WATCH);
			}
		}
		else if (state == PlayerState.FLY_UP || state == PlayerState.PUNCH || state == PlayerState.FLY_DOWN) {
			if (state == PlayerState.FLY_UP) {
				var distance = SCR_Helper.DistanceBetweenTwoPoint (x, y, bossScript.x, bossScript.y);
				if (distance <= PLAYER_PUNCH_RANGE) {
					Punch (distance);
					SwitchState (PlayerState.PUNCH);
					speedY = 0;
					trail.GetComponent<SCR_Trail>().TurnParticleOff();
				}
				else if (y > SCR_Gameplay.instance.cameraHeight + SCR_Gameplay.SCREEN_H) {
					SwitchState (PlayerState.FLY_DOWN);
					speedY = 0;
					trail.GetComponent<SCR_Trail>().TurnParticleOff();
				}
			}
			else if (state == PlayerState.PUNCH) {
				speedY -= SCR_Gameplay.GRAVITY * dt;
				target.SetActive (false);
				punchCount -= dt;
				if (punchCount < 0) {
					punchCount = 0;
					SwitchState (PlayerState.FLY_DOWN);
				}
			}
			else if (state == PlayerState.FLY_DOWN) {
				speedY -= SCR_Gameplay.GRAVITY * dt;
				target.SetActive (false);
			}
			
			x += speedX * dt;
			y += speedY * dt;
			
			if (speedX < 0 && x <= -(SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X)) {
				x = -(SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X);
				speedX = -speedX;
				direction = 1;
				Ricochet();
			}
			else if (speedX > 0 && x >= (SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X)) {
				x = (SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X);
				speedX = -speedX;
				direction = -1;
				Ricochet();
			}
			
			if (state == PlayerState.FLY_UP) {
				trail.GetComponent<SCR_Trail>().MoveTo (x, y);
			}
			else if (state == PlayerState.FLY_DOWN) {
				if (y <= SCR_Gameplay.instance.cameraHeight - PLAYER_SIZE || y <= PLAYER_START_Y) {
					y = PLAYER_START_Y;
					SwitchState (PlayerState.LAND);
					
					landCount = 0;
					landParticle.SetActive (true);
					landParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y + PLAYER_SMOKE_OFFSET_Y, landParticle.transform.position.z);
				}
			}
		}
		else if (state == PlayerState.LAND) {
			landCount += dt;
			if (landCount >= PLAYER_LAND_TIME) {
				SwitchState (PlayerState.WATCH);
			}
		}
		else if (state == PlayerState.WATCH) {
			if (bossScript.x < x) {
				direction = -1;
			}
			else {
				direction = 1;
			}
		}
		
		if (y <= SCR_Gameplay.instance.cameraHeight - PLAYER_SIZE) {
			marker.SetActive (true);
			marker.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, SCR_Gameplay.instance.cameraHeight, landParticle.transform.position.z);
		}
		else {
			marker.SetActive (false);
		}
		
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE * (-direction), SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE, 1);
		
		float shadowScale = 1 - (y - PLAYER_START_Y) / PLAYER_SHADOW_DISTANCE;
		if (shadowScale < 0) shadowScale = 0;
		shadow.transform.position 	= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, shadow.transform.position.y, shadow.transform.position.z);
		shadow.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SHADOW_SCALE * shadowScale, SCR_Gameplay.SCREEN_SCALE * PLAYER_SHADOW_SCALE * shadowScale, 1);
	}
	// ==================================================
	private void Ricochet () {
		if (state == PlayerState.FLY_UP) {
			if (ricocheted == false) {
				ricocheted = true;
			}
			else {
				speedY *= 0.8f;
				speedX *= 0.8f;
			}
		}
		else if (state == PlayerState.FLY_DOWN) {
			speedX *= 0.1f;
		}
		SCR_Audio.PlayBounceSound();
	}
	// ==================================================
	private void Punch (float distance) {
		float punchAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, bossScript.x, bossScript.y);
		float punchX = PLAYER_PUNCH_FORCE * SCR_Helper.Sin (punchAngle);
		float punchY = PLAYER_PUNCH_FORCE * (1 + SCR_Helper.Cos (punchAngle) * 0.33f);
		
		float particleX = (x + bossScript.x) * 0.5f;
		float particleY = (y + bossScript.y) * 0.5f;
		punchParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + particleX, particleY, punchParticle.transform.position.z);
		foreach(Transform child in punchParticle.transform) {
			child.gameObject.SetActive (true);
		}
		
		bossScript.Punch (punchX, Mathf.Abs(punchY), false);
		if (ricocheted == true) {
			bossScript.ShowMoneyBag();
			SCR_Gameplay.instance.ShowRicochet (bossScript.x + SCR_Gameplay.SCREEN_W * 0.5f, bossScript.y - SCR_Gameplay.instance.cameraHeight);
			SCR_Audio.PlayPunchRicochetSound();
			SCR_Gameplay.instance.AddMoneyAtPosition(RICOCHET_MONEY, bossScript.x + SCR_Gameplay.SCREEN_W * 0.5f, bossScript.y - SCR_Gameplay.instance.cameraHeight);
		}
		else {
			SCR_Audio.PlayPunchNormalSound();
			SCR_Gameplay.instance.AddMoneyAtPosition(PUNCH_MONEY[SCR_Gameplay.instance.comboCount], bossScript.x + SCR_Gameplay.SCREEN_W * 0.5f, bossScript.y - SCR_Gameplay.instance.cameraHeight);
		}
		
		SCR_Gameplay.instance.PunchSuccess(bossScript.x + SCR_Gameplay.SCREEN_W * 0.5f, bossScript.y - SCR_Gameplay.instance.cameraHeight);
		
		punchCount = PLAYER_PUNCH_TIME;
	}
	// ==================================================
	
	
	
	
	// ==================================================
	// Public functions
	// ==================================================
	public void GoGrabTheBoss () {
		if (state == PlayerState.TALK) {
			SCR_Audio.PlayTransformSound();
			
			SwitchState (PlayerState.TRANSFORM);
				
			if (SCR_Profile.showTutorial == 1) {
				bossScript.HideTutorial();	
			}
			
			securityScript.StartCheer();
		}
	}
	public bool IsGrabbingTheBoss () {
		return state == PlayerState.GRAB;
	}
	public void ThrowTheBoss () {
		if (state == PlayerState.GRAB) {
			SCR_Audio.PlayFirstPunchSound();
			SwitchState (PlayerState.CHARGE);
		}
	}
	public void TurnOffCrossHair () {
		target.SetActive (false);
	}
	public void Aim (float px, float py) {
		if (state == PlayerState.WATCH || state == PlayerState.LAND) {
			target.SetActive (true);
			target.GetComponent<SCR_Target>().SetPosition (px - SCR_Gameplay.SCREEN_W * 0.5f, py);
			
			float x1, x2, y1, y2;
			x1 = x;
			y1 = SCR_Gameplay.instance.cameraHeight - PLAYER_SIZE;
			if (y1 < PLAYER_START_Y) {
				y1 = PLAYER_START_Y;
			}
			
			float aimAngle = SCR_Helper.AngleBetweenTwoPoint (x1, y1, px - SCR_Gameplay.SCREEN_W * 0.5f, py);
			x2 = x1 + SCR_Gameplay.SCREEN_H * SCR_Helper.Sin (aimAngle) * 2;
			y2 = y1 + SCR_Gameplay.SCREEN_H * SCR_Helper.Cos (aimAngle) * 2;
			
			target.GetComponent<SCR_Target>().SetLine (x1, y1, x2, y2);
			
			currentAimX = px;
			currentAimY = py;
		}
	}
	public void PerformPunch () {
		if (target.activeSelf) {
			targetX = currentAimX - SCR_Gameplay.SCREEN_W * 0.5f;
			targetY = currentAimY;
			
			if (SCR_Profile.showTutorial == 1) {
				if (SCR_Gameplay.instance.tutorialStep == TutorialStep.AIM) {
					target.SetActive (false);
					return;
				}
				if (SCR_Gameplay.instance.tutorialStep == TutorialStep.PUNCH) {
					if (SCR_Helper.DistanceBetweenTwoPoint (targetX, targetY, bossScript.predictX, bossScript.predictY) > PLAYER_TUTORIAL_RANGE) {
						target.SetActive (false);
						//return;
					}
				}
			}
			
			
			y = SCR_Gameplay.instance.cameraHeight - PLAYER_SIZE;
			if (y < PLAYER_START_Y) {
				y = PLAYER_START_Y;
			}
			
			if (targetX >= x) 	direction = 1;
			else				direction = -1;
			
			flyAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, targetX, targetY);
			speedX = PLAYER_FLY_SPEED * SCR_Helper.Sin (flyAngle);
			speedY = PLAYER_FLY_SPEED * SCR_Helper.Cos (flyAngle);
			
			trail.GetComponent<SCR_Trail>().JumpTo (x, y);
			trail.GetComponent<SCR_Trail>().TurnParticleOn();
			
			target.SetActive (false);
			
			SwitchState (PlayerState.FLY_UP);
			SCR_Audio.PlayFlyUpSound();
			
			ricocheted = false;
			
			if (SCR_Profile.showTutorial == 1) {
				SCR_Gameplay.instance.TriggerTutorial (TutorialStep.FLY_UP);
				bossScript.HideTutorial();	
			}
		}
		else {
			target.SetActive (false);
		}
	}
	public void AddDeltaCameraToPlayer (float amount) {
		if (state == PlayerState.FLY_UP) {
			y += amount * 0.5f;
			
			targetY += amount;
			
			if (y < targetY - PLAYER_PUNCH_RANGE && ricocheted == false) {
				flyAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, targetX, targetY);
				speedX = PLAYER_FLY_SPEED * SCR_Helper.Sin (flyAngle);
				speedY = PLAYER_FLY_SPEED * SCR_Helper.Cos (flyAngle);
			}
		}
		
		punchParticle.transform.position = new Vector3 (punchParticle.transform.position.x, punchParticle.transform.position.y + amount, punchParticle.transform.position.z);
	}
	public void ReAdjustY () {
		if (y > PLAYER_SIZE + SCR_Gameplay.SCREEN_H) {
			y = PLAYER_SIZE + SCR_Gameplay.SCREEN_H;
		}
	}
	// ==================================================
}
