using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Sameer : MonoBehaviour {
	public static readonly string[] Player = {
		"Boy_Idle",
		"Boy_Transform",
		"Boy_Walk",
		"Boy_Walk_Grab",
		"Boy_Punch",
		"Boy_Hit",
		"Boy_Hit",
		"Boy_Hit",
		"Boy_Land_pose",
		"Boy_Land",
		"Boy_Look"
	};
	
	public static readonly string[] Boss = {
		"Boss_Idle",
		"Boss_Grab_Loop",
		"Boss_Fly",
		"Boss_Fly_02",
		"Boss_Fly_03_Pose",
		"Boss_Fall_Pose",
		"Boss_Fall_Pose1",
		"Boss_Run",
	};
	
	public static readonly string[] Security = {
		"Scu_Idle",
		"Scu_Cheer",
		"Scu_Hit",
		"Scu_Hit"
	};
}

/*
public enum BossState {
	TALK = 0,
	GRAB,
	FLY_1,
	FLY_2,
	FLY_3,
	FALL,
	SLIDE,
	RUN
}

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

public enum SecurityState {
	STAND = 0,
	CHEER,
	FLY_UP,
	FLY_DOWN
}
*/