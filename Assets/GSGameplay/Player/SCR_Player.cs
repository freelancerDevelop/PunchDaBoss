using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
	TALK = 0,
	WALK,
	GRAB,
	THROW,
	FLY_UP,
	FLY_DOWN
}

public class SCR_Player : MonoBehaviour {
	// Const
	public const float PLAYER_START_X	= 100;
	public const float PLAYER_START_Y	= 350;
	public const float PLAYER_SCALE		= 0.4f;
	
	// Stuff
	private Animator 	animator		= null;
	private PlayerState state			= PlayerState.TALK;
	private int			direction		= -1;
	
	
	
	private void Start () {
		animator = GetComponent<Animator>();
		
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + PLAYER_START_X, PLAYER_START_Y, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE, 1);
		
		SwitchState (PlayerState.TALK);
	}
	
	public void SwitchState (PlayerState s) {
		state = s;
		animator.SetInteger("AnimationClip", (int)state);
	}
	
	private void Update () {
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + PLAYER_START_X, PLAYER_START_Y - SCR_Gameplay.instance.cameraHeight, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE * direction, SCR_Gameplay.SCREEN_SCALE * PLAYER_SCALE, 1);
	}
}
