using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class SCR_ComboText : MonoBehaviour {
	private const float COMBO_TEXT_FADE_SPEED = 2;
	private const float COMBO_TEXT_FLY_SPEED = 300;
	private const float COMBO_TEXT_VIBRATION = 12;
	
	public bool	 special = false;
	public float alpha = 0.0f;
	public float x = 0.0f;
	public float y = 0.0f;
	
	public void Spawn (float px, float py) {
		x = px;
		y = py;
		alpha = 1.0f;
		
		transform.localScale = new Vector3 (1, 1 ,1);
		if (special) {
			transform.localScale = new Vector3 (1.2f, 1.2f, 1.2f);
		}
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
	}
	
	private void Update () {
		float dt = Time.deltaTime;
		
		if (!special) {
			alpha -= COMBO_TEXT_FADE_SPEED * dt;
		}
		else {
			alpha -= COMBO_TEXT_FADE_SPEED * dt * 0.5f;
		}
		
		gameObject.GetComponent<Image>().color = new Color (1, 1, 1, alpha);
		
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
