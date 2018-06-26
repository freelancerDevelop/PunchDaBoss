using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TutorialFinger : MonoBehaviour {
	private const float SCALE_SPEED 	= 300;
	private const float SCALE_VARIANCE 	= 0.08f;
	
	private float 	scaleCount = 0;
	private bool 	scaleMovement = true;
	
	private void Start() {
		gameObject.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		
		Animate();
	}
	
	private void Update() {
		if (scaleMovement) {
			float dt = Time.deltaTime;
			scaleCount += dt * SCALE_SPEED;
			if (scaleCount > 360) scaleCount -= 360;
			
			float scale = SCR_Helper.Sin(scaleCount) * SCALE_VARIANCE * SCR_Gameplay.SCREEN_SCALE;
			
			gameObject.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE + scale, SCR_Gameplay.SCREEN_SCALE + scale, 1);
		}
		else {
			gameObject.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		}
	}
	
	public void Animate() {
		scaleMovement = true;
	}
	public void Stop() {
		scaleMovement = false;
	}
}
