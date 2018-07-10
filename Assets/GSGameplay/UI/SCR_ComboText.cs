using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class SCR_ComboText : MonoBehaviour {
	//private const float COMBO_TEXT_FADE_SPEED = 2.0f;
	private const float COMBO_TEXT_FLY_SPEED = 300;
	private const float COMBO_TEXT_VIBRATION = 12;
	
	public bool	 special = false;
	public float alpha = 0.0f;
	public float x = 0.0f;
	public float y = 0.0f;
	
	private float red = 0;
	private float green = 0;
	private float blue = 0;
	
	private float comboTextFadeSpeed = 0;
	
	public void Spawn (float px, float py) {
		comboTextFadeSpeed = 1 / SCR_Gameplay.COMBO_TIME;
		
		x = px;
		y = py;
		alpha = 1.0f;
		
		transform.localScale = new Vector3 (0.25f, 0.25f ,1);
		if (special) {
			transform.localScale = new Vector3 (0.3f, 0.3f, 1);
		}
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
	}
	
	public void SetColor (float r, float g, float b) {
		GetComponent<Text>().color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1);
		red = r / 255;
		green = g / 255;
		blue = b / 255;
	}
	
	private void Update () {
		float dt = Time.deltaTime;
		
		//if (!special) {
			alpha -= comboTextFadeSpeed * dt;
		//}
		//else {
		//	alpha -= COMBO_TEXT_FADE_SPEED * dt * 0.5f;
		//}
		
		//gameObject.GetComponent<Image>().color = new Color (1, 1, 1, alpha);
		gameObject.GetComponent<Text>().color = new Color (red, green, blue, alpha);
		
		y += COMBO_TEXT_FLY_SPEED * alpha * dt;
		
		if (!special) {
			gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
		}
		else {
			gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x + Random.Range(-COMBO_TEXT_VIBRATION, COMBO_TEXT_VIBRATION), y + Random.Range(-COMBO_TEXT_VIBRATION, COMBO_TEXT_VIBRATION));
		}
		
		if (alpha <= 0) {
			gameObject.SetActive (false);
		}
	}
}
