using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Attract : MonoBehaviour {
	public const float ATTRACT_SCALE	= 2.0f;
	public const float FADE_SPEED		= 2.5f;
	public const float SCALE_SPEED		= 0.5f;
	
	public bool 	live = false;
	public float 	alpha = 0;
	
	public GameObject[] children = new GameObject[3];
	public float[] childScale = new float[3];
	
	private void Start () {
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * ATTRACT_SCALE, SCR_Gameplay.SCREEN_SCALE * ATTRACT_SCALE, 1);
		
		childScale[0] = 0.99f;
		childScale[1] = 0.66f;
		childScale[2] = 0.33f;
		
		for (int i=0; i<3; i++) {
			children[i] = transform.GetChild(i).gameObject;
			children[i].transform.localScale = new Vector3 (childScale[i], childScale[i], 1.0f);
		}
	}

	private void Update () {
		float dt = Time.deltaTime;
		
		transform.position = new Vector3 (SCR_Gameplay.instance.boss.transform.position.x, SCR_Gameplay.instance.boss.transform.position.y, transform.position.z);
		
		if (live) {
			alpha += FADE_SPEED * dt;
			if (alpha > 0.5f) alpha = 0.5f;
		}
		else {
			alpha -= FADE_SPEED * dt;
			if (alpha < 0) alpha = 0;
		}
		for (int i=0; i<3; i++) {
			childScale[i] -= SCALE_SPEED * dt;
			if (childScale[i] < 0) childScale[i] += 1.0f;
			children[i].transform.localScale = new Vector3 (childScale[i], childScale[i], 1.0f);
			
			float localAlpha = (1.0f - childScale[i]) * 2;
			if (localAlpha > 1) localAlpha = 1;
			Color color = children[i].GetComponent<SpriteRenderer>().color;
			color.a = alpha * localAlpha;
			children[i].GetComponent<SpriteRenderer>().color = color;
		}
	}
	
	public void FadeIn () {
		live = true;
	}
	
	public void FadeOut () {
		live = false;
	}
}
