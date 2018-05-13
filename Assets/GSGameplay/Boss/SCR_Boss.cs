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
	public const float BOSS_THROWN_SPEED_Y	= 2500.0f;
	public const float BOSS_ROTATE_MIN		= 150.0f;
	public const float BOSS_ROTATE_MAX		= 800.0f;
	public const float BOSS_SLIDE_FRICTION	= 700.0f;
	public const float BOSS_RUN_SPEED		= 600.0f;
	public const float BOSS_MAX_SPEED_X		= 1000.0f;
	public const float BOSS_SIZE			= 200;
	
	public const float BOSS_SHADOW_OFFSET	= -120;
	public const float BOSS_SHADOW_DISTANCE	= 1500;
	// ==================================================
	// Prefab
	public	GameObject	PFB_Shadow;
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
	public bool		getHit		= false;
	
	private	GameObject	shadow	= null;
	// ==================================================
	
	
	
	
	// ==================================================
	// Private functions
	// ==================================================
	private void Start () {
		animator = GetComponent<Animator>();
		
		x = BOSS_START_X;
		y = BOSS_START_Y;
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, 1);
		
		shadow = Instantiate (PFB_Shadow);
		shadow.transform.position 	= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y + BOSS_SHADOW_OFFSET, shadow.transform.position.z);
		shadow.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * (-direction), SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, 1);
		
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
		if (SCR_Gameplay.instance.gameState == GameState.BOSS_FALLING) {
			dt = 0;
		}
		
		if (state == BossState.FLY) {
			float oldSpeedY = speedY;
			speedY -= SCR_Gameplay.GRAVITY * dt;
			
			if (speedY < 0 && oldSpeedY >= 0) {
				if (!getHit) {
					SCR_Gameplay.instance.TriggerTutorial (TutorialStep.AIM);
				}
				else {
					SCR_Gameplay.instance.TriggerTutorial (TutorialStep.FINISH, true);
				}
			}
			
			x += speedX * dt;
			y += speedY * dt;
			
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				speedX = -speedX;
				direction = 1;
				RandomRotate ();
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				speedX = -speedX;
				direction = -1;
				RandomRotate ();
			}
			
			if (y <= SCR_Gameplay.instance.cameraHeight - BOSS_SIZE) {
				SCR_Gameplay.instance.gameState = GameState.BOSS_FALLING;
				SCR_Gameplay.instance.Lose();
				
				if (!getHit) {
					SCR_Gameplay.instance.TriggerTutorial (TutorialStep.MISS, true);
				}
			}
			else if (y <= BOSS_START_Y) {
				y = BOSS_START_Y;
				SwitchState (BossState.SLIDE);
			}
			
			rotation += rotateSpeed * dt;
		}
		else if (state == BossState.SLIDE) {
			if (speedX > 0) {
				speedX -= BOSS_SLIDE_FRICTION * dt;
				if (speedX < 0) {
					speedX = 0;
					SwitchState (BossState.RUN);
				}
			}
			else {
				speedX += BOSS_SLIDE_FRICTION * dt;
				if (speedX > 0) {
					speedX = 0;
					SwitchState (BossState.RUN);
				}
			}
			
			x += speedX * dt;
			
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				direction = 1;
				speedX = -speedX;
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				direction = -1;
				speedX = -speedX;
			}
			
			rotation = 0;
		}
		else if (state == BossState.RUN) {
			x += direction * BOSS_RUN_SPEED * dt;
			rotation = 0;
		}
		
		transform.position 			= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localScale 		= new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, 1);
		transform.localEulerAngles 	= new Vector3 (0, 0, rotation);
		
		float shadowScale = 1 - (y - BOSS_START_Y) / BOSS_SHADOW_DISTANCE;
		if (shadowScale < 0) shadowScale = 0;
		shadow.transform.position 	= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, shadow.transform.position.y, shadow.transform.position.z);
		shadow.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * shadowScale, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * shadowScale, 1);
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
			y = BOSS_START_Y;
			speedX = BOSS_THROWN_SPEED_X * -direction;
			speedY = BOSS_THROWN_SPEED_Y;
			RandomRotate ();
			SwitchState (BossState.FLY);
		}
	}
	public bool IsFlying () {
		return state == BossState.FLY;
	}
	public bool IsRunning () {
		return state == BossState.RUN;
	}
	public void Punch (float px, float py) {
		if (state == BossState.FLY) {
			speedX += px;
			if (speedX > BOSS_MAX_SPEED_X) {
				speedX = BOSS_MAX_SPEED_X;
			}
			else if (speedX < -BOSS_MAX_SPEED_X) {
				speedX = -BOSS_MAX_SPEED_X;
			}
			speedY += py;
			RandomRotate ();
			getHit = true;
			SCR_Gameplay.instance.TriggerTutorial (TutorialStep.HIT);
		}
	}
	public void ReAdjustY () {
		if (y > BOSS_SIZE + SCR_Gameplay.SCREEN_H) {
			y = BOSS_SIZE + SCR_Gameplay.SCREEN_H;
		}
	}
	// ==================================================
}
