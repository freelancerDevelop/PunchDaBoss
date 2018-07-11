using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PowerUp : MonoBehaviour {
	public const float POWER_UP_SIZE 		= 300;
	public const float POWER_UP_SCALE		= 1;
	public const float POWER_UP_SPEED_Y 	= 1000;
	
	[System.NonSerialized] public float	x	= 0;
	[System.NonSerialized] public float	y	= 0;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * POWER_UP_SCALE, SCR_Gameplay.SCREEN_SCALE * POWER_UP_SCALE, 1);
	}
	
	// Update is called once per frame
	void Update () {
		y += POWER_UP_SPEED_Y * Time.deltaTime;
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		
		if (y <= SCR_Gameplay.instance.cameraHeight - POWER_UP_SIZE) {
			gameObject.SetActive (false);
			SCR_Gameplay.instance.powerUp = null;
		}
	}
	
	public void Spawn (float px, float py) {
		x = px;
		y = py;
		
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
	}
	
	public void AddDeltaCameraToObject (float deltaCamera) {
		y += 0.5f * deltaCamera;
	}
}
