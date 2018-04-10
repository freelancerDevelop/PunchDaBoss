using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SCR_Background : MonoBehaviour {
	// Const
	private const int 	BACKGROUND_HEIGHT 			= 5760;
	private const int 	BACKGROUND_LOOP_HEIGHT 		= 1920;
	
	private const float BACKGROUND_SCROLL_RATIO		= 0.2f;
	private const float MIDDLEGROUND_SCROLL_RATIO	= 0.5f;
	private const float FOREGROUND_SCROLL_RATIO		= 1.0f;
	
	// Prefab
	public GameObject 		PFB_Background			= null;
	public GameObject		PFB_BackgroundLoop		= null;
	public GameObject		PFB_Middleground		= null;
	public GameObject		PFB_Foreground			= null;
	
	// Object
	private GameObject		background				= null;
	private GameObject[] 	backgroundLoop			= null;
	private GameObject 		middleground			= null;
	private GameObject 		foreground				= null;
	
	// Instance
	public static SCR_Background	instance		= null;
	
	private float cameraTempY = 0;
	
	// Init
	private void Start () {
		instance = this;
		
		background = Instantiate (PFB_Background);
		background.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		
		backgroundLoop = new GameObject[2];
		backgroundLoop[0] = Instantiate (PFB_BackgroundLoop);
		backgroundLoop[1] = Instantiate (PFB_BackgroundLoop);
		for (var i=0; i<2; i++) {
			backgroundLoop[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		}
		
		SetCameraY (0);
	}
	
	// Set camera position
	public static void SetCameraY (float cameraY) {
		float backgroundY = cameraY * BACKGROUND_SCROLL_RATIO;
		if (backgroundY < BACKGROUND_HEIGHT) {
			instance.background.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY, instance.background.transform.position.z);
			instance.backgroundLoop[0].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY + BACKGROUND_HEIGHT, instance.backgroundLoop[0].transform.position.z);
			instance.backgroundLoop[1].SetActive (false);
		}
		else {
			backgroundY = backgroundY - BACKGROUND_HEIGHT;
			backgroundY = backgroundY % BACKGROUND_LOOP_HEIGHT;
			for (var i=0; i<2; i++) {
				instance.backgroundLoop[i].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY + i * BACKGROUND_LOOP_HEIGHT, instance.backgroundLoop[i].transform.position.z);
			}
			instance.backgroundLoop[1].SetActive (true);
			instance.background.SetActive (false);
		}
	}
	
	private void Update () {
		cameraTempY += Time.deltaTime * 3000;
		SetCameraY (cameraTempY);
	}
	
}
