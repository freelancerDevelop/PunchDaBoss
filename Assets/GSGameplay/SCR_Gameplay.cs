using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Gameplay : MonoBehaviour {
	// PREFAB
	
	
	// SCREEN
	public static float SCREEN_RATIO 		= 0;
	public static float SCREEN_W 			= 0;
	public static float SCREEN_H 			= 0;
	public static float SCREEN_SCALE		= 0;
	
	public static float TOUCH_SCALE			= 0;
	
	public static float BACKGROUND_SQUARE	= 120;
	
	// GAME
	public const  int 	MAP_SIZE			= 15;
	public const  int	BRICK_ROW_NUMBER	= 4;
	public const  float	SCORE_TO_TIME		= 0.2f;
	
	// INSTANCE
	public static SCR_Gameplay instance 	= null;
	public float  score						= 0;
	
	// INIT
	private void Start () {
		// Don't do anything if menu state is not the first state
		// Load the menu instead
		if (SCR_Menu.menuLoaded == false) {
			SceneManager.LoadScene("GS_Menu/SCN_Menu");
			return;
		}
		
		// Assign instance
		instance = this;
		
		// Calculate screen resolution
		SCREEN_RATIO = Screen.height * 1.0f / Screen.width;
		SCREEN_W = 1080;
		SCREEN_H = 1080 * SCREEN_RATIO;
		SCREEN_SCALE = SCREEN_W / 10.8f;
		Camera.main.orthographicSize = SCREEN_H * 0.5f;
		Camera.main.transform.position = new Vector3 (SCREEN_W * 0.5f, SCREEN_H * 0.5f, Camera.main.transform.position.z);
		
		TOUCH_SCALE = SCREEN_W / Screen.width;
		
		// OK, create the level
		NewGame ();
	}
	
	private void NewGame () {
		
	}
	
	
	// UPDATE
	private void Update () {
		// Don't do anything if menu state is not the first state
		if (SCR_Menu.menuLoaded == false) {
			return;
		}
		
		float dt = Time.deltaTime;
		
		
	}
	
	private void FixedUpdate () {
		
	}
}
