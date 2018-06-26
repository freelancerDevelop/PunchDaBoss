using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_LightBar : MonoBehaviour {
	private const float MIN_SCALE = 0.008f;
	private const float MAX_SCALE = 5.00f;
	private const float MAX_SPEED = 200;
	
	public static float deltaCamera = 0;
	
	private float x = 0;
	private float y = 0;
	
	private void Start () {
		Spawn();
	}
	
	private void Spawn () {
		x = Random.Range (0, SCR_Gameplay.SCREEN_W);
		y = SCR_Gameplay.instance.cameraHeight + Random.Range (-SCR_Gameplay.SCREEN_H, SCR_Gameplay.SCREEN_H * 2);
		
		transform.position = new Vector3 (x, y, transform.position.z);
	}
	
	private void Update () {
		float realDelta = deltaCamera / Time.timeScale;
		float scale = MIN_SCALE + ((realDelta * realDelta) / (MAX_SPEED * MAX_SPEED)) * (MAX_SCALE - MIN_SCALE);
		transform.localScale = new Vector3(SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE * scale, 1);
		
		if (y < SCR_Gameplay.instance.cameraHeight - SCR_Gameplay.SCREEN_H || y > SCR_Gameplay.instance.cameraHeight + SCR_Gameplay.SCREEN_H * 2) {
			Spawn();
		}
	}
}
