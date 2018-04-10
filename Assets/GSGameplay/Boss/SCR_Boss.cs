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
	private Animator 	animator	= null;
	private BossState 	state		= BossState.TALK;

	private void Start () {
		animator = GetComponent<Animator>();
	}
	
	private void SwitchAnimation () {
		animator.SetInteger("AnimationClip", (int)state);
	}
	
	private void Update () {
		//GetComponent(SpriteRenderer).sprite = newSprite;
	}
}
