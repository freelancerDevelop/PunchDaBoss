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
	FLY_DOWN
}

public class SCR_Player : MonoBehaviour {
	// ==================================================
	// Const
	public const float PLAYER_START_X		= 100;
	public const float PLAYER_START_Y		= 350;
	public const float PLAYER_SCALE			= 0.4f;
	public const float PLAYER_WALK_SPEED	= 400.0f;
	public const float PLAYER_GRAB_RANGE	= 100.0f;
	public const float PLAYER_GRAB_HEIGHT	= 100.0f;
	public const float PLAYER_REVERSE_X		= 140.0f;
	
	public const float PLAYER_CHARGE_TIME	= 0.2f;
	public const float PLAYER_THROW_TIME	= 0.3f;
	public const float PLAYER_UP_SPEED		= 5000.0f;
	public const float PLAYER_UP_OFFSET		= 200;
	public const float PLAYER_UP_FRICTION	= 5000;
	public const float PLAYER_UP_BRAKE		= 40000;
	// ==================================================
	// Stuff
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
	public 	float	targetY		= 0;
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
				if (y > targetY) {
					speedY -= PLAYER_UP_BRAKE * dt;
				}
			}
			else  if (state == PlayerState.FLY_DOWN) {
				speedY -= SCR_Gameplay.GRAVITY * dt;
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
			
			if (state == PlayerState.FLY_UP && speedY < 0) {
				SwitchState (PlayerState.FLY_DOWN);
			}
			else if (state == PlayerState.FLY_DOWN && y <= PLAYER_START_Y) {
				y = PLAYER_START_Y;
				SwitchState (PlayerState.WALK);
			}
		}
		
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y - SCR_Gameplay.instance.cameraHeight, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE, 1);
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
		}
	}
	public void PerformPunch (float px, float py) {
		if (state == PlayerState.WALK) {
			if (py > y + SCR_Gameplay.SCREEN_H + PLAYER_UP_OFFSET) {
				y = py - SCR_Gameplay.SCREEN_H - PLAYER_UP_OFFSET;
			}
			
			targetY = py;
			
			flyAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, px - SCR_Gameplay.SCREEN_W * 0.5f, py);
			speedX = PLAYER_UP_SPEED * SCR_Helper.Sin (flyAngle);
			speedY = PLAYER_UP_SPEED * SCR_Helper.Cos (flyAngle);
			
			if (speedX >= 0) 	direction = 1;
			else				direction = -1;
			
			SwitchState (PlayerState.FLY_UP);
		}
	}
	// ==================================================
}
