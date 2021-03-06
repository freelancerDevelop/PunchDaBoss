﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType {
	PIZZA = 0,
	BUBBLE,
	MAGNET,
	COUNT
}

public class SCR_PowerUp : MonoBehaviour {
	public const float POWER_UP_SIZE 			= 300;
	public const float POWER_UP_SCALE			= 0.8f;
	public const float POWER_UP_SPEED_Y 		= 1000;
	public const float POWER_UP_ROTATION_SPEED	= -200;
	
	
	[System.NonSerialized] public float	x		= 0;
	[System.NonSerialized] public float	y		= 0;
	
	public PowerUpType type;
	
	private float angle = 0;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * POWER_UP_SCALE, SCR_Gameplay.SCREEN_SCALE * POWER_UP_SCALE, 1);
		angle = transform.localEulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
		y += POWER_UP_SPEED_Y * Time.deltaTime;
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		
		angle += POWER_UP_ROTATION_SPEED * Time.deltaTime;
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
	}
	
	public void Spawn () {
		x = Random.Range (-(SCR_Gameplay.SCREEN_W - POWER_UP_SIZE) * 0.5f, (SCR_Gameplay.SCREEN_W - POWER_UP_SIZE) * 0.5f);
		y = SCR_Gameplay.instance.cameraHeight + SCR_Gameplay.SCREEN_H;
						
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
	}
	
	public void AddDeltaCameraToObject (float deltaCamera) {
		y += 0.5f * deltaCamera;
	}
}
