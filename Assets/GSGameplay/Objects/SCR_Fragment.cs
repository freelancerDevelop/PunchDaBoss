using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Fragment : MonoBehaviour {
	public const float ROTATION_SPEED = 360;
	public const float FRAG_SPEED_X = 3000;
	public const float FRAG_SPEED_Y = 5000;
	
	public float angle 		= 0;
	public float x 			= 0;
	public float y 			= 0;
	public float speedA 	= 0;
	public float speedX 	= 0;
	public float speedY 	= 0;
	public float size		= 0;
	
	private void Start () {
		
	}
	
	public void Spawn (float px, float py, Sprite image, float ps, float scale) {
		x = px;
		y = py;
		
		speedX = Random.Range(-FRAG_SPEED_X, FRAG_SPEED_X);
		speedY = Random.Range(0, FRAG_SPEED_Y);
		
		speedA = Random.Range (ROTATION_SPEED * 0.4f, ROTATION_SPEED);
		if (Random.Range(0, 100) % 2 == 0) {
			speedA = -speedA;
		}
		
		gameObject.GetComponent<SpriteRenderer>().sprite = image;
		size = ps;
		
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * scale, SCR_Gameplay.SCREEN_SCALE * scale, 1);
	}
	
	private void Update () {
		float dt = Time.deltaTime;
		
		speedY -= SCR_Gameplay.GRAVITY * dt;
		
		x += speedX * dt;
		y += speedY * dt;
		angle += speedA * dt;
		
		if (x <= -(SCR_Gameplay.SCREEN_W * 0.5f)) {
			x = -(SCR_Gameplay.SCREEN_W * 0.5f);
			speedX = -speedX * 0.75f;
		}
		else if (x >= (SCR_Gameplay.SCREEN_W * 0.5f)) {
			x = (SCR_Gameplay.SCREEN_W * 0.5f);
			speedX = -speedX * 0.75f;
		}
		
		if (y <= SCR_Gameplay.instance.cameraHeight - size) {
			gameObject.SetActive (false);
		}
		
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		transform.localEulerAngles = new Vector3 (0, 0, angle);	
	}
}
