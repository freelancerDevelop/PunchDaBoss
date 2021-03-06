﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum BossState {
	TALK = 0,
	GRAB,
	FLY_1,
	FLY_2,
	FLY_3,
	FLY_4,
	FLY_5,
	FLY_6,
	FALL,
	SLIDE,
	RUN
}

public enum BossType {
	THE_BOSS,
	THE_DICTATOR,
	MR_PRESIDENT,
	THE_GENERAL,
	ERIX_LUKE,
	// -- //
	BOSS_COUNT
}

public enum BossSize {
	SMALL,
	BIG,
	ENLARGING,
	SHRINKING
}

public class SCR_Boss : MonoBehaviour {
	// ==================================================
	// Const
	public const float BOSS_START_X				= -50;
	public const float BOSS_START_Y				= 300;
	public const float BOSS_SCALE				= 0.8f;
	public const float BOSS_ENLARGE_MULTIPLIER	= 1.8f;
	public const float BOSS_ENLARGE_DURATION	= 5.0f;
	public const float BOSS_BUBBLE_DURATION		= 5.0f;
	public const float BOSS_MAGNET_FORCE		= 5000.0f;
	public const float BOSS_MAGNET_DURATION		= 5.0f;
	public const float BOSS_FLASH_DURATION		= 2.5f; // must multiply by 0.8 this value for the real value
	public const int   BOSS_FLASH_TIMES 		= 3;
	public const float BOSS_REVERSE_X			= 50;
	public const float BOSS_THROWN_SPEED_X		= 100;
	public const float BOSS_THROWN_SPEED_Y		= 6500;
	public const float BOSS_TUTORIAL_SPEED_X	= 1400;
	public const float BOSS_TUTORIAL_SPEED_Y	= 4500;
	public const float BOSS_TUTORIAL_DELTA		= 0.25f;
	public const float BOSS_ROTATE_MIN			= 50;
	public const float BOSS_ROTATE_MAX			= 300;
	public const float BOSS_SLIDE_FRICTION		= 700;
	public const float BOSS_RUN_SPEED			= 600;
	public const float BOSS_MIN_HANDICAP		= 0.0f;
	public const float BOSS_HANDICAP_HEIGHT		= 70000;
	public const float BOSS_MAX_SPEED_X			= 1500;
	public const float BOSS_MAX_SPEED_Y			= 5500;
	public const float BOSS_MAX_SPEED_Y_BONUS	= 10000;
	public const float BOSS_SIZE				= 200;
	public const float BOSS_CRASH_BONUS			= 6000;
	
	public const float BOSS_SHADOW_OFFSET		= -120;
	public const float BOSS_SHADOW_DISTANCE		= 1500;
	public const float BOSS_SMOKE_RATE			= 0.05f;
	public const float BOSS_SMOKE_OFFSET_X		= 0;
	public const float BOSS_SMOKE_OFFSET_Y		= -130;
	
	public const float BOSS_TALK_SCALE			= 1f;
	public const float BOSS_TALK_OFFSET_X		= -200;
	public const float BOSS_TALK_OFFSET_Y		= 300;
	public const float BOSS_TALK_START			= 1.5f;
	public const float BOSS_TALK_END			= 5.0f;
	
	public const float BOSS_BURN_OFFSET_X		= 0;
	public const float BOSS_BURN_OFFSET_Y		= 150;
	public const float BOSS_BURN_SPEED_MIN		= 5500;
	public const float BOSS_BURN_SPEED_MAX		= 7500;
	public const float BOSS_BURN_ALPHA			= 0.5f;
	
	public const float BOSS_NAME_OFFSET_Y		= -150;
	// ==================================================
	// Boss type
	public	BossType		bossType;
	// ==================================================
	// Prefab
	public	GameObject		PFB_Shadow;
	public	GameObject[]	PFB_MoneyEffect;
	public	GameObject		PFB_MoneyBagEffect;
	public	GameObject		PFB_Smoke;
	public	GameObject		PFB_Land;
	public	GameObject		PFB_Bubble;
	public	GameObject		PFB_Attract;
	public	GameObject		PFB_TalkBubble;
	public	GameObject		PFB_AtmosBurn;
	public	GameObject		PFB_BossName;
	public 	GameObject 		PFB_TutorialRange;
	
	public	GameObject		PFB_TutorialTarget;
	public	GameObject		PFB_TutorialFinger;
	// ==================================================
	// Stuff
	private DragonBones.Animation	animation		= null;
	private BossState 	state						= BossState.TALK;
	private float		enlargeTime					= 0;
	private float		bubbleTime					= 0;
	private float		magnetTime					= 0;
	// ==================================================
	// More stuff
	[System.NonSerialized] public int		direction	= 1;
	[System.NonSerialized] public float		x			= 0;
	[System.NonSerialized] public float		y			= 0;
	[System.NonSerialized] public float		speedX		= 0;
	[System.NonSerialized] public float		speedY		= 0;
	[System.NonSerialized] public float		maxSpeedY	= BOSS_MAX_SPEED_Y;
	[System.NonSerialized] public float		rotation	= 0;
	[System.NonSerialized] public float		rotateSpeed	= 0;
	[System.NonSerialized] public float		predictX	= 0;
	[System.NonSerialized] public float		predictY	= 0;
	[System.NonSerialized] public BossSize	size		= BossSize.SMALL;

	private	GameObject		shadow				= null;
	private	GameObject[]	moneyParticle		= new GameObject[6];
	private GameObject		moneyBagParticle	= null;
	private	GameObject		smokeParticle		= null;
	private	GameObject		landParticle		= null;
	private	GameObject		talkBubble			= null;
	private	GameObject		atmosBurn			= null;
	private	GameObject		bubble				= null;
	private	GameObject		attract				= null;
	private	GameObject		bossName			= null;
	private GameObject 		tutorialRange		= null;
	
	private	GameObject		tutorialTarget		= null;
	private	GameObject		tutorialFinger		= null;
	
	private Material		material			= null;
	
	
	private string[][]	dialogues;
	private string[]	dialogue;
	private int 		dialogueID = 0;
	private int 		particleID = 0;
	private float 		talkCounter = 1.2f;
	private float 		tutorialCounter = 0;
	private bool 		flashing = false;
	private int			flashCount = 0;
	
	public  Vector3		originalScale;
	public  Vector3		currentScale;
	/*
	public  string[]	resultTitle;
	public  int[]		resultTitleFontSize;
	*/
	// ==================================================
	
	
	
	
	// ==================================================
	// Private functions
	// ==================================================
	private void Start () {
		/*
		resultTitle = new string[(int)BossType.BOSS_COUNT];
		resultTitle[(int)BossType.THE_BOSS]		= "Your Boss";
		resultTitle[(int)BossType.THE_DICTATOR]	= "The dictator";
		resultTitle[(int)BossType.MR_PRESIDENT]	= "Mr President";
		resultTitle[(int)BossType.THE_GENERAL]	= "The general";
		resultTitle[(int)BossType.ERIX_LUKE]	= "Erix Luke";
		
		resultTitleFontSize = new int[(int)BossType.BOSS_COUNT];
		resultTitleFontSize[(int)BossType.THE_BOSS]		= 155;
		resultTitleFontSize[(int)BossType.THE_DICTATOR]	= 120;
		resultTitleFontSize[(int)BossType.MR_PRESIDENT]	= 120;
		resultTitleFontSize[(int)BossType.THE_GENERAL]	= 130;
		resultTitleFontSize[(int)BossType.ERIX_LUKE]	= 150;
		*/
		
		dialogues = new string[(int)BossType.BOSS_COUNT][];
		
		dialogues[(int)BossType.THE_BOSS] = new string[] 
			{"It's the 3rd time you're late!"
			,"You're such a slow worker!"
			,"You'll never be promoted!"
			,"One more mistake and you're fired!"
			,"Why are you so lazy?"
			,"You'll OT this weekend, alone!"
			,"Don't even ask for a raise."
			,"You working result is pathetic..."
			,"You should feel ashamed."};
		
		dialogues[(int)BossType.THE_DICTATOR] = new string[] 
			{"It's the 3rd time you're late!"
			,"You're such a slow worker!"
			,"You'll never be promoted!"
			,"One more mistake and you're fired!"
			,"Why are you so lazy?"
			,"You'll OT this weekend, alone!"
			,"Don't even ask for a raise."
			,"You working result is pathetic..."
			,"You should feel ashamed."};
		
		dialogues[(int)BossType.MR_PRESIDENT] = new string[] 
			{"It's the 3rd time you're late!"
			,"You're such a slow worker!"
			,"You'll never be promoted!"
			,"One more mistake and you're fired!"
			,"Why are you so lazy?"
			,"You'll OT this weekend, alone!"
			,"Don't even ask for a raise."
			,"You working result is pathetic..."
			,"You should feel ashamed."};
		
		dialogues[(int)BossType.THE_GENERAL] = new string[] 
			{"It's the 3rd time you're late!"
			,"You're such a slow worker!"
			,"You'll never be promoted!"
			,"One more mistake and you're fired!"
			,"Why are you so lazy?"
			,"You'll OT this weekend, alone!"
			,"Don't even ask for a raise."
			,"You working result is pathetic..."
			,"You should feel ashamed."};
		
		dialogues[(int)BossType.ERIX_LUKE] = new string[] 
			{"It's the 3rd time you're late!"
			,"You're such a slow worker!"
			,"You'll never be promoted!"
			,"One more mistake and you're fired!"
			,"Why are you so lazy?"
			,"You'll OT this weekend, alone!"
			,"Don't even ask for a raise."
			,"You working result is pathetic..."
			,"You should feel ashamed."};
			
		dialogue = dialogues[(int)bossType];
		System.Array.Sort(dialogue, RandomSort);
		
		animation = transform.GetChild(0).gameObject.GetComponent<DragonBones.UnityArmatureComponent>().animation;
		
		x = BOSS_START_X;
		y = BOSS_START_Y;
		
		originalScale = new Vector3 (
			SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE,
			SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE,
			1);
			
		currentScale = originalScale;

		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localScale 	= new Vector3 (currentScale.x * (-direction), currentScale.y, currentScale.z);
		
		shadow = Instantiate (PFB_Shadow);
		shadow.transform.position 	= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y + BOSS_SHADOW_OFFSET, shadow.transform.position.z);
		shadow.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * (-direction), SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE);
		
		for (int i=0; i<6; i++) {
			moneyParticle[i] = Instantiate (PFB_MoneyEffect[i%3]);
			moneyParticle[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE);
			moneyParticle[i].SetActive (false);
		}
		
		moneyBagParticle = Instantiate(PFB_MoneyBagEffect);
		moneyBagParticle.transform.localScale = new Vector3(SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE);
		moneyBagParticle.SetActive(false);
		
		smokeParticle = Instantiate (PFB_Smoke);
		smokeParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE);
		ParticleSystem.EmissionModule emission = smokeParticle.GetComponent<ParticleSystem>().emission;
		emission.rateOverTime = 0;
		
		landParticle = Instantiate (PFB_Land);
		landParticle.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE);
		landParticle.SetActive (false);
		
		talkBubble = Instantiate (PFB_TalkBubble);
		talkBubble.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_TALK_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_TALK_SCALE, 1);
		talkBubble.transform.localPosition = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x + BOSS_TALK_OFFSET_X, y + BOSS_TALK_OFFSET_Y, talkBubble.transform.localPosition.z);
		talkBubble.SetActive (false);
		
		bubble = Instantiate (PFB_Bubble);
		attract = Instantiate (PFB_Attract);
		
		atmosBurn = Instantiate (PFB_AtmosBurn);
		atmosBurn.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		atmosBurn.transform.localPosition = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x + BOSS_BURN_OFFSET_X, y + BOSS_BURN_OFFSET_Y, atmosBurn.transform.localPosition.z);
		atmosBurn.SetActive (false);
		
		bossName = Instantiate (PFB_BossName);
		bossName.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		bossName.transform.localPosition = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y + BOSS_NAME_OFFSET_Y, bossName.transform.localPosition.z);
		bossName.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = SCR_Profile.bosses[SCR_Profile.bossSelecting].name;
		//bossName.SetActive (false);
		
		tutorialTarget = Instantiate (PFB_TutorialTarget);
		tutorialTarget.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, 1);
		tutorialTarget.SetActive (false);
		
		tutorialFinger = Instantiate (PFB_TutorialFinger);
		tutorialFinger.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		tutorialFinger.SetActive (false);
		
		
		if (SCR_Profile.showTutorial == 1) {
			tutorialFinger.transform.localPosition = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, SCR_Gameplay.SCREEN_H * 0.7f, tutorialTarget.transform.localPosition.z);
			tutorialFinger.SetActive (true);
		}
		
		rotation = 0;
		
		material = GetComponentInChildren<MeshRenderer>().sharedMaterial;
		material.SetFloat("_FlashAmount", 0);
		
		SwitchState (BossState.TALK);
	}
	// ==================================================
	private void SwitchState (BossState s) {
		state = s;
		//animator.SetInteger("AnimationClip", (int)state);
		animation.Play(SCR_Sameer.Boss[(int)state]);
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
		
		if (state >= BossState.FLY_1 && state <= BossState.FLY_6) {
			if (SCR_Profile.showTutorial == 1) {
				if (SCR_Gameplay.instance.tutorialStep == TutorialStep.AIM) {
					predictY = y - (SCR_Gameplay.GRAVITY * BOSS_TUTORIAL_DELTA * BOSS_TUTORIAL_DELTA) * 0.5f;
					tutorialCounter += dt * 100;
					tutorialFinger.transform.localPosition = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + SCR_Helper.Sin(tutorialCounter) * SCR_Gameplay.SCREEN_W * 0.3f, predictY, tutorialTarget.transform.localPosition.z);
					tutorialFinger.SetActive (true);
					tutorialFinger.GetComponent<SCR_TutorialFinger>().Stop();
				}
				else if (SCR_Gameplay.instance.tutorialStep == TutorialStep.PUNCH) {
					predictX = x + speedX * BOSS_TUTORIAL_DELTA;
					predictY = y - (SCR_Gameplay.GRAVITY * BOSS_TUTORIAL_DELTA * BOSS_TUTORIAL_DELTA) * 0.5f;
					tutorialTarget.transform.localPosition = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + predictX, predictY, tutorialTarget.transform.localPosition.z);
					tutorialTarget.SetActive (true);
					//tutorialFinger.transform.localPosition = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + predictX, predictY, tutorialTarget.transform.localPosition.z);
					tutorialFinger.SetActive (false);
					//tutorialFinger.GetComponent<SCR_TutorialFinger>().Animate();

					if (!tutorialRange) {
						tutorialRange = Instantiate (PFB_TutorialRange);
						tutorialRange.transform.localScale = new Vector3(SCR_Gameplay.SCREEN_SCALE * 3, SCR_Gameplay.SCREEN_SCALE * 3, 1);
						tutorialRange.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + SCR_Gameplay.instance.player.GetComponent<SCR_Player>().x, SCR_Gameplay.instance.cameraHeight - SCR_Player.PLAYER_SIZE, tutorialRange.transform.position.z);
						float angle = SCR_Helper.AngleBetweenTwoPoint(SCR_Gameplay.instance.player.GetComponent<SCR_Player>().x, SCR_Gameplay.instance.cameraHeight - SCR_Player.PLAYER_SIZE, predictX, predictY);
						tutorialRange.transform.localEulerAngles = new Vector3 (0, 0, -angle);
					}
				}
			}
			if (SCR_Gameplay.instance.tutorialStep != TutorialStep.AIM && SCR_Gameplay.instance.tutorialStep != TutorialStep.PUNCH) {
				float oldSpeedY = speedY;
				
				if (bubbleTime > 0)
					speedY -= SCR_Gameplay.GRAVITY * dt * (speedY / 10000);
				else
					speedY -= SCR_Gameplay.GRAVITY * dt;
				
				if (speedY < 0 && oldSpeedY >= 0 && SCR_Gameplay.instance.tutorialStep == TutorialStep.THROW) {
					SCR_Gameplay.instance.TriggerTutorial (TutorialStep.AIM);
				}
				
				if (speedY > maxSpeedY)	speedY = maxSpeedY;
				y += speedY * dt;
				
				//if (speedY > maxSpeedY)	y += maxSpeedY * dt;
				//else 					y += speedY * dt;
				
				
				
				if (magnetTime > 0) {
					if (SCR_Gameplay.instance.player.GetComponent<SCR_Player>().IsFlyingUp()) {
						float flyAngle = SCR_Helper.AngleBetweenTwoPoint (x, y, SCR_Gameplay.instance.player.GetComponent<SCR_Player>().x, SCR_Gameplay.instance.player.GetComponent<SCR_Player>().y);
						float attractX = BOSS_MAGNET_FORCE * SCR_Helper.Sin (flyAngle);
						float attractY = BOSS_MAGNET_FORCE * SCR_Helper.Cos (flyAngle); // we don't need this actually
						x += attractX * dt;
					}
				}
				
				x += speedX * dt;
				
				
				
				if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
					x = -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
					if (bubbleTime > 0) speedX = -speedX * 0.7f;
					else speedX = -speedX;
					RandomRotate ();
					SCR_Audio.PlayBounceSound();
				}
				else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
					x = (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
					if (bubbleTime > 0) speedX = -speedX * 0.7f;
					else speedX = -speedX;
					RandomRotate ();
					SCR_Audio.PlayBounceSound();
				}
				
				if (y <= SCR_Gameplay.instance.cameraHeight - BOSS_SIZE) {
					SwitchState (BossState.FALL);
					SCR_Gameplay.instance.gameState = GameState.BOSS_FALLING;
					SCR_Gameplay.instance.Lose();
					
					if (size == BossSize.BIG) {
						Shrink();
					}
					
					SCR_Audio.PlayFallSound();
				}
				
				rotation += rotateSpeed * dt;
				
				float r = currentScale.x / originalScale.x;
				atmosBurn.SetActive (true);
				atmosBurn.transform.localPosition = new Vector3 (
					SCR_Gameplay.SCREEN_W * 0.5f + x + BOSS_BURN_OFFSET_X * r,
					y + BOSS_BURN_OFFSET_Y * r,
					atmosBurn.transform.localPosition.z);
				
				float alpha = (speedY - BOSS_BURN_SPEED_MIN) / (BOSS_BURN_SPEED_MAX - BOSS_BURN_SPEED_MIN);
				if (alpha > BOSS_BURN_ALPHA) alpha = BOSS_BURN_ALPHA;
				Color color = atmosBurn.GetComponent<SpriteRenderer>().color;
				color.a = alpha;
				atmosBurn.GetComponent<SpriteRenderer>().color = color;
				
				if (alpha > 0) {
					SCR_Gameplay.instance.ShakeCamera (0.016f);
				}
			}
		}
		else if (state == BossState.FALL) {
			speedY -= SCR_Gameplay.GRAVITY * dt;
			
			x += speedX * dt;
			y += speedY * dt;
			
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				speedX = -speedX;
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				x = (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X);
				speedX = -speedX;
			}
			
			if (y <= BOSS_START_Y) {
				y = BOSS_START_Y;
				SwitchState (BossState.SLIDE);
				SCR_Audio.PlayPunchNormalSound();
				
				smokeParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x + BOSS_SMOKE_OFFSET_X * direction, y + BOSS_SMOKE_OFFSET_Y, smokeParticle.transform.position.z);
				
				landParticle.SetActive (true);
				landParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y + BOSS_SMOKE_OFFSET_Y, landParticle.transform.position.z);
			}
			
			rotation = 0;
		}
		else if (state == BossState.SLIDE) {
			if (speedX > 0) {
				speedX -= BOSS_SLIDE_FRICTION * dt;
				direction = 1;
				if (speedX < 0) {
					speedX = 0;
					SwitchState (BossState.RUN);
					SCR_RunSound.Play();
				}
			}
			else {
				speedX += BOSS_SLIDE_FRICTION * dt;
				direction = -1;
				if (speedX > 0) {
					speedX = 0;
					SwitchState (BossState.RUN);
					SCR_RunSound.Play();
				}
			}
			
			smokeParticle.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x + BOSS_SMOKE_OFFSET_X * direction, y + BOSS_SMOKE_OFFSET_Y, smokeParticle.transform.position.z);
			ParticleSystem.EmissionModule emission = smokeParticle.GetComponent<ParticleSystem>().emission;
			emission.rateOverTime = Mathf.Abs(speedX) * BOSS_SMOKE_RATE;
			
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
			
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				SCR_RunSound.Fade();
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f - BOSS_REVERSE_X)) {
				SCR_RunSound.Fade();
			}
		}
		
		transform.position 			= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localScale 		= new Vector3 (currentScale.x * (-direction), currentScale.y, currentScale.z);
		transform.localEulerAngles 	= new Vector3 (0, 0, rotation);
		
		bossName.transform.localPosition = new Vector3 (
			SCR_Gameplay.SCREEN_W * 0.5f + x,
			y + BOSS_NAME_OFFSET_Y * currentScale.x / originalScale.x,
			bossName.transform.localPosition.z);
		
		
		float shadowScale = 1 - (y - BOSS_START_Y) / BOSS_SHADOW_DISTANCE;
		if (shadowScale < 0) shadowScale = 0;
		shadow.transform.position 	= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, shadow.transform.position.y, shadow.transform.position.z);
		shadow.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * shadowScale, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * shadowScale, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE);
	
		for (int i=0; i<6; i++) {
			moneyParticle[i].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, moneyParticle[i].transform.position.z);
		}
		
		moneyBagParticle.transform.position = moneyParticle[0].transform.position;
		
		if (enlargeTime > 0) {
			enlargeTime -= dt;
			if (enlargeTime <= 0) {
				Shrink();
				enlargeTime = 0;
			}
			else if (enlargeTime < BOSS_FLASH_DURATION * 0.8f) {
				Flash();
			}
		}
		
		if (bubbleTime > 0) {
			bubbleTime -= dt;
			if (bubbleTime < BOSS_FLASH_DURATION * 0.8f) {
				Flash();
			}
		}
		else {
			bubble.GetComponent<SCR_Bubble>().FadeOut();
		}
		
		if (magnetTime > 0) {
			magnetTime -= dt;
			if (magnetTime < BOSS_FLASH_DURATION * 0.8f) {
				Flash();
			}
		}
		else {
			attract.GetComponent<SCR_Attract>().FadeOut();
		}
		
		
		
		
		
		if (state == BossState.TALK) {
			if (talkCounter < BOSS_TALK_START) {
				talkCounter += dt;
				if (talkCounter >= BOSS_TALK_START) {
					
					SCR_Audio.PlayTalkSound();
					dialogueID ++;
					if (dialogueID >= dialogue.Length) {
						dialogueID = 0;
					}
					talkBubble.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = dialogue[dialogueID];
					talkBubble.SetActive (true);
				}
			}
			else {
				talkCounter += dt;
				if (talkCounter >= BOSS_TALK_END) {
					talkBubble.SetActive (false);
					talkCounter = 0;
				}
			}
		}
	}
	
	private void RandomFlyPose () {
		BossState s = state;
		while (s == state) {
			int random = Random.Range(0, 1000) % 6;
			int i = (int)BossState.FLY_1 + random;
			s = (BossState)i;
		};
		SwitchState (s);
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
			talkBubble.SetActive (false);
			SCR_GrabbedSound.Play();
			SCR_Audio.StopTalkSound();
		}
	}
	public void Thrown () {
		if (state == BossState.GRAB) {
			y = BOSS_START_Y;
			if (SCR_Profile.showTutorial == 1) {
				speedX = BOSS_TUTORIAL_SPEED_X * -direction;
				speedY = BOSS_TUTORIAL_SPEED_Y;
			}
			else {
				speedX = BOSS_THROWN_SPEED_X * -direction;
				speedY = BOSS_THROWN_SPEED_Y;
			}
			
			RandomRotate ();
			RandomFlyPose ();
			
			for (int i=0; i<3; i++) {
				moneyParticle[i].SetActive (true);
			}
			
			SCR_GrabbedSound.Stop();
			SCR_Audio.PlayScreamSound();
		}
	}
	public bool IsFlying () {
		return (state >= BossState.FLY_1 && state <= BossState.FLY_6) || state == BossState.FALL;
	}
	public bool IsRunning () {
		return state == BossState.RUN;
	}
	public void Punch (float px, float py, bool isSecurityGuy) {
		if (state >= BossState.FLY_1 && state <= BossState.FLY_6) {
			float handicap = BOSS_MIN_HANDICAP + (y / BOSS_HANDICAP_HEIGHT) * (1 - BOSS_MIN_HANDICAP);
			if (handicap > 1) handicap = 1;
			
			speedX += px;
			if (speedX > handicap * BOSS_MAX_SPEED_X) {
				speedX = handicap * BOSS_MAX_SPEED_X;
			}
			else if (speedX < -handicap * BOSS_MAX_SPEED_X) {
				speedX = -handicap * BOSS_MAX_SPEED_X;
			}
			
			if (isSecurityGuy) 	{
				maxSpeedY = BOSS_MAX_SPEED_Y_BONUS;
				speedY += py;
			}
			else {
				if (speedY <= BOSS_MAX_SPEED_Y) {
					maxSpeedY = BOSS_MAX_SPEED_Y;	
					speedY += py;
					if (speedY > BOSS_MAX_SPEED_Y) {
						speedY = BOSS_MAX_SPEED_Y;
					}
				}
			}
			
			RandomRotate ();
			SCR_Gameplay.instance.TriggerTutorial (TutorialStep.CONTINUE);
			
			RandomFlyPose();
			
			if (particleID == 0) {
				for (int i=0; i<3; i++) {
					ParticleSystem.EmissionModule emission = moneyParticle[i].GetComponent<ParticleSystem>().emission;
					
					if (size == BossSize.BIG) {
						emission.rateOverTimeMultiplier = 120;
					}
					else {
						emission.rateOverTimeMultiplier = 40;
					}
					
					moneyParticle[i].GetComponent<ParticleSystem>().startSpeed = Random.Range (py * 0.012f, py * 0.014f);
					moneyParticle[i].SetActive(true);
				}
				particleID = 1;
			}
			else {
				for (int i=3; i<6; i++) {
					ParticleSystem.EmissionModule emission = moneyParticle[i].GetComponent<ParticleSystem>().emission;
					
					if (size == BossSize.BIG) {
						emission.rateOverTimeMultiplier = 120;
					}
					else {
						emission.rateOverTimeMultiplier = 40;
					}
					
					moneyParticle[i].GetComponent<ParticleSystem>().startSpeed = Random.Range (py * 0.012f, py * 0.014f);
					moneyParticle[i].SetActive(true);
				}
				particleID = 0;
			}
			
			SCR_Audio.PlayScreamSound();
			bubble.GetComponent<SCR_Bubble>().Hit();
		}
	}
	public void ShowMoneyBag() {
		moneyBagParticle.SetActive(true);
	}
	public void CrashIntoObject (float angle) {
		float changeX = BOSS_CRASH_BONUS * SCR_Helper.Sin(angle);
		float changeY = BOSS_CRASH_BONUS * SCR_Helper.Cos(angle);
		
		if (changeY < 0) changeY *= 0.5f;
		
		speedX += changeX;
		speedY += changeY;
		
		float handicap = BOSS_MIN_HANDICAP + (y / BOSS_HANDICAP_HEIGHT) * (1 - BOSS_MIN_HANDICAP);
		if (handicap > 1) handicap = 1;
		if (speedX > handicap * BOSS_MAX_SPEED_X) {
			speedX = handicap * BOSS_MAX_SPEED_X;
		}
		else if (speedX < -handicap * BOSS_MAX_SPEED_X) {
			speedX = -handicap * BOSS_MAX_SPEED_X;
		}
		
		Time.timeScale = 0.1f;
	}
	public void ReAdjustY () {
		if (y > BOSS_SIZE + SCR_Gameplay.SCREEN_H) {
			y = BOSS_SIZE + SCR_Gameplay.SCREEN_H;
		}
	}
	public void HideTutorial () {
		tutorialFinger.SetActive (false);
		tutorialTarget.SetActive (false);
		if (tutorialRange) tutorialRange.SetActive (false);
	}
	
	
	
	public void Bubble () {
		bubbleTime = BOSS_BUBBLE_DURATION;
		bubble.GetComponent<SCR_Bubble>().FadeIn();
	}
	public void Magnet () {
		magnetTime = BOSS_MAGNET_DURATION;
		attract.GetComponent<SCR_Attract>().FadeIn();
	}
	
	public void Enlarge () {
		enlargeTime = BOSS_ENLARGE_DURATION;
		
		if (size == BossSize.SMALL) {
			float newScale = originalScale.x * BOSS_ENLARGE_MULTIPLIER;
			iTween.ValueTo(gameObject, iTween.Hash("from", Mathf.Abs(transform.localScale.x), "to", newScale, "time", 1.0f, "easetype", "easeOutElastic", "onupdate", "UpdateScale"));
			size = BossSize.BIG;
		}
	}
	public void Shrink () {
		if (size == BossSize.BIG) {
			iTween.ValueTo(gameObject, iTween.Hash("from", Mathf.Abs(transform.localScale.x), "to", originalScale.x, "time", 1.0f, "easetype", "easeOutElastic", "onupdate", "UpdateScale"));
			size = BossSize.SMALL;
		}
	}
	private void UpdateScale (float scale) {
		currentScale = new Vector3(scale, scale, 1);
	}
	
	
	public void Flash () {
		if (!flashing) {
			iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1.0f, "time", BOSS_FLASH_DURATION / BOSS_FLASH_TIMES * 0.5f, "easetype", "easeInOutSine", "onupdate", "UpdateFlashAmount", "oncomplete", "CompleteFlash", "looptype", "pingPong"));
			flashing = true;
		}
	}
	private void UpdateFlashAmount (float amount) {
		material.SetFloat("_FlashAmount", amount);
	}
	private void CompleteFlash () {
		flashCount++;
		if (flashCount >= BOSS_FLASH_TIMES * 2) {
			flashCount = 0;
			flashing = false;
			iTween.Stop(gameObject);
		}
	}
	// ==================================================
	
	// this function just returns a number in the range -1 to +1
	// and is used by Array.Sort to 'shuffle' the array
	private int RandomSort(string a, string b) {
		return Random.Range(-1, 2);
	}
}
