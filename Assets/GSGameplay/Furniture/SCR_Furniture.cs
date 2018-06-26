using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Furniture : MonoBehaviour {
	public const float ROTATION_SPEED = 360;
	public const float FURNITURE_SIZE = 450;
	
	public float startY		= 0;
	public float angle 		= 0;
	public float x 			= 0;
	public float y 			= 0;
	public float speedA 	= 0;
	public float speedX 	= 0;
	public float speedY 	= 0;
	public bool  broken		= false;
	

	public void Spawn (float px, float py) {
		x = px;
		y = py;
		startY = py;
		
		transform.position 		= new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localScale 	= new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
	}
	
	public void Break (float px, float py, float force) {
		speedX = Random.Range(force * 0.1f, force * 0.3f);
		speedY = Random.Range(force * 0.3f, force);
		
		speedA = Random.Range (ROTATION_SPEED * 0.4f, ROTATION_SPEED);
		if (Random.Range(0, 100) % 2 == 0) {
			speedA = -speedA;
		}
		
		broken = true;
	}
	
	private void Update () {
		float dt = Time.deltaTime;
		
		if (broken) {
			speedY -= SCR_Gameplay.GRAVITY * dt;
			
			x += speedX * dt;
			y += speedY * dt;
			angle += speedA * dt;
			
			if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f)) {
				x = -(SCR_Gameplay.SCREEN_W * 0.5f);
				speedX = -speedX;
			}
			else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f)) {
				x = (SCR_Gameplay.SCREEN_W * 0.5f);
				speedX = -speedX;
			}
			
			if (y <= SCR_Gameplay.instance.cameraHeight - FURNITURE_SIZE) {
				gameObject.SetActive (false);
			}
			
			transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
			transform.localEulerAngles = new Vector3 (0, 0, angle);	
		}
	}
}
