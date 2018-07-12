using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_SecurityProgress : MonoBehaviour {
	private Image image;
	private Material material;
	
	[System.NonSerialized] public float powerUpTime;
	
	private bool prepareToFlash = false;
	
	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		material = image.material;
		powerUpTime = -1;
		UpdateFlashAmount(0);
	}
	
	// Update is called once per frame
	void Update () {
		if (powerUpTime >= 0) {
			powerUpTime += Time.deltaTime;
			if (powerUpTime >= SCR_Gameplay.POWER_UP_SECURITY_DURATION) {
				StopFlashing();
			}
		}
	}
	
	public void SetTargetProgress(float progress) {
		if (!prepareToFlash) {
			iTween.Stop(gameObject);
			iTween.ValueTo(gameObject, iTween.Hash("from", image.fillAmount, "to", progress, "time", 0.5f, "easetype", "easeInOutSine", "onupdate", "UpdateProgress", "ignoretimescale", true));
		}
	}
	
	private void UpdateProgress(float progress) {
		image.fillAmount = progress;
	}
	
	public void Flash() {
		if (image.fillAmount < 1) {
			iTween.Stop(gameObject);
			iTween.ValueTo(gameObject, iTween.Hash("from", image.fillAmount, "to", 1, "time", 0.5f, "easetype", "easeInOutSine", "onupdate", "UpdateProgress", "oncomplete", "StartFlashing", "ignoretimescale", true));
			prepareToFlash = true;
		}
		else {
			StartFlashing();
		}
	}
	
	private void StartFlashing() {
		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1.0f, "time", 0.333f, "easetype", "easeInOutSine", "onupdate", "UpdateFlashAmount", "looptype", "pingPong"));
		powerUpTime = 0;
		prepareToFlash = false;
	}
	
	private void StopFlashing() {
		iTween.Stop(gameObject);
		iTween.ValueTo(gameObject, iTween.Hash("from", material.GetFloat("_FlashAmount"), "to", 0, "time", 0.333f, "easetype", "easeInOutSine", "onupdate", "UpdateFlashAmount"));
		SetTargetProgress(0);
		powerUpTime = -1;
	}
	
	public void UpdateFlashAmount (float amount) {
		material.SetFloat("_FlashAmount", amount);
	}
}
