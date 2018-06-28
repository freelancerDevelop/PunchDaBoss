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
	FLY_UP,
	CONTINUE,
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
	// Screen
	public static float SCREEN_RATIO 			= 0;
	public static float SCREEN_W 				= 0;
	public static float SCREEN_H 				= 0;
	public static float SCREEN_SCALE			= 0;
	public static float TOUCH_SCALE				= 0;
	
	public static float GRAVITY					= 3500.0f;
	public static float CAMERA_OFFSET_Y			= 700.0f; // Distance from top of the screen to the boss
	public static float CAMERA_SPEED_MULTIPLIER = 15.0f;
	public static float CAMERA_ENDING_Y			= 100.0f;
	public static float CAMERA_SHAKE_AMOUNT		= 4.0f;
	
	public static float COMBO_TIME				= 1.5f;
	
	public static float FURNITURE_Y				= 1470.0f;
	public static float FRAGMENT_Y				= 1308.0f;
	public static float LAPTOP_Y				= 1570.0f;
	
	public static float PUNCH_TEXT_OFFSET_Y		= 200.0f;
	
	public static float OBJECT_SPAWN_TIME		= 15.0f;
	
	public static int	MONEY_FOR_HIGHLIGHT		= 5;
	public static float	TUTORIAL_FADE_SPEED		= 0.3f;
	public static float	TUTORIAL_HOLD_DURATION	= 0.5f;
	
	
	// Prefab
	public GameObject 	PFB_Player				= null;
	public GameObject[]	PFB_Boss				= null;
	public GameObject[]	PFB_Security			= null;
	public GameObject[]	PFB_FlyingObject		= null;
	public GameObject[]	PFB_Furniture			= null;
	
	public GameObject 	PFB_Destruction			= null;
	public GameObject 	PFB_Ricochet			= null;
	public GameObject[]	PFB_Combo				= null;
	
	
	
	// Instance
	public static SCR_Gameplay instance 		= null;
	
	
	// On-screen object
	public GameObject	cvsMain;
	public GameObject	pnlResult;
	public GameObject 	txtMoney;
	public GameObject 	txtMoneyAdd;
	public GameObject	txtPunchNumber;
	public GameObject	txtHeightNumber;
	public GameObject	txtBestNumber;
	public GameObject	txtMoneyNumber;
	public GameObject	txtCurrentHeight;
	public GameObject	imgHighScore;
	public GameObject	imgClockContent;
	public GameObject[]	imgCombo;
	public GameObject	txtTapToBack;
	public GameObject	txtTutorial;
	public GameObject	pnlFlashWhite;
	
	
	// Object
	[System.NonSerialized] public GameObject 	player			= null;
	[System.NonSerialized] public GameObject 	boss			= null;
	[System.NonSerialized] public GameObject 	security		= null;
	[System.NonSerialized] public GameObject 	flyingObject	= null;
	
	// Game
	[System.NonSerialized] public GameState 	gameState		= GameState.TALKING;
	[System.NonSerialized] public float 		cameraHeight	= 0.0f;
	[System.NonSerialized] public float 		cameraTarget	= 0.0f;
	[System.NonSerialized] public float 		cameraShakeTime	= 0.0f;
	[System.NonSerialized] public int			maxBossY		= 0;
	[System.NonSerialized] public int			punchNumber		= 0;
	[System.NonSerialized] public float			punchVolume		= 0;
	[System.NonSerialized] public bool			breakFurniture	= false;
	
	[System.NonSerialized] public int			comboCount		= 0;
	[System.NonSerialized] public float			comboTime		= 0;
	
	[System.NonSerialized] public float			objectCounter	= 0;
	[System.NonSerialized] public float			internalMoney	= 0;
	
	[System.NonSerialized] public float			flashWhiteAlpha	= 0;
	
	[System.NonSerialized] public TutorialStep	tutorialStep	= TutorialStep.NONE;
	[System.NonSerialized] public float			tutorialAlpha	= 0;
	[System.NonSerialized] public float			tutorialCounter	= 0;
	
	
	private Vector2		txtMoneyAddOriginalPosition;
	private Vector2		txtMoneyAddOriginalAnchorMin;
	private Vector2		txtMoneyAddOriginalAnchorMax;
	private TextAnchor	txtMoneyAddOriginalAlignment;
	private int			txtMoneyAddOriginalFontSize;
	
	private int			totalReward;
	
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
		boss 		= Instantiate (PFB_Boss[SCR_Profile.bossSelecting]);
		security	= Instantiate (PFB_Security[SCR_Profile.bossSelecting]);
		
		pnlResult.SetActive (false);
		txtTapToBack.SetActive (false);
		txtTutorial.SetActive (false);

		SCR_Pool.Flush ();
		TriggerTutorial (TutorialStep.GRAB);
		SCR_LightBar.deltaCamera = 0;
		
		SCR_UnityAnalytics.StartGame();
		
		internalMoney = SCR_Profile.money;
		txtMoney.GetComponent<Text>().text = internalMoney.ToString();
		
		txtMoneyAddOriginalPosition = txtMoneyAdd.GetComponent<RectTransform>().anchoredPosition;
		txtMoneyAddOriginalAnchorMin = txtMoneyAdd.GetComponent<RectTransform>().anchorMin;
		txtMoneyAddOriginalAnchorMax = txtMoneyAdd.GetComponent<RectTransform>().anchorMax;
		txtMoneyAddOriginalAlignment = txtMoneyAdd.GetComponent<Text>().alignment;
		txtMoneyAddOriginalFontSize = txtMoneyAdd.GetComponent<Text>().fontSize;
		
		totalReward = 0;
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
			
			if (gameState == GameState.TALKING) {
				gameState = GameState.GRABBING;
				player.GetComponent<SCR_Player>().GoGrabTheBoss();
			}
			else if (gameState == GameState.PUNCHING) {
				if (SCR_Profile.showTutorial == 0 
				|| tutorialStep == TutorialStep.AIM  
				|| tutorialStep == TutorialStep.PUNCH 
				|| tutorialStep == TutorialStep.CONTINUE) {
					player.GetComponent<SCR_Player>().Aim (touchX, touchY + cameraHeight);
				}
				
				if (SCR_Profile.showTutorial == 1 && tutorialStep == TutorialStep.AIM) {
					tutorialCounter += dt;
					if (tutorialCounter > TUTORIAL_HOLD_DURATION) {
						TriggerTutorial (TutorialStep.PUNCH);
					}
				}
			}
		}
		else {
			if (gameState == GameState.GRABBING) {
				if (player.GetComponent<SCR_Player>().IsGrabbingTheBoss()) {
					gameState = GameState.PUNCHING;
					player.GetComponent<SCR_Player>().ThrowTheBoss();
				}
			}
			else if (gameState == GameState.PUNCHING) {
				player.GetComponent<SCR_Player>().PerformPunch ();
			}
			tutorialCounter = 0;
		}
		
		if (Input.GetMouseButtonUp(0) && gameState == GameState.BOSS_RUNNING) {
			if (boss.GetComponent<SCR_Boss>().IsRunning()) {
				SceneManager.LoadScene("GSMenu/SCN_Menu");
				SCR_Audio.PlayClickSound();
			}
		}
		
		if (gameState == GameState.PUNCHING) {
			cameraTarget = boss.GetComponent<SCR_Boss>().y - SCREEN_H + CAMERA_OFFSET_Y;
			if (cameraTarget > cameraHeight) {
				float deltaCamera = (cameraTarget - cameraHeight) * dt * CAMERA_SPEED_MULTIPLIER;
				SCR_LightBar.deltaCamera = deltaCamera;
				
				cameraHeight += deltaCamera;
				
				player.GetComponent<SCR_Player>().AddDeltaCameraToPlayer (deltaCamera);
				security.GetComponent<SCR_Security>().AddDeltaCameraToSecurity (deltaCamera);
				
				if (flyingObject != null) {
					flyingObject.GetComponent<SCR_FlyingObject>().AddDeltaCameraToObject (deltaCamera);
				}
			}
			
			if (!breakFurniture) {
				if (boss.GetComponent<SCR_Boss>().y > FURNITURE_Y - SCR_Boss.BOSS_SIZE) {
					SCR_Background.BreakFurniture (boss.GetComponent<SCR_Boss>().x, boss.GetComponent<SCR_Boss>().y, boss.GetComponent<SCR_Boss>().speedY);
					breakFurniture = true;
				}
			}
			
			if (flyingObject == null) {
				objectCounter += dt;
				if (objectCounter >= OBJECT_SPAWN_TIME) {
					objectCounter = 0;
					
					if (cameraHeight > 300000) {
						flyingObject = SCR_Pool.GetFreeObject (PFB_FlyingObject[2]);	
					}
					else if (cameraHeight > 150000) {
						flyingObject = SCR_Pool.GetFreeObject (PFB_FlyingObject[1]);	
					}
					else {
						flyingObject = SCR_Pool.GetFreeObject (PFB_FlyingObject[0]);	
					}
					
					//int choose = Random.Range (0, PFB_FlyingObject.Length);
					//flyingObject = SCR_Pool.GetFreeObject (PFB_FlyingObject[choose]);
					
					float x = Random.Range (-(SCREEN_W - SCR_FlyingObject.OBJECT_SIZE) * 0.5f, (SCREEN_W - SCR_FlyingObject.OBJECT_SIZE) * 0.5f);
					float y = cameraHeight + Random.Range (1.0f, 1.5f) * SCREEN_H;
					flyingObject.GetComponent<SCR_FlyingObject>().Spawn (x, y);
				}
			}
			else {
				if (flyingObject.GetComponent<SCR_FlyingObject>().broken == false) {
					if (SCR_Helper.DistanceBetweenTwoPoint(flyingObject.GetComponent<SCR_FlyingObject>().x, flyingObject.GetComponent<SCR_FlyingObject>().y, boss.GetComponent<SCR_Boss>().x, boss.GetComponent<SCR_Boss>().y) < (SCR_FlyingObject.OBJECT_SIZE + SCR_Boss.BOSS_SIZE) * 0.5f) {
						float bossAngle = SCR_Helper.AngleBetweenTwoPoint(flyingObject.GetComponent<SCR_FlyingObject>().x, flyingObject.GetComponent<SCR_FlyingObject>().y, boss.GetComponent<SCR_Boss>().x, boss.GetComponent<SCR_Boss>().y);
						boss.GetComponent<SCR_Boss>().CrashIntoObject (bossAngle);
						flyingObject.GetComponent<SCR_FlyingObject>().Break();
					}
					
					else if (SCR_Helper.DistanceBetweenTwoPoint(flyingObject.GetComponent<SCR_FlyingObject>().x, flyingObject.GetComponent<SCR_FlyingObject>().y, player.GetComponent<SCR_Player>().x, player.GetComponent<SCR_Player>().y) < (SCR_FlyingObject.OBJECT_SIZE + SCR_Player.PLAYER_SIZE) * 0.5f) {
						flyingObject.GetComponent<SCR_FlyingObject>().Break();
					}
				}
			}
		}
		else if (gameState == GameState.BOSS_FALLING) {
			float deltaCamera = -cameraHeight * dt * CAMERA_SPEED_MULTIPLIER;
			if (deltaCamera > -1) deltaCamera = -1;
			SCR_LightBar.deltaCamera = deltaCamera;
			
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
		
		if (cameraShakeTime > 0) {
			cameraShakeTime -= dt;
			Camera.main.transform.position = new Vector3 (SCREEN_W * 0.5f + Random.Range(-CAMERA_SHAKE_AMOUNT, CAMERA_SHAKE_AMOUNT), SCREEN_H * 0.5f + cameraHeight + Random.Range(-CAMERA_SHAKE_AMOUNT, CAMERA_SHAKE_AMOUNT), Camera.main.transform.position.z);
		}
		else {
			Camera.main.transform.position = new Vector3 (SCREEN_W * 0.5f, SCREEN_H * 0.5f + cameraHeight, Camera.main.transform.position.z);
		}
		
		if (comboTime > 0) {
			comboTime -= dt;
			if (comboTime <= 0) {
				comboTime = 0;
				comboCount = 0;
				for (int i=1; i<imgCombo.Length; i++) {
					imgCombo[i].SetActive (false);
				}
				imgCombo[0].SetActive (true);
			}
		}
		imgClockContent.GetComponent<Image>().fillAmount = comboTime / COMBO_TIME;
		
		if (boss.GetComponent<SCR_Boss>().y * 0.01f - 3 > maxBossY) {
			maxBossY = (int)(boss.GetComponent<SCR_Boss>().y * 0.01f - 3);
			txtCurrentHeight.GetComponent<Text>().text = maxBossY.ToString();
		}
		
		if (SCR_Profile.showTutorial == 1) {
			if (tutorialStep != TutorialStep.AIM && Time.timeScale < 1) {
				Time.timeScale += dt * 2.0f;
				if (Time.timeScale > 1) Time.timeScale = 1;
			}
		}
		else {
			if (Time.timeScale < 1) {
				Time.timeScale += dt * 3.0f;
				if (Time.timeScale > 1) Time.timeScale = 1;
			}
		}
		
		if (flashWhiteAlpha > 0) {
			flashWhiteAlpha -= dt * 2.0f;
			if (flashWhiteAlpha < 0) flashWhiteAlpha = 0;
			
			Color color = pnlFlashWhite.GetComponent<Image>().color;
			color.a = flashWhiteAlpha;
			pnlFlashWhite.GetComponent<Image>().color = color;
		}
		
		if (SCR_Profile.showTutorial == 0) {
			if (tutorialAlpha > 0) {
				tutorialAlpha -= TUTORIAL_FADE_SPEED * dt;
				if (tutorialAlpha < 0) tutorialAlpha = 0;
				Color color = txtTutorial.GetComponent<Text>().color;
				color.a = tutorialAlpha;
				txtTutorial.GetComponent<Text>().color = color;
			}
		}
		
		
		if (SCR_Profile.money - internalMoney > 1000) {
			internalMoney += 57;
		}
		else if (SCR_Profile.money - internalMoney > 100) {
			internalMoney += 13;
		}
		else if (SCR_Profile.money - internalMoney > 10) {
			internalMoney += 7;
		}
		else if (SCR_Profile.money - internalMoney > 0) {
			internalMoney += 1;
		}
		
		if (SCR_Profile.money - internalMoney < -1000) {
			internalMoney -= 57;
		}
		else if (SCR_Profile.money - internalMoney < -100) {
			internalMoney -= 13;
		}
		else if (SCR_Profile.money - internalMoney < -10) {
			internalMoney -= 7;
		}
		else if (SCR_Profile.money - internalMoney < 0) {
			internalMoney -= 1;
		}
		
		txtMoney.GetComponent<Text>().text = internalMoney.ToString();
	}
	
	
	
	
	public void PunchSuccess (float x, float y) {
		comboTime = COMBO_TIME;
		comboCount ++;
		if (comboCount == 5) {
			security.GetComponent<SCR_Security>().PerformPunch();
		}
		
		for (int i=0; i<imgCombo.Length; i++) {
			imgCombo[i].SetActive (false);
		}
		imgCombo[comboCount].SetActive (true);
		
		punchNumber ++;
		if (SCR_Profile.showTutorial == 1) {
			txtTutorial.GetComponent<Text>().text = "Keep on punching (" + punchNumber.ToString() + "/3)";
			if (punchNumber == 3) {
				TriggerTutorial (TutorialStep.FINISH);
			}
		}
		
		if (comboCount >= 1) {
			GameObject text = SCR_Pool.GetFreeObject (PFB_Combo[comboCount-1]);
			text.transform.SetParent (cvsMain.transform);
			text.GetComponent<SCR_ComboText>().Spawn (x, y);
		}
	}
	
	public void SecurityPunchSuccess () {
		comboCount = 0;
		
		for (int i=1; i<imgCombo.Length; i++) {
			imgCombo[i].SetActive (false);
		}
		imgCombo[0].SetActive (true);
	}
	
	
	public void Lose () {
		pnlResult.SetActive (true);
		txtTapToBack.SetActive (true);
		
		if (maxBossY > SCR_Profile.highScore)
			imgHighScore.SetActive (true);
		else
			imgHighScore.SetActive (false);
		
		SCR_Profile.ReportScore (maxBossY);
		txtPunchNumber.GetComponent<Text>().text = punchNumber.ToString();
		txtHeightNumber.GetComponent<Text>().text = maxBossY.ToString();
		txtBestNumber.GetComponent<Text>().text = SCR_Profile.highScore.ToString();
		
		int money = (int)(maxBossY);
		AddMoney (money);
		
		txtMoneyNumber.GetComponent<Text>().text = "$" + totalReward.ToString();
		
		SCR_UnityAnalytics.FinishGame(maxBossY);
	}
	
	
	public void TriggerTutorial (TutorialStep step, bool force = false) {
		if (SCR_Profile.showTutorial == 1) {
			if (step == tutorialStep + 1 || force == true) {
				tutorialStep = step;

					
				if (step == TutorialStep.GRAB) {
					tutorialAlpha = 1;
					txtTutorial.SetActive (true);
					txtTutorial.GetComponent<Text>().text = "Tap anywhere to start";
					SCR_UnityAnalytics.StartTutorial();
				}
				else if (step == TutorialStep.THROW) {
					txtTutorial.SetActive (false);
				}
				else if (step == TutorialStep.AIM) {
					txtTutorial.SetActive (true);
					txtTutorial.GetComponent<Text>().text = "Tap and hold to aim";
				}
				else if (step == TutorialStep.PUNCH) {
					txtTutorial.SetActive (true);
					txtTutorial.GetComponent<Text>().text = "Release to start punching";
				}
				else if (step == TutorialStep.FLY_UP) {
					txtTutorial.GetComponent<Text>().text = "Keep on punching (0/3)";
					Time.timeScale = 0.05f;
				}
				else if (step == TutorialStep.CONTINUE) {
					
				}
				else if (step == TutorialStep.FINISH) {
					txtTutorial.GetComponent<Text>().text = "Well done!";
					SCR_Profile.showTutorial = 0;
					SCR_Profile.SaveProfile();
					
					SCR_UnityAnalytics.FinishTutorial();
				}
			}
		}
	}
	
	
	public void AddMoney (int money) {
		txtMoneyAdd.GetComponent<SCR_MoneyAdd>().SetText (money.ToString());
		txtMoneyAdd.GetComponent<RectTransform>().anchorMin = txtMoneyAddOriginalAnchorMin;
		txtMoneyAdd.GetComponent<RectTransform>().anchorMax = txtMoneyAddOriginalAnchorMax;
		txtMoneyAdd.GetComponent<RectTransform>().anchoredPosition = txtMoneyAddOriginalPosition;
		txtMoneyAdd.GetComponent<Text>().alignment = txtMoneyAddOriginalAlignment;
		txtMoneyAdd.GetComponent<Text>().fontSize = txtMoneyAddOriginalFontSize;
		SCR_Profile.AddMoney (money);
		
		totalReward += money;
	}
	
	public void AddMoneyAtPosition (int money, float x, float y) {
		txtMoneyAdd.GetComponent<SCR_MoneyAdd>().Spawn (money.ToString(), x, y + PUNCH_TEXT_OFFSET_Y);
		txtMoneyAdd.GetComponent<RectTransform>().anchorMin = Vector2.zero;
		txtMoneyAdd.GetComponent<RectTransform>().anchorMax = Vector2.zero;
		txtMoneyAdd.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
		txtMoneyAdd.GetComponent<Text>().fontSize = 80;
		SCR_Profile.AddMoney (money);
		
		totalReward += money;
	}
	
	public void ShowDestruction (float x, float y) {
		GameObject text = SCR_Pool.GetFreeObject (PFB_Destruction);
		text.transform.SetParent (cvsMain.transform);
		text.GetComponent<SCR_ComboText>().Spawn (x, y + PUNCH_TEXT_OFFSET_Y);
	}
	public void ShowRicochet (float x, float y) {
		GameObject text = SCR_Pool.GetFreeObject (PFB_Ricochet);
		text.transform.SetParent (cvsMain.transform);
		text.GetComponent<SCR_ComboText>().Spawn (x, y + PUNCH_TEXT_OFFSET_Y);
		security.GetComponent<SCR_Security>().PerformPunch();
	}
	
	public void ShakeCamera (float duration) {
		cameraShakeTime = duration;
	}
	
	public void FlashWhite () {
		flashWhiteAlpha = 0.9f;
	}
}
