﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public enum GameState {
	TALKING = 0,
	GRABBING,
	PUNCHING,
	BOSS_FALLING,
	BOSS_RUNNING
}

public class SCR_Gameplay : MonoBehaviour {
	// Prefab
	public GameObject PFB_Player				= null;
	public GameObject PFB_Boss					= null;
	
	// Screen
	public static float SCREEN_RATIO 			= 0;
	public static float SCREEN_W 				= 0;
	public static float SCREEN_H 				= 0;
	public static float SCREEN_SCALE			= 0;
	public static float TOUCH_SCALE				= 0;
	
	public static float GRAVITY					= 1500.0f;
	public static float CAMERA_OFFSET_Y			= 400.0f; // Distance from top of the screen to the boss
	public static float CAMERA_SPEED_MULTIPLIER = 10.0f;
	
	public static float CAMERA_ENDING_Y			= 100.0f;
	
	// Instance
	public static SCR_Gameplay instance 		= null;
	
	// On-screen object
	public GameObject	pnlCooldown;
	public GameObject	txtPunchName;
	
	// Object
	[System.NonSerialized] public GameObject 	player			= null;
	[System.NonSerialized] public GameObject 	boss			= null;
	[System.NonSerialized] public float 		cameraHeight	= 0.0f;
	[System.NonSerialized] public float 		cameraTarget	= 0.0f;
	
	// Game
	[System.NonSerialized] public GameState 	gameState		= GameState.TALKING;
	
	
	
	
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
	
	// Start game
	private void Start () {
		gameState	= GameState.TALKING;
		
		player		= Instantiate (PFB_Player);
		boss 		= Instantiate (PFB_Boss);
		
		txtPunchName.GetComponent<Text>().text = SCR_Profile.GetPunchName();
	}
	
	// Update
	private void Update () {
		// Don't do anything if menu state is not the first state
		if (SCR_Menu.menuLoaded == false) {
			return;
		}
		
		float dt = Time.deltaTime;
		//cameraHeight += dt * 1000;
		
		if (Input.GetMouseButton(0)) {
			if (gameState == GameState.PUNCHING) {
				float touchX = Input.mousePosition.x * TOUCH_SCALE;
				float touchY = Input.mousePosition.y * TOUCH_SCALE;
				player.GetComponent<SCR_Player>().Aim (touchX, touchY + cameraHeight);
			}
		}
		else if (Input.GetMouseButtonUp(0)) {
			if (gameState == GameState.TALKING) {
				gameState = GameState.GRABBING;
				player.GetComponent<SCR_Player>().GoGrabTheBoss();
			}
			else if (gameState == GameState.GRABBING) {
				if (player.GetComponent<SCR_Player>().IsGrabbingTheBoss()) {
					gameState = GameState.PUNCHING;
					player.GetComponent<SCR_Player>().ThrowTheBoss();
				}
			}
			else if (gameState == GameState.PUNCHING) {
				float touchX = Input.mousePosition.x * TOUCH_SCALE;
				float touchY = Input.mousePosition.y * TOUCH_SCALE;
				player.GetComponent<SCR_Player>().PerformPunch (touchX, touchY + cameraHeight);
			}
		}
		
		if (gameState == GameState.PUNCHING) {
			cameraTarget = boss.GetComponent<SCR_Boss>().y - SCREEN_H + CAMERA_OFFSET_Y;
			if (cameraTarget > cameraHeight) {
				float deltaCamera = (cameraTarget - cameraHeight) * dt * CAMERA_SPEED_MULTIPLIER;
				cameraHeight += deltaCamera;
				
				player.GetComponent<SCR_Player>().AddDeltaCameraToTarget (deltaCamera);
			}
		}
		else if (gameState == GameState.BOSS_FALLING) {
			float deltaCamera = -cameraHeight * dt * CAMERA_SPEED_MULTIPLIER;
			if (deltaCamera > -1) deltaCamera = -1;
			cameraHeight += deltaCamera;
			
			if (cameraHeight < CAMERA_ENDING_Y) {
				gameState = GameState.BOSS_RUNNING;
				player.GetComponent<SCR_Player>().ReAdjustY();
				boss.GetComponent<SCR_Boss>().ReAdjustY();
			}
		}
		else if (gameState == GameState.BOSS_RUNNING) {
			float deltaCamera = -cameraHeight * dt * CAMERA_SPEED_MULTIPLIER;
			if (deltaCamera > -1) deltaCamera = -1;
			cameraHeight += deltaCamera;
			if (cameraHeight < 0) cameraHeight = 0;
		}
		
		float cooldown = player.GetComponent<SCR_Player>().cooldown;
		pnlCooldown.GetComponent<RectTransform>().sizeDelta = new Vector2(512, 512 * cooldown / SCR_Profile.GetPunchCooldown());
	}
	
	
	public void Lose () {
		SceneManager.LoadScene("GSMenu/SCN_Menu");
	}
}
