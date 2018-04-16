using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState {
	TALK = 0,
	GRAB,
	FLY,
	SLIDE,
	RUN
}

public class SCR_Boss : MonoBehaviour {
	// ==================================================
	// Const
	public const float BOSS_START_X			= -100;
	public const float BOSS_START_Y			= 350;
	public const float BOSS_SCALE			= 0.4f;
	public const float BOSS_REVERSE_X		= 50.0f;
	public const float BOSS_THROWN_SPEED_X	= 1000.0f;
	public const float BOSS_THROWN_SPEED_Y	= 4500.0f;
	public const float BOSS_ROTATE_MIN		= 150.0f;
	public const float BOSS_ROTATE_MAX		= 800.0f;
	public const float BOSS_SLIDE_FRICTION	= 700.0f;
	public const float BOSS_RUN_SPEED		= 600.0f;
	// ==================================================
	// Stuff
	private Animator 	animator	= null;
	private BossState 	state		= BossState.TALK;
	// ==================================================
	// More stuff
	public int		direction	= 1;
	public float	x			= 0;
	public float	y			= 0;
	public float	speedX		= 0;
	public float	speedY		= 0;
	public float	rotation	= 0;
	public float	rotateSpeed	= 0;
	// ==================================================
	
	
	
	
	// ==================================================
	// Private functions
	// ==================================================
	private void Start () {
		animator = GetComponent<Animator>();
		
		x = BOSS_START_X;
		y = BOSS_START_Y;
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y - SCR_Gameplay.instance.cameraHeight, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, 1);
		
		rotation = 0;
		
		SwitchState (BossState.TALK);
	}
	// ==================================================
	private void SwitchState (BossState s) {
		state = s;
		animator.SetInteger("AnimationClip", (int)state);
	}
	// ==================================================
	private void RandomRotate () {
		rotateSpeed = Random.Range (BOSS_ROTATE_MIN, BOSS_ROTATE_MAX);
		if (Random.Range(0, 100) > 50) {
			rotateSpeed = -rotateSpeed;
		}
	}
	
	// ==================================================
	private void Update () {
		float dt = Time.deltaTime;
		if (state == BossState.FLY) {
			speedY -= SCR_Gameplay.GRAVITY * dt;
			
			x += direction * speedX * dt;
			y += speedY * dt;
			
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				direction = 1;
				RandomRotate ();
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				direction = -1;
				RandomRotate ();
			}
			
			if (y < BOSS_START_Y) {
				y = BOSS_START_Y;
				SwitchState (BossState.SLIDE);
			}
			
			rotation += rotateSpeed * dt;
		}
		else if (state == BossState.SLIDE) {
			speedX -= BOSS_SLIDE_FRICTION * dt;
			
			x += direction * speedX * dt;
			
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				direction = 1;
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				direction = -1;
			}
			
			if (speedX <= 0) {
				speedX = 0;
				SwitchState (BossState.RUN);
			}
			
			rotation = 0;
		}
		else if (state == BossState.RUN) {
			x += direction * BOSS_RUN_SPEED * dt;
			
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				
			}
			
			rotation = 0;
		}
		
		transform.position 			= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y - SCR_Gameplay.instance.cameraHeight, transform.position.z);
		transform.localScale 		= new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, 1);
		transform.localEulerAngles 	= new Vector3 (0, 0, rotation);
	}
	// ==================================================
	
	
	
	
	// ==================================================
	// Public functions
	// ==================================================
	public bool IsTalking () {
		return state == BossState.TALK;
	}
	public void Grabbed () {
		if (state == BossState.TALK) {
			SwitchState (BossState.GRAB);
		}
	}
	public void Thrown () {
		if (state == BossState.GRAB) {
			direction = -direction;
			y = BOSS_START_Y;
			speedX = BOSS_THROWN_SPEED_X;
			speedY = BOSS_THROWN_SPEED_Y;
			RandomRotate ();
			SwitchState (BossState.FLY);
		}
	}
	public bool IsFlying () {
		return state == BossState.FLY;
	}
	// ==================================================
}
