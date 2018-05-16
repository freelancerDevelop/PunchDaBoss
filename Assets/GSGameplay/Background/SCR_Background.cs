using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SCR_Background : MonoBehaviour {
	// Const
	private const int 	BACKGROUND_HEIGHT 			= 1920;
	private const int 	BACKGROUND_PART 			= 5;
	
	private const float BACKGROUND_SCROLL_RATIO		= 0.01f;
	private const float MIDDLEGROUND_1_SCROLL_RATIO	= 0.05f;
	private const float MIDDLEGROUND_2_SCROLL_RATIO	= 0.15f;
	private const float FOREGROUND_SCROLL_RATIO		= 1.0f;
	
	private const float MIDDLEGROUND_1_OFFSET		= 200;
	private const float MIDDLEGROUND_2_OFFSET		= 80;
	
	private const float	CLOUD_SIZE		 			= 200;
	private const float	CLOUD_SIZE_SPACING 			= 1000;

	// Prefab
	public GameObject[] 	PFB_Background			= null;
	public GameObject		PFB_Middleground_1		= null;
	public GameObject		PFB_Middleground_2		= null;
	public GameObject		PFB_Foreground			= null;
	public GameObject		PFB_Cloud				= null;
	
	// Object
	private GameObject[]	background				= null;
	private GameObject 		middleground_1			= null;
	private GameObject 		middleground_2			= null;
	private GameObject 		foreground				= null;
	private GameObject[]	cloud					= null;
	
	// Others
	private float[]			cloudY					= null;
	
	// Instance
	public static SCR_Background	instance		= null;
	
	
	// Init
	private void Start () {
		instance = this;
		
		background = new GameObject[BACKGROUND_PART];
		for (var i=0; i<BACKGROUND_PART; i++) {
			background[i] = Instantiate (PFB_Background[i]);
			background[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		}
		
		middleground_1 = Instantiate (PFB_Middleground_1);
		middleground_1.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		
		middleground_2 = Instantiate (PFB_Middleground_2);
		middleground_2.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		
		foreground = Instantiate (PFB_Foreground);
		foreground.transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		
		cloud = new GameObject[3];
		cloudY = new float[3];
		for (var i=0; i<3; i++) {
			cloud[i] = Instantiate (PFB_Cloud);
			cloud[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
			cloudY[i] = 0;
		}
		
		DeployCloud ();
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
					instance.background[i].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY + cameraY, instance.background[i].transform.position.z);
					instance.background[i+1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY + cameraY + BACKGROUND_HEIGHT, instance.background[i+1].transform.position.z);
				}
			}
		}
		else {
			backgroundY = backgroundY % BACKGROUND_HEIGHT;
			instance.background[BACKGROUND_PART-2].SetActive (true);
			instance.background[BACKGROUND_PART-1].SetActive (true);
			instance.background[BACKGROUND_PART-2].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY + cameraY, instance.background[BACKGROUND_PART-2].transform.position.z);
			instance.background[BACKGROUND_PART-1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -backgroundY + cameraY + BACKGROUND_HEIGHT, instance.background[BACKGROUND_PART-1].transform.position.z);
		}
		
		float middleground_1_Y = cameraY * (-1 + MIDDLEGROUND_1_SCROLL_RATIO);
		instance.middleground_1.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -middleground_1_Y + MIDDLEGROUND_1_OFFSET, instance.middleground_1.transform.position.z);
		
		float middleground_2_Y = cameraY * (-1 + MIDDLEGROUND_2_SCROLL_RATIO);
		instance.middleground_2.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -middleground_2_Y + MIDDLEGROUND_2_OFFSET, instance.middleground_2.transform.position.z);
		
		float foregroundY = cameraY * (-1 + FOREGROUND_SCROLL_RATIO);
		instance.foreground.transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -foregroundY, instance.foreground.transform.position.z);
		
		for (int i=0; i<instance.cloudY.Length; i++) {
			instance.cloud[i].transform.position = new Vector3 (instance.cloud[i].transform.position.x, instance.cloudY[i], instance.cloud[i].transform.position.z);
		}
	}
	
	private void DeployCloud () {
		for (int i=0; i<cloudY.Length; i++) {
			float cloudX = Random.Range (0, SCR_Gameplay.SCREEN_W);
			cloudY[i] = SCR_Gameplay.SCREEN_H + CLOUD_SIZE + CLOUD_SIZE_SPACING * i;
			cloud[i].transform.position = new Vector3 (cloudX, cloudY[i], cloud[i].transform.position.z);
		}
	}
	
	private void Update () {
		for (int i=0; i<cloudY.Length; i++) {
			if (cloudY[i] < SCR_Gameplay.instance.cameraHeight - CLOUD_SIZE) {
				cloudY[i] = SCR_Gameplay.instance.cameraHeight + SCR_Gameplay.SCREEN_H + CLOUD_SIZE;
				
				float cloudX = Random.Range (0, SCR_Gameplay.SCREEN_W);
				cloud[i].transform.position = new Vector3 (cloudX, cloudY[i], cloud[i].transform.position.z);
			}
		}
	}
	
}
