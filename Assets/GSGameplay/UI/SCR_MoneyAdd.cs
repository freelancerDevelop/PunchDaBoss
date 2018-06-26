using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SCR_MoneyAdd : MonoBehaviour {
	private const float COMBO_TEXT_FADE_SPEED = 2;
	public float alpha = 0.0f;
	
	public void SetText (string text) {
		gameObject.GetComponent<Text>().text = "+$" + text;
		alpha = 2.0f;
	}
	
	// Update is called once per frame
	private void Update () {
		alpha -= COMBO_TEXT_FADE_SPEED * Time.deltaTime;
		if (alpha < 0) alpha = 0;
		
		if (alpha > 1) {
			gameObject.GetComponent<Text>().color = new Color (0, 1, 0, 1);
		}
		else {
			gameObject.GetComponent<Text>().color = new Color (0, 1, 0, alpha);
		}
	}
}
