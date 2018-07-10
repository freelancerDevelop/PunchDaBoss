using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_SecurityProgress : MonoBehaviour {
	private Image image;
	
	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SetTargetProgress(float progress) {
		iTween.Stop(gameObject);
		iTween.ValueTo(gameObject, iTween.Hash("from", image.fillAmount, "to", progress, "time", 0.5f, "easetype", "easeInOutSine", "onupdate", "UpdateProgress", "ignoretimescale", true));
	}
	
	private void UpdateProgress(float progress) {
		image.fillAmount = progress;
	}
}
