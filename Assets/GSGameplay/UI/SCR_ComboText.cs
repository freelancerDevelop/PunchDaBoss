using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class SCR_ComboText : MonoBehaviour {
	private const float COMBO_TEXT_FADE_SPEED = 2;
	private const float COMBO_TEXT_FLY_SPEED = 300;
	
	public float alpha = 0.0f;
	public float x = 0.0f;
	public float y = 0.0f;
	
	public void Spawn (string caption, float px, float py) {
		x = px;
		y = py;
		alpha = 1.0f;
		
		transform.localScale = new Vector3 (1, 1 ,1);
		transform.GetChild(0).gameObject.GetComponent<Text>().text = caption;
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
	}
	
	private void Update () {
		float dt = Time.deltaTime;
		alpha -= COMBO_TEXT_FADE_SPEED * dt;
		
		gameObject.GetComponent<Image>().color = new Color (1, 1, 1, alpha);
		transform.GetChild(0).gameObject.GetComponent<Text>().color = new Color (1, 1, 1, alpha);
		
		y += COMBO_TEXT_FLY_SPEED * alpha * dt;
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
		
		if (alpha <= 0) {
			gameObject.SetActive (false);
		}
	}
}
