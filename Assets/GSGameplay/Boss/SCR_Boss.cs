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
	// Const
	public const float BOSS_START_X		= -100;
	public const float BOSS_START_Y		= 350;
	public const float BOSS_SCALE		= 0.4f;
	
	// Stuff
	private Animator 	animator		= null;
	private BossState 	state			= BossState.TALK;
	private int			direction		= 1;

	private void Start () {
		animator = GetComponent<Animator>();
		
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + BOSS_START_X, BOSS_START_Y, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, 1);
		
		SwitchState (BossState.TALK);
	}
	
	public void SwitchState (BossState s) {
		state = s;
		animator.SetInteger("AnimationClip", (int)state);
	}
	
	private void Update () {
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + BOSS_START_X, BOSS_START_Y - SCR_Gameplay.instance.cameraHeight, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * BOSS_SCALE, 1);
	}
}
