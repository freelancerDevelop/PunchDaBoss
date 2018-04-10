using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SCR_Background : MonoBehaviour {
	// Const
	private const int 	BACKGROUND_HEIGHT 			= 1920;
	private const int 	BACKGROUND_PART 			= 5;
	
	private const float BACKGROUND_SCROLL_RATIO		= 0.2f;
	private const float MIDDLEGROUND_SCROLL_RATIO	= 0.6f;
	private const float FOREGROUND_SCROLL_RATIO		= 1.0f;
	
	private const float BACKGROUND_OFFSET			= 200;
	private const float MIDDLEGROUND_OFFSET			= 0;

	
	// Prefab
	public GameObject[] 	PFB_Background			= null;
	public GameObject		PFB_Middleground		= null;
	public GameObject		PFB_Foreground			= null;
	
	// Object
	private GameObject[]	background				= null;
	private GameObject 		middleground			= null;
	private GameObject 		foreground				= null;
	
	// Instance
	public static SCR_Background	instance		= null;
	
	private float cameraTempY = 0;
	
	// Init
	private void Start () {
		instance = this;
		
		background = new GameObject[BACKGROUND_PART];
		for (var i=0; i<BACKGROUND_PART; i++) {
			background[i] = Instantiate (PFB_Background[i]);
			background[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		}
		
		middleground = Instantiate (PFB_Middleground);
		middleground.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		
		foreground = Instantiate (PFB_Foreground);
		foreground.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		
		SetCameraY (0);
	}
	
	// Set camera position
	public static void SetCameraY (float cameraY) {
		float backgroundY = cameraY * BACKGROUND_SCROLL_RATIO;
		
		for (int i=0; i<BACKGROUND_PART; i++) {
			instance.background[i].SetActive (false);
		}
		
		if (backgroundY < (BACKGROUND_PART-1) * BACKGROUND_HEIGHT) {
			for (int i=0; i<BACKGROUND_PART-1; i++) {
				if (backgroundY >= i * BACKGROUND_HEIGHT && backgroundY < (i+1) * BACKGROUND_HEIGHT) {
					instance.background[i].SetActive (true);
					instance.background[i+1].SetActive (true);
					
					backgroundY = backgroundY % BACKGROUND_HEIGHT;
					instance.background[i].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY + BACKGROUND_OFFSET, instance.background[i].transform.position.z);
					instance.background[i+1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY + BACKGROUND_HEIGHT + BACKGROUND_OFFSET, instance.background[i+1].transform.position.z);
				}
			}
		}
		else {
			backgroundY = backgroundY % BACKGROUND_HEIGHT;
			instance.background[BACKGROUND_PART-2].SetActive (true);
			instance.background[BACKGROUND_PART-1].SetActive (true);
			instance.background[BACKGROUND_PART-2].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY, instance.background[BACKGROUND_PART-2].transform.position.z);
			instance.background[BACKGROUND_PART-1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY + BACKGROUND_HEIGHT, instance.background[BACKGROUND_PART-1].transform.position.z);
		}
		
		
		float middlegroundY = cameraY * MIDDLEGROUND_SCROLL_RATIO;
		instance.middleground.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -middlegroundY + MIDDLEGROUND_OFFSET, instance.middleground.transform.position.z);
		
		float foregroundY = cameraY * FOREGROUND_SCROLL_RATIO;
		instance.foreground.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -foregroundY, instance.foreground.transform.position.z);
	}
	
	private void Update () {
		//cameraTempY += Time.deltaTime * 50;
		//SetCameraY (cameraTempY);
	}
	
}
