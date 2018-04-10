﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Gameplay : MonoBehaviour {
	// Screen
	public static float SCREEN_RATIO 		= 0;
	public static float SCREEN_W 			= 0;
	public static float SCREEN_H 			= 0;
	public static float SCREEN_SCALE		= 0;
	public static float TOUCH_SCALE			= 0;
	
	// Instance
	public static SCR_Gameplay instance 	= null;
	
	
	// Init
	private void Awake () {
		// Don't do anything if menu state is not the first state
		// Load the menu instead
		if (SCR_Menu.menuLoaded == false) {
			SceneManager.LoadScene("GSMenu/SCN_Menu");
			return;
		}
		
		// Assign instance
		instance = this;
		
		// Calculate screen resolution
		SCREEN_RATIO = Screen.height * 1.0f / Screen.width;
		SCREEN_W = 1080;
		SCREEN_H = 1080 * SCREEN_RATIO;
		SCREEN_SCALE = SCREEN_W / 10.8f;
		TOUCH_SCALE = SCREEN_W / Screen.width;
		
		// Set camera
		Camera.main.orthographicSize = SCREEN_H * 0.5f;
		Camera.main.transform.position = new Vector3 (SCREEN_W * 0.5f, SCREEN_H * 0.5f, Camera.main.transform.position.z);
	}
	
	// Update
	private void Update () {
		// Don't do anything if menu state is not the first state
		if (SCR_Menu.menuLoaded == false) {
			return;
		}
		
		float dt = Time.deltaTime;
		
		
	}
}
