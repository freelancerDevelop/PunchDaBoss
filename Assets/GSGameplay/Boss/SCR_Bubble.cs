using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Bubble : MonoBehaviour {
	public const float BUBBLE_SCALE		= 0.8f;
	public const float FADE_SPEED		= 5.0f;
	
	public const float DEFAULT_VIBRATE	= 0.1f;
	public const float HIT_VIBRATE		= 0.2f;
	public const float VIBRATE_SUPRESS	= 0.05f;
	
	public bool 	live = false;
	public float 	alpha = 0;
	
	private float 	vibrate = 0;
	private int	 	vibrateDir = 1;
	private float 	vibrateMax = DEFAULT_VIBRATE;
	
	private void Start () {
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * BUBBLE_SCALE, SCR_Gameplay.SCREEN_SCALE * BUBBLE_SCALE, 1);
	}
	
	private void Update () {
		float dt = Time.deltaTime;
		
		transform.position = new Vector3 (SCR_Gameplay.instance.boss.transform.position.x, SCR_Gameplay.instance.boss.transform.position.y, transform.position.z);
		
		if (live) {
			alpha += FADE_SPEED * dt;
			if (alpha > 1) alpha = 1;
		}
		else {
			alpha -= FADE_SPEED * dt;
			if (alpha < 0) alpha = 0;
		}
		Color color = GetComponent<SpriteRenderer>().color;
		color.a = alpha;
		GetComponent<SpriteRenderer>().color = color;
		
		if (vibrateMax > DEFAULT_VIBRATE) {
			vibrateMax -= VIBRATE_SUPRESS * dt;
		}
		
		float vibrateSpeed = (vibrateMax * 10) * (vibrateMax * 10) * 0.01f; // vibrateSpeed from 0.01 to 0.04 now
		vibrate += vibrateSpeed * vibrateDir;
		if ((vibrate > vibrateMax && vibrateDir > 0) || (vibrate < -vibrateMax && vibrateDir < 0)) {
			vibrateDir = -vibrateDir;
		}
		
		float baseScale = SCR_Gameplay.SCREEN_SCALE * BUBBLE_SCALE;
		transform.localScale = new Vector3 (baseScale * (1 + vibrate), baseScale * (1 - vibrate), 1);
	}
	
	public void FadeIn () {
		live = true;
	}
	
	public void FadeOut () {
		live = false;
	}
	
	public void Hit () {
		if (live) {
			vibrateMax = HIT_VIBRATE;
		}
	}
}
