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
	
	public const  float GRAVITY					= 4000.0f;
	public const  float CAMERA_OFFSET_Y			= 750.0f; // Distance from top of the screen to the boss
	public const  float CAMERA_SPEED_MULTIPLIER = 15.0f;
	public const  float CAMERA_ENDING_Y			= 100.0f;
	public const  float CAMERA_SHAKE_AMOUNT		= 4.0f;
	
	public const  float COMBO_TIME				= 1.5f;
	
	public const  float FURNITURE_Y				= 1470.0f;
	public const  float FRAGMENT_Y				= 1308.0f;
	public const  float LAPTOP_Y				= 1570.0f;
	
	public const  float PUNCH_TEXT_OFFSET_Y		= 200.0f;
	
	public const  float OBJECT_SPAWN_TIME_MIN	= 6.0f;
	public const  float OBJECT_SPAWN_TIME_MAX	= 10.0f;
	public const  float OBJECT_DANGER_BEFORE	= 1.0f;
	public const  int 	OBJECT_DANGER_TIMES		= 2;
	
	public const  float POWER_UP_ENLARGE_SPAWN_TIME_MIN		= 7.0f;
	public const  float POWER_UP_ENLARGE_SPAWN_TIME_MAX		= 12.0f;
	public const  float POWER_UP_SECURITY_SPAWN_TIME_MIN	= 7.0f;
	public const  float POWER_UP_SECURITY_SPAWN_TIME_MAX	= 12.0f;
	public const  float POWER_UP_SECURITY_DURATION			= 7.0f;
	
	public const  int	MONEY_FOR_HIGHLIGHT		= 5;
	public const  float	TUTORIAL_FADE_SPEED		= 0.3f;
	public const  float	TUTORIAL_HOLD_DURATION	= 0.5f;
	
	
	// Prefab
	public GameObject 	PFB_Player				= null;
	public GameObject[]	PFB_Boss				= null;
	public GameObject[]	PFB_Security			= null;
	public GameObject[]	PFB_FlyingObject		= null;
	public GameObject[] PFB_PowerUp				= null;
	public GameObject[]	PFB_Furniture			= null;
	
	public GameObject 	PFB_Destruction			= null;
	public GameObject 	PFB_Ricochet			= null;
	public GameObject[]	PFB_Combo				= null;
	public GameObject	PFB_ComboText			= null;
	
	
	
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
	//public GameObject	imgClockContent;
	//public GameObject[]	imgCombo;
	public GameObject	btnReplay;
	public GameObject	btnMainMenu;
	public GameObject	imgNotice;
	public GameObject	txtTutorial;
	public GameObject	pnlFlashWhite;
	
	public Image		imgSecurityProgressBG;
	public Image		imgSecurityProgressFG;
	
	public Image		imgDanger;
	
	public Text			txtResultTitle;
	
	// Object
	[System.NonSerialized] public GameObject 	player						= null;
	[System.NonSerialized] public GameObject 	boss						= null;
	[System.NonSerialized] public GameObject 	security					= null;
	[System.NonSerialized] public GameObject 	flyingObject				= null;
	[System.NonSerialized] public GameObject 	powerUpEnlarge				= null;
	[System.NonSerialized] public GameObject 	powerUpSecurity				= null;
	
	// Game
	[System.NonSerialized] public GameState 	gameState					= GameState.TALKING;
	[System.NonSerialized] public float 		cameraHeight				= 0.0f;
	[System.NonSerialized] public float 		cameraTarget				= 0.0f;
	[System.NonSerialized] public float 		cameraShakeTime				= 0.0f;
	[System.NonSerialized] public int			maxBossY					= 0;
	[System.NonSerialized] public int			punchNumber					= 0;
	[System.NonSerialized] public float			punchVolume					= 0;
	[System.NonSerialized] public bool			breakFurniture				= false;
	
	[System.NonSerialized] public int			comboCount					= 0;
	[System.NonSerialized] public float			comboTime					= 0;
	
	[System.NonSerialized] public float			objectCounter				= 0;
	[System.NonSerialized] public float			powerUpEnlargeCounter		= 0;
	[System.NonSerialized] public float			powerUpSecurityCounter		= 0;
	[System.NonSerialized] public float			internalMoney				= 0;
	
	[System.NonSerialized] public float			flashWhiteAlpha				= 0;
	
	[System.NonSerialized] public TutorialStep	tutorialStep				= TutorialStep.NONE;
	[System.NonSerialized] public float			tutorialAlpha				= 0;
	[System.NonSerialized] public float			tutorialCounter				= 0;
	
	[System.NonSerialized] public float			objectSpawnTime				= 0;
	[System.NonSerialized] public float			powerUpEnlargeSpawnTime		= 0;
	[System.NonSerialized] public float			powerUpSecuritySpawnTime	= 0;
	[System.NonSerialized] public int			securityProgress			= 0;
	
	
	private Vector2		txtMoneyAddOriginalPosition;
	private Vector2		txtMoneyAddOriginalAnchorMin;
	private Vector2		txtMoneyAddOriginalAnchorMax;
	private TextAnchor	txtMoneyAddOriginalAlignment;
	private int			txtMoneyAddOriginalFontSize;
	
	private int			totalReward;
	
	private bool		dangerShowed;
	private int			dangerCounter;
	
	private int			shouldSelect;
	
	
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
		btnReplay.SetActive (false);
		btnMainMenu.SetActive (false);
		imgNotice.SetActive (false);
		txtTutorial.SetActive (false);
		
		imgDanger.gameObject.SetActive (false);

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
		
		objectSpawnTime = Random.Range(OBJECT_SPAWN_TIME_MIN, OBJECT_SPAWN_TIME_MAX);
		dangerShowed = false;
		dangerCounter = 0;
		
		powerUpEnlargeSpawnTime = Random.Range(POWER_UP_ENLARGE_SPAWN_TIME_MIN, POWER_UP_ENLARGE_SPAWN_TIME_MAX);
		powerUpSecuritySpawnTime = Random.Range(POWER_UP_SECURITY_SPAWN_TIME_MIN, POWER_UP_SECURITY_SPAWN_TIME_MAX);
		
		shouldSelect = 0;
		
		securityProgress = 0;
		imgSecurityProgressFG.fillAmount = 0;
		
		imgSecurityProgressBG.gameObject.SetActive(false);
		imgSecurityProgressFG.gameObject.SetActive(false);
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
		/*
		if (Input.GetMouseButtonUp(0) && gameState == GameState.BOSS_RUNNING) {
			if (boss.GetComponent<SCR_Boss>().IsRunning()) {
				SceneManager.LoadScene("GSMenu/SCN_Menu");
				SCR_Audio.PlayClickSound();
			}
		}
		*/
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
				
				if (powerUpEnlarge != null) {
					powerUpEnlarge.GetComponent<SCR_PowerUp>().AddDeltaCameraToObject (deltaCamera);
				}
				
				if (powerUpSecurity != null) {
					powerUpSecurity.GetComponent<SCR_PowerUp>().AddDeltaCameraToObject (deltaCamera);
				}
			}
			
			if (!breakFurniture) {
				if (boss.GetComponent<SCR_Boss>().y > FURNITURE_Y - SCR_Boss.BOSS_SIZE) {
					SCR_Background.BreakFurniture (boss.GetComponent<SCR_Boss>().x, boss.GetComponent<SCR_Boss>().y, boss.GetComponent<SCR_Boss>().speedY);
					breakFurniture = true;
				}
			}
			
			if (flyingObject == null) {
				if (SCR_Profile.showTutorial == 0) {
					objectCounter += dt;
					if (objectCounter >= objectSpawnTime - OBJECT_DANGER_BEFORE && !dangerShowed) {
						ShowDanger();
						dangerShowed = true;
					}
					
					if (objectCounter >= objectSpawnTime) {
						objectCounter = 0;
						objectSpawnTime = Random.Range(OBJECT_SPAWN_TIME_MIN, OBJECT_SPAWN_TIME_MAX);
						
						dangerShowed = false;
						
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
			
			if (powerUpEnlarge == null) {
				powerUpEnlargeCounter += dt;
				if (powerUpEnlargeCounter >= powerUpEnlargeSpawnTime) {
					powerUpEnlargeCounter = 0;
					powerUpEnlargeSpawnTime = Random.Range(POWER_UP_ENLARGE_SPAWN_TIME_MIN, POWER_UP_ENLARGE_SPAWN_TIME_MAX);
					
					powerUpEnlarge = SCR_Pool.GetFreeObject(PFB_PowerUp[0]);
					
					float x = Random.Range (-(SCREEN_W - SCR_PowerUp.POWER_UP_SIZE) * 0.5f, (SCREEN_W - SCR_PowerUp.POWER_UP_SIZE) * 0.5f);
					float y = cameraHeight + SCREEN_H;
					powerUpEnlarge.GetComponent<SCR_PowerUp>().Spawn (x, y);
				}
			}
			else {
				float powerUpX = powerUpEnlarge.GetComponent<SCR_PowerUp>().x;
				float powerUpY = powerUpEnlarge.GetComponent<SCR_PowerUp>().y;
				float bossX = boss.GetComponent<SCR_Boss>().x;
				float bossY = boss.GetComponent<SCR_Boss>().y;
				float distance = (SCR_PowerUp.POWER_UP_SIZE + SCR_Boss.BOSS_SIZE) * 0.5f;
				if (SCR_Helper.DistanceBetweenTwoPoint(powerUpX, powerUpY, bossX, bossY) < distance) {
					powerUpEnlarge.SetActive(false);
					powerUpEnlarge = null;
					
					boss.GetComponent<SCR_Boss>().Enlarge();
				}
				else if (powerUpY <= cameraHeight - SCR_PowerUp.POWER_UP_SIZE) {
					powerUpEnlarge.SetActive (false);
					powerUpEnlarge = null;
				}
			}
			
			if (powerUpSecurity == null) {
				powerUpSecurityCounter += dt;
				if (powerUpSecurityCounter >= powerUpSecuritySpawnTime) {
					powerUpSecurityCounter = 0;
					powerUpSecuritySpawnTime = Random.Range(POWER_UP_SECURITY_SPAWN_TIME_MIN, POWER_UP_SECURITY_SPAWN_TIME_MAX);
					
					powerUpSecurity = SCR_Pool.GetFreeObject(PFB_PowerUp[1]);
					
					float x = Random.Range (-(SCREEN_W - SCR_PowerUp.POWER_UP_SIZE) * 0.5f, (SCREEN_W - SCR_PowerUp.POWER_UP_SIZE) * 0.5f);
					float y = cameraHeight + SCREEN_H;
					powerUpSecurity.GetComponent<SCR_PowerUp>().Spawn (x, y);
				}
			}
			else {
				float powerUpX = powerUpSecurity.GetComponent<SCR_PowerUp>().x;
				float powerUpY = powerUpSecurity.GetComponent<SCR_PowerUp>().y;
				float bossX = boss.GetComponent<SCR_Boss>().x;
				float bossY = boss.GetComponent<SCR_Boss>().y;
				float distance = (SCR_PowerUp.POWER_UP_SIZE + SCR_Boss.BOSS_SIZE) * 0.5f;
				if (SCR_Helper.DistanceBetweenTwoPoint(powerUpX, powerUpY, bossX, bossY) < distance) {
					powerUpSecurity.SetActive(false);
					powerUpSecurity = null;
					
					imgSecurityProgressFG.GetComponent<SCR_SecurityProgress>().Flash();
				}
				else if (powerUpY <= cameraHeight - SCR_PowerUp.POWER_UP_SIZE) {
					powerUpSecurity.SetActive (false);
					powerUpSecurity = null;
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
				/*
				for (int i=1; i<imgCombo.Length; i++) {
					imgCombo[i].SetActive (false);
				}
				imgCombo[0].SetActive (true);
				*/
			}
		}
		//imgClockContent.GetComponent<Image>().fillAmount = comboTime / COMBO_TIME;
		
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
		
		if (imgSecurityProgressFG.GetComponent<SCR_SecurityProgress>().powerUpTime < 0) {
			securityProgress++;
			int steps = SCR_Security.SECURITY_PROGRESS_STEPS;
			imgSecurityProgressFG.GetComponent<SCR_SecurityProgress>().SetTargetProgress((float)securityProgress / steps);
			if (securityProgress >= steps) {
				security.GetComponent<SCR_Security>().PerformPunch();
			}
		}
		else {
			security.GetComponent<SCR_Security>().PerformPunch();
		}
		
		/*
		for (int i=0; i<imgCombo.Length; i++) {
			imgCombo[i].SetActive (false);
		}
		imgCombo[comboCount].SetActive (true);
		*/
		punchNumber ++;
		if (SCR_Profile.showTutorial == 1) {
			txtTutorial.GetComponent<Text>().text = "Keep on punching (" + punchNumber.ToString() + "/3)";
			if (punchNumber == 3) {
				TriggerTutorial (TutorialStep.FINISH);
			}
		}
		
		ShowCombo(x, y);
	}
	
	public void SecurityPunchSuccess (float x, float y) {
		/*
		comboCount = 0;
		
		for (int i=1; i<imgCombo.Length; i++) {
			imgCombo[i].SetActive (false);
		}
		imgCombo[0].SetActive (true);
		*/
		
		comboTime = COMBO_TIME;
		comboCount++;
		ShowCombo(x, y);
		
		securityProgress = 0;
		if (imgSecurityProgressFG.GetComponent<SCR_SecurityProgress>().powerUpTime < 0) {
			imgSecurityProgressFG.GetComponent<SCR_SecurityProgress>().SetTargetProgress(0);
		}
	}
	
	private void ShowCombo (float x, float y) {
		if (comboCount >= 1) {
			//GameObject text = SCR_Pool.GetFreeObject (PFB_Combo[comboCount-1]);
			GameObject text = SCR_Pool.GetFreeObject (PFB_ComboText);
			text.GetComponent<Text>().text = "X" + comboCount;
			text.transform.SetParent (cvsMain.transform);
			text.GetComponent<SCR_ComboText>().Spawn (x, y);
			
			float startR = 255;
			float startG = 226;
			float startB = 0;
			
			float endR = 255;
			float endG = 0;
			float endB = 0;
			
			float startIndex = 2;
			float endIndex = 10;
			
			if (comboCount < startIndex) {
				text.GetComponent<SCR_ComboText>().SetColor (255, 255, 255);
			}
			else if (comboCount >= endIndex) {
				text.GetComponent<SCR_ComboText>().SetColor (endR, endG, endB);
			}
			else {
				text.GetComponent<SCR_ComboText>().SetColor (
					startR + (comboCount - startIndex) / (endIndex - startIndex) * (endR - startR),
					startG + (comboCount - startIndex) / (endIndex - startIndex) * (endG - startG),
					startB + (comboCount - startIndex) / (endIndex - startIndex) * (endB - startB));
			}
		}
	}
	
	public void Lose () {
		iTween.Stop(imgSecurityProgressFG.gameObject);
		imgSecurityProgressFG.GetComponent<SCR_SecurityProgress>().UpdateFlashAmount(0);
		
		pnlResult.SetActive (true);
		btnReplay.SetActive (true);
		btnMainMenu.SetActive (true);
		
		txtResultTitle.text = boss.GetComponent<SCR_Boss>().resultTitle[SCR_Profile.bossSelecting];
		txtResultTitle.fontSize = boss.GetComponent<SCR_Boss>().resultTitleFontSize[SCR_Profile.bossSelecting];
		
		SCR_WaitMusic.FadeIn();
		SCR_PunchMusic.FadeOut();
		
		if (maxBossY > SCR_Profile.highScore)
			imgHighScore.SetActive (true);
		else
			imgHighScore.SetActive (false);
		
		SCR_Profile.ReportScore (maxBossY);
		txtPunchNumber.GetComponent<Text>().text = punchNumber.ToString();
		txtHeightNumber.GetComponent<Text>().text = maxBossY.ToString();
		txtBestNumber.GetComponent<Text>().text = SCR_Profile.highScore.ToString();
		
		int money = (int)(maxBossY * 0.5);
		AddMoney (money);
		
		txtMoneyNumber.GetComponent<Text>().text = "$" + totalReward.ToString();
		
		imgSecurityProgressBG.color = new Color(1, 1, 1, 0);
		imgSecurityProgressFG.color = new Color(1, 1, 1, 0);
		
		// -- //
		bool found = false;
		
		for (int i = 0; i < SCR_Profile.bosses.Length; i++) {
			if (SCR_Profile.bosses[i].unlocked == 0 && SCR_Profile.money >= SCR_Profile.bosses[i].cost) {
				if (SCR_Profile.bosses[i].recommended == 0) {
					shouldSelect = i;
					found = true;
					break;
				}
			}
		}
		
		if (found) {
			imgNotice.SetActive (true);
		}
		// -- //
		
		SCR_UnityAnalytics.FinishGame(maxBossY);
	}
	
	public void ShowSecurityProgress () {
		if (!imgSecurityProgressBG.gameObject.activeSelf) {
			imgSecurityProgressBG.gameObject.SetActive(true);
			imgSecurityProgressFG.gameObject.SetActive(true);
			
			imgSecurityProgressBG.color = new Color(1, 1, 1, 0);
			imgSecurityProgressFG.color = new Color(1, 1, 1, 0);
			
			iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.25f, "easetype", "easeInOutSine", "onupdate", "UpdateSecurityProgressAlpha", "ignoretimescale", true));
		}
	}
	
	private void UpdateSecurityProgressAlpha (float alpha) {
		imgSecurityProgressBG.color = new Color(1, 1, 1, alpha);
		imgSecurityProgressFG.color = new Color(1, 1, 1, alpha);
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
		text.GetComponent<SCR_SpecialText>().Spawn (x, y + PUNCH_TEXT_OFFSET_Y);
	}
	public void ShowRicochet (float x, float y) {
		GameObject text = SCR_Pool.GetFreeObject (PFB_Ricochet);
		text.transform.SetParent (cvsMain.transform);
		text.GetComponent<SCR_SpecialText>().Spawn (x, y + PUNCH_TEXT_OFFSET_Y);
		//security.GetComponent<SCR_Security>().PerformPunch();
	}
	
	public void ShakeCamera (float duration) {
		cameraShakeTime = duration;
	}
	
	public void FlashWhite () {
		flashWhiteAlpha = 0.9f;
	}
	
	public void ShowDanger () {
		const float duration = 0.4f;
		imgDanger.color = new Color(imgDanger.color.r, imgDanger.color.g, imgDanger.color.b, 0);
		imgDanger.gameObject.SetActive(true);
		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 0.65f, "time", duration, "easetype", "easeInOutSine", "onupdate", "UpdateDanger", "oncomplete", "CompleteDanger", "looptype", "pingPong", "ignoretimescale", true));
	}
	
	private void UpdateDanger(float alpha) {
		imgDanger.color = new Color(imgDanger.color.r, imgDanger.color.g, imgDanger.color.b, alpha);
	}
	
	private void CompleteDanger() {
		dangerCounter++;
		if (dangerCounter == OBJECT_DANGER_TIMES * 2) {	// because 1 cycle has 2 phases
			iTween.Stop(gameObject);
			imgDanger.gameObject.SetActive(false);
			dangerCounter = 0;
		}
	}
	
	public void OnReplay () {
		SCR_RunSound.Stop();
		SCR_Audio.PlayClickSound();
		SceneManager.LoadScene("GSGameplay/SCN_Gameplay");
	}
	
	public void OnMainMenu () {
		if (imgNotice.activeSelf) {
			SCR_Profile.SelectBoss(shouldSelect);
			SCR_Profile.bosses[shouldSelect].recommended = 1;
			SCR_Profile.SaveProfile();
		}
		
		SCR_Audio.PlayClickSound();
		SceneManager.LoadScene("GSMenu/SCN_Menu");
	}
}
