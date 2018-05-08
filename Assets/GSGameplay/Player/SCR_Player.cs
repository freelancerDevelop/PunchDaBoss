using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
	TALK = 0,
	WALK,
	GRAB,
	CHARGE,
	THROW,
	FLY_UP,
	FLY_DOWN,
	WATCH
}

public class SCR_Player : MonoBehaviour {
	// ==================================================
	// Const
	public const float PLAYER_START_X			= 100;
	public const float PLAYER_START_Y			= 350;
	public const float PLAYER_SCALE				= 0.4f;
	public const float PLAYER_WALK_SPEED		= 400.0f;
	public const float PLAYER_GRAB_RANGE		= 100.0f;
	public const float PLAYER_GRAB_HEIGHT		= 100.0f;
	public const float PLAYER_REVERSE_X			= 140.0f;
	
	public const float PLAYER_CHARGE_TIME		= 0.2f;
	public const float PLAYER_THROW_TIME		= 0.3f;
	public const float PLAYER_START_COOLDOWN	= 1.0f;
	public const float PLAYER_SIZE				= 200;
	public const float PLAYER_UP_FRICTION		= 5000;
	// ==================================================
	// Stuff
	public	GameObject	PFB_Target;
	
	private Animator 	animator	= null;
	private PlayerState state		= PlayerState.TALK;
	private SCR_Boss	bossScript	= null;
	// ==================================================
	// More stuff
	public 	float	x			= 0;
	public 	float	y			= 0;
	public 	int		direction	= -1;
	private	float	chargeCount	= 0;
	private float	flyAngle	= 0;
	public 	float	speedX		= 0;
	public 	float	speedY		= 0;
	public 	float	targetX		= 0;
	public 	float	targetY		= 0;
	public	float	cooldown	= 0;
	
	private	GameObject	target	= null;
	// ==================================================
	
	
	
	
	// ==================================================
	// Private functions
	// ==================================================
	private void Start () {
		animator = GetComponent<Animator>();
		
		x = PLAYER_START_X;
		y = PLAYER_START_Y;
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y - SCR_Gameplay.instance.cameraHeight, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE, 1);
		
		bossScript = SCR_Gameplay.instance.boss.GetComponent<SCR_Boss>();
		target = Instantiate (PFB_Target);
		target.SetActive (false);
		
		chargeCount = 0;
		
		SwitchState (PlayerState.TALK);
	}
	// ==================================================
	private void SwitchState (PlayerState s) {
		state = s;
		animator.SetInteger("AnimationClip", (int)state);
	}
	// ==================================================
	private void Update () {
		float dt = Time.deltaTime;
		
		if (SCR_Gameplay.instance.gameState == GameState.BOSS_FALLING) {
			dt = 0;
		}
		
		if (state == PlayerState.WALK || state == PlayerState.GRAB) {
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
						SwitchState (PlayerState.GRAB);
					}
				}
			}
			else if (state == PlayerState.GRAB) {
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
			bossScript.x = x - direction * PLAYER_GRAB_RANGE;
			bossScript.y = y - PLAYER_GRAB_HEIGHT;
			bossScript.direction = -direction;
			if (chargeCount >= PLAYER_CHARGE_TIME) {
				chargeCount = 0;
				SwitchState (PlayerState.THROW);
				bossScript.Thrown();
			}
		}
		else if (state == PlayerState.THROW) {
			chargeCount += dt;
			if (chargeCount >= PLAYER_THROW_TIME) {
				chargeCount = 0;
				SwitchState (PlayerState.WALK);
			}
		}
		else if (state == PlayerState.FLY_UP || state == PlayerState.FLY_DOWN) {
			if (state == PlayerState.FLY_UP) {
				var distance = SCR_Helper.DistanceBetweenTwoPoint (x, y, bossScript.x, bossScript.y);
				if (distance <= SCR_Profile.GetPunchRange()) {
					Punch (distance);
					SwitchState (PlayerState.FLY_DOWN);
					speedY = 0;
				}
				else if (y > SCR_Gameplay.instance.cameraHeight + SCR_Gameplay.SCREEN_H) {
					speedY = 0;
					SwitchState (PlayerState.FLY_DOWN);
				}
				target.SetActive (true);
				target.GetComponent<SCR_Target>().SetPosition (targetX, targetY - SCR_Profile.GetPunchRange());
			}
			else  if (state == PlayerState.FLY_DOWN) {
				speedY -= SCR_Gameplay.GRAVITY * dt;
				target.SetActive (false);
			}
			
			x += speedX * dt;
			y += speedY * dt;
			
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X)) {
				speedX = -speedX;
				direction = 1;
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - PLAYER_REVERSE_X)) {
				speedX = -speedX;
				direction = -1;
			}
			
			if (state == PlayerState.FLY_DOWN) {
				if (y <= SCR_Gameplay.instance.cameraHeight - PLAYER_SIZE || y <= PLAYER_START_Y) {
					y = PLAYER_START_Y;
					SwitchState (PlayerState.WALK);
				}
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
		
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y - SCR_Gameplay.instance.cameraHeight, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE, 1);
	
		if (cooldown > 0) {
			cooldown -= dt;
			if (cooldown < 0) cooldown = 0;
		}
	}
	// ==================================================
	private void Punch (float distance) {
		float punchAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, bossScript.x, bossScript.y);
		float punchX = SCR_Profile.GetPunchForce() * SCR_Helper.Sin (punchAngle);
		float punchY = SCR_Profile.GetPunchForce() * (1 + SCR_Helper.Cos (punchAngle) * 0.33f);
		
		SCR_Gameplay.instance.punchNumber ++;
		bossScript.Punch (punchX, Mathf.Abs(punchY));
	}
	// ==================================================
	
	
	
	
	// ==================================================
	// Public functions
	// ==================================================
	public void GoGrabTheBoss () {
		if (state == PlayerState.TALK) {
			SwitchState (PlayerState.WALK);
		}
	}
	public bool IsGrabbingTheBoss () {
		return state == PlayerState.GRAB;
	}
	public void ThrowTheBoss () {
		if (state == PlayerState.GRAB) {
			SwitchState (PlayerState.CHARGE);
			cooldown = PLAYER_START_COOLDOWN;
		}
	}
	public void TurnOffCrossHair () {
		target.SetActive (false);
	}
	public void Aim (float px, float py) {
		if (cooldown <= 0) {
			target.SetActive (true);
			target.GetComponent<SCR_Target>().SetPosition (px - SCR_Gameplay.SCREEN_W * 0.5f, py);
			
			float x1, x2, y1, y2;
			x1 = x;
			y1 = y;
			if (py > y1 + SCR_Gameplay.SCREEN_H + PLAYER_SIZE) {
				y1 = py - SCR_Gameplay.SCREEN_H - PLAYER_SIZE;
			}
			
			float aimAngle = SCR_Helper.AngleBetweenTwoPoint (x1, y1, px - SCR_Gameplay.SCREEN_W * 0.5f, py);
			x2 = x1 + SCR_Gameplay.SCREEN_H * SCR_Helper.Sin (aimAngle) * 2;
			y2 = y1 + SCR_Gameplay.SCREEN_H * SCR_Helper.Cos (aimAngle) * 2;
			
			target.GetComponent<SCR_Target>().SetLine (x1, y1, x2, y2);
		}
	}
	public void PerformPunch (float px, float py) {
		if (cooldown <= 0 && target.activeSelf) {
			targetX = px - SCR_Gameplay.SCREEN_W * 0.5f;
			targetY = py;
			
			if (py > y + SCR_Gameplay.SCREEN_H + PLAYER_SIZE) {
				y = py - SCR_Gameplay.SCREEN_H - PLAYER_SIZE;
			}
			
			if (px >= x) 	direction = 1;
			else			direction = -1;
			
			flyAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, targetX, py);
			speedX = SCR_Profile.GetPunchSpeed() * SCR_Helper.Sin (flyAngle);
			speedY = SCR_Profile.GetPunchSpeed() * SCR_Helper.Cos (flyAngle);
			
			target.GetComponent<SCR_Target>().HideLine();
			
			SwitchState (PlayerState.FLY_UP);
			
			cooldown = SCR_Profile.GetPunchCooldown();
		}
	}
	public void AddDeltaCameraToTarget (float amount) {
		if (state == PlayerState.FLY_UP) {
			targetY += amount;
			
			if (y < targetY - SCR_Profile.GetPunchRange()) {
				flyAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, targetX, targetY);
				speedX = SCR_Profile.GetPunchSpeed() * SCR_Helper.Sin (flyAngle);
				speedY = SCR_Profile.GetPunchSpeed() * SCR_Helper.Cos (flyAngle);
			}
		}
	}
	public void ReAdjustY () {
		if (y > PLAYER_SIZE + SCR_Gameplay.SCREEN_H) {
			y = PLAYER_SIZE + SCR_Gameplay.SCREEN_H;
		}
	}
	// ==================================================
}
