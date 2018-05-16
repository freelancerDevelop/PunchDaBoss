using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum TutorialStep {
	NONE = 0,
	GRAB,
	THROW,
	AIM,
	PUNCH,
	CONTINUE,
	HIT,
	MISS,
	FINISH
}

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
	public GameObject	pnlResult;
	public GameObject	txtPunchNumber;
	public GameObject	txtHeightNumber;
	public GameObject	txtMoneyNumber;
	public GameObject	txtCurrentHeight;
	public GameObject	pnlTutorial;
	public GameObject	txtTutorial;
	public GameObject	imgCooldown;
	
	// Object
	[System.NonSerialized] public GameObject 	player			= null;
	[System.NonSerialized] public GameObject 	boss			= null;
	
	// Game
	[System.NonSerialized] public GameState 	gameState		= GameState.TALKING;
	[System.NonSerialized] public float 		cameraHeight	= 0.0f;
	[System.NonSerialized] public float 		cameraTarget	= 0.0f;
	[System.NonSerialized] public int			maxBossY		= 0;
	[System.NonSerialized] public int			punchNumber		= 0;
	
	[System.NonSerialized] public TutorialStep	tutorialStep	= TutorialStep.NONE;
	
	
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
		
		
		pnlResult.SetActive (false);
		pnlTutorial.SetActive (false);
		imgCooldown.SetActive (false);
		txtPunchName.GetComponent<Text>().text = SCR_Profile.GetPunchName();
	
		TriggerTutorial (TutorialStep.GRAB);
	}
	
	// Update
	private void Update () {
		// Don't do anything if menu state is not the first state
		if (SCR_Menu.menuLoaded == false) {
			return;
		}
		
		float dt = Time.deltaTime;
		
		if (Input.GetMouseButton(0)) {
			float touchX = Input.mousePosition.x * TOUCH_SCALE;
			float touchY = Input.mousePosition.y * TOUCH_SCALE;
			
			if (gameState == GameState.PUNCHING) {
				if (SCR_Profile.showTutorial == 0 || tutorialStep == TutorialStep.AIM || tutorialStep == TutorialStep.PUNCH) {
					player.GetComponent<SCR_Player>().Aim (touchX, touchY + cameraHeight);
					TriggerTutorial (TutorialStep.PUNCH);
				}
					
				if (player.GetComponent<SCR_Player>().GetCoolDown() >= 0.99f) {
					imgCooldown.SetActive (false);
				}
				else {
					imgCooldown.SetActive (true);
					imgCooldown.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = player.GetComponent<SCR_Player>().GetCoolDown();
					imgCooldown.GetComponent<RectTransform>().anchoredPosition = new Vector2(touchX, touchY);
				}
			}
			else if (gameState == GameState.BOSS_RUNNING) {
				if (boss.GetComponent<SCR_Boss>().IsRunning()) {
					SceneManager.LoadScene("GSMenu/SCN_Menu");
				}
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
				
				TriggerTutorial (TutorialStep.CONTINUE);
			}
			
			imgCooldown.SetActive (false);
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
			
			player.GetComponent<SCR_Player>().TurnOffCrossHair();
			
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
		
		SCR_Background.SetCameraY (cameraHeight);
		Camera.main.transform.position = new Vector3 (SCREEN_W * 0.5f, SCREEN_H * 0.5f + cameraHeight, Camera.main.transform.position.z);
		
		if (boss.GetComponent<SCR_Boss>().y * 0.01f - 3 > maxBossY) {
			maxBossY = (int)(boss.GetComponent<SCR_Boss>().y * 0.01f - 3);
			txtCurrentHeight.GetComponent<Text>().text = maxBossY.ToString();
		}
		
		float cooldown = player.GetComponent<SCR_Player>().cooldown;
		pnlCooldown.GetComponent<RectTransform>().sizeDelta = new Vector2(512, 512 * cooldown / SCR_Profile.GetPunchCooldown());
		
		if (SCR_Profile.showTutorial == 1) {
			if (tutorialStep != TutorialStep.AIM && tutorialStep != TutorialStep.PUNCH && Time.timeScale < 1) {
				if (tutorialStep == TutorialStep.MISS) {
					Time.timeScale += dt * 3.0f;
				}
				else {
					Time.timeScale += dt * 0.5f;
				}
				if (Time.timeScale > 1) Time.timeScale = 1;
			}
		}
		else {
			if (Time.timeScale < 1) {
				Time.timeScale += dt * 10.0f;
				if (Time.timeScale > 1) Time.timeScale = 1;
			}
		}
	}
	
	
	public void Lose () {
		pnlResult.SetActive (true);
		txtPunchNumber.GetComponent<Text>().text = punchNumber.ToString();
		txtHeightNumber.GetComponent<Text>().text = maxBossY.ToString();
		
		int money = (int)(0.1f * maxBossY);
		txtMoneyNumber.GetComponent<Text>().text = money.ToString();
		SCR_Profile.AddMoney (money);
	}
	
	
	public void TriggerTutorial (TutorialStep step, bool force = false) {
		if (SCR_Profile.showTutorial == 1) {
			if (step == tutorialStep + 1 || force == true) {
				tutorialStep = step;
				pnlTutorial.SetActive (true);
					
				if (step == TutorialStep.GRAB) {
					txtTutorial.GetComponent<Text>().text = "Tap anywhere to grab your boss!";
				}
				else if (step == TutorialStep.THROW) {
					txtTutorial.GetComponent<Text>().text = "Now, tap anywhere to throw him upward.";
				}
				else if (step == TutorialStep.AIM) {
					Time.timeScale = 0.05f;
					txtTutorial.GetComponent<Text>().text = "Tap and hold your finger to aim your punch. Aim slightly ahead of his trajectory.";
				}
				else if (step == TutorialStep.PUNCH) {
					Time.timeScale = 0.05f;
					txtTutorial.GetComponent<Text>().text = "Remember, always aim slightly ahead of his trajectory. Release your finger to rush up and punch him.";
				}
				else if (step == TutorialStep.CONTINUE) {
					txtTutorial.GetComponent<Text>().text = "The cooldown is on the top left corner of the screen.";
				}
				else if (step == TutorialStep.HIT) {
					txtTutorial.GetComponent<Text>().text = "Nice hit! Now, keep punching your boss as high as possible.";
				}
				else if (step == TutorialStep.MISS) {
					txtTutorial.GetComponent<Text>().text = "I can't believe you miss that. Let's try again.";
				}
				else if (step == TutorialStep.FINISH) {
					pnlTutorial.SetActive (false);
					SCR_Profile.showTutorial = 0;
					SCR_Profile.SaveProfile();
				}
			}
		}
	}
	
	
	public void ShakeCamera (float magnitude, float duration) {
		
	}
}
