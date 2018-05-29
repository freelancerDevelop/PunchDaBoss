using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Menu : MonoBehaviour {
	public static bool menuLoaded = false;
	public GameObject imgBackground2 = null;
	public GameObject imgPlayer = null;
	
	public GameObject btnSoundOn = null;
	public GameObject btnSoundOff = null;
	
	private float timeCounter = 0;
	private float sequence1 = 0;
	private float sequence2 = 0;
	private float lightCounter = 0;
	
	private float playerDir = -1;
	private float playerX = 0;
	private float playerY = 150;
	
	private static bool musicPlayed = false;
	
	private void Start () {
		// Set up game's stuff
		Application.targetFrameRate = 60;
		Screen.orientation = ScreenOrientation.Portrait;
		Screen.SetResolution(540, 960, false);
	
		// Load profile
		SCR_Profile.Init ();
		SCR_Profile.LoadProfile ();
		RefreshSoundButtonStatus();
		
		// OK, confirm that menu is now the first state
		menuLoaded = true;
		
		// Animation
		sequence1 = Random.Range (4, 6);
		sequence2 = Random.Range (0.3f, 0.7f);
		
		// Music
		SCR_WaitMusic.FadeIn();
		SCR_PunchMusic.FadeOut();
	}
	
	public void OnPlay () {
		// Load latest level
		SceneManager.LoadScene("GSGameplay/SCN_Gameplay");
		SCR_Audio.PlayClickSound();
	}
	
	public void OnShop () {
		// Load latest level
		SceneManager.LoadScene("GSShop/SCN_Shop");
		SCR_Audio.PlayClickSound();
	}
	
	public void OnSound () {
		SCR_Profile.ToggleSound();
		RefreshSoundButtonStatus();
		RefreshSoundStatus();
		SCR_Audio.PlayClickSound();
	}
	
	public void RefreshSoundButtonStatus () {
		if (SCR_Profile.soundOn == 1) {
			btnSoundOn.SetActive (true);
			btnSoundOff.SetActive (false);
		}
		else {
			btnSoundOn.SetActive (false);
			btnSoundOff.SetActive (true);
		}
	}
	
	public void RefreshSoundStatus () {
		if (SCR_Profile.soundOn == 1) {
			if (musicPlayed == true) {
				SCR_WaitMusic.Play();
				SCR_PunchMusic.Play();
			}
		}
		else {
			if (musicPlayed == true) {
				SCR_WaitMusic.Stop();
				SCR_PunchMusic.Stop();
			}
		}
	}
	
	public void OnReset () {
		SCR_Profile.ResetProfile();
		SCR_Audio.PlayClickSound();
	}
	
	private void Update () {
		float dt = Time.deltaTime;
		timeCounter += dt;
		
		if (!musicPlayed) {
			musicPlayed = true;
			SCR_WaitMusic.Play();
			SCR_PunchMusic.Play();
		}
		
		if (timeCounter < sequence1) {
			imgBackground2.SetActive (true);
		}
		else if (timeCounter > sequence1 && timeCounter < sequence1 + sequence2) {
			lightCounter += dt;
			if (lightCounter > 0.07f) {
				lightCounter = 0;
				imgBackground2.SetActive (!imgBackground2.activeSelf);
			}
		}
		else if (timeCounter > sequence1 + sequence2 && timeCounter < 10) {
			imgBackground2.SetActive (false);
		}
		else if (timeCounter > 10) {
			timeCounter = 0;
			sequence1 = Random.Range (4, 6);
			sequence2 = Random.Range (0.5f, 0.7f);
		}
		
		
		playerX += playerDir * dt * 200;
		if (playerX < -750) playerDir = 1;
		if (playerX >  900) playerDir = -1;
		imgPlayer.transform.localScale = new Vector3 (-playerDir * 0.8f, 0.8f, 1);
		imgPlayer.GetComponent<RectTransform>().anchoredPosition = new Vector3 (playerX, playerY);
	}
}
