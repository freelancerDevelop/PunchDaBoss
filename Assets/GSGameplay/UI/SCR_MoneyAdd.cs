using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SCR_MoneyAdd : MonoBehaviour {
	private const float COMBO_TEXT_FADE_SPEED = 2;
	private const float COMBO_TEXT_FLY_SPEED = 300;
	private const float COMBO_TEXT_VIBRATION = 12;
	
	public bool	 special = false;
	public float alpha = 0.0f;
	public float x = 0.0f;
	public float y = 0.0f;
	public bool  moving = false;
		
	public void SetText (string text) {
		gameObject.GetComponent<Text>().text = "+$" + text;
		alpha = 2.0f;
		moving = false;
	}
	
	public void Spawn (string text, float px, float py) {
		gameObject.GetComponent<Text>().text = "+$" + text;
		alpha = 2.0f;
		
		x = px;
		y = py;
		
		transform.localScale = new Vector3 (1, 1 ,1);
		if (special) {
			transform.localScale = new Vector3 (1.2f, 1.2f, 1.2f);
		}
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
		
		moving = true;
	}
	
	// Update is called once per frame
	private void Update () {
		float dt = Time.deltaTime;
		
		// alpha
		alpha -= COMBO_TEXT_FADE_SPEED * dt;
		if (alpha < 0) alpha = 0;
		
		if (alpha > 1) {
			gameObject.GetComponent<Text>().color = new Color (0, 1, 0, 1);
		}
		else {
			gameObject.GetComponent<Text>().color = new Color (0, 1, 0, alpha);
		}
		
		// position
		if (moving) {
			y += COMBO_TEXT_FLY_SPEED * alpha * dt;
			
			if (!special) {
				gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
			}
			else {
				gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x + Random.Range(-COMBO_TEXT_VIBRATION, COMBO_TEXT_VIBRATION), y + Random.Range(-COMBO_TEXT_VIBRATION, COMBO_TEXT_VIBRATION));
			}
		}
	}
}
