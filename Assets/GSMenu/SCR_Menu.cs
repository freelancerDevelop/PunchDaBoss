using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DragonBones;


public class SCR_Menu : MonoBehaviour {
	public static bool menuLoaded = false;
	
	public GameObject btnSoundOn = null;
	public GameObject btnSoundOff = null;
	
	public GameObject txtMoney = null;
	
	public GameObject btnPlay = null;
	public GameObject btnBuy = null;
	
	public GameObject inpName = null;
	
	public GameObject[] dgbBoss;
	
	public SCR_Scroll scrScroll;
	
	private static int bossSelecting = 0;
	private static bool musicPlayed = false;
	
	private void Start () {
		// Set up game's stuff
		Application.targetFrameRate = 60;
		Screen.orientation = ScreenOrientation.Portrait;
		//Screen.SetResolution(540, 960, false);
	
		// Load profile
		SCR_Profile.Init ();
		SCR_Profile.LoadProfile ();
		
		bossSelecting = SCR_Profile.bossSelecting;
		
		RefreshSoundButtonStatus();
		UpdateMoneyNumber();
		UpdateBoss();
		
		// Music
		SCR_WaitMusic.FadeIn();
		SCR_PunchMusic.FadeOut();
		SCR_RunSound.Stop();
		
		// OK, confirm that menu is now the first state
		menuLoaded = true;
		
		// Skip right into gameplay for tutorial
		/*
		if (SCR_Profile.showTutorial == 1) {
			SceneManager.LoadScene("GSGameplay/SCN_Gameplay");
		}
		*/
	}
	
	public void OnPlay () {
		// Load latest level
		SCR_Profile.SelectBoss (bossSelecting);
		SCR_Audio.PlayClickSound();
		SceneManager.LoadScene("GSGameplay/SCN_Gameplay");
	}
	
	public void OnSound () {
		SCR_Profile.ToggleSound();
		RefreshSoundButtonStatus();
		RefreshSoundStatus();
		SCR_Audio.PlayClickSound();
	}
	
	public void OnNext () {
		if (!scrScroll.tweening) {
			bossSelecting ++;
			if (bossSelecting >= SCR_Profile.bosses.Length) {
				bossSelecting = 0;
			}
			UpdateBoss();
			SCR_Audio.PlayClickSound();
			scrScroll.ForceSnapProfileIndex(bossSelecting);
		}
	}
	public void OnPrev () {
		if (!scrScroll.tweening) {
			bossSelecting --;
			if (bossSelecting < 0) {
				bossSelecting = SCR_Profile.bosses.Length - 1;
			}
			UpdateBoss();
			SCR_Audio.PlayClickSound();
			scrScroll.ForceSnapProfileIndex(bossSelecting);
		}
	}
	
	public void OnBuy () {
		if (bossSelecting == 0 || bossSelecting == 1) {
			SCR_Profile.BuyBoss (bossSelecting);
			UpdateBoss();
			UpdateMoneyNumber();
		}
		SCR_Audio.PlayBuySound();
	}
	
	public void SelectBoss (int index) {
		bossSelecting = index;
		UpdateBoss();
	}
	
	public void UpdateBoss () {
		inpName.GetComponent<InputField>().text = SCR_Profile.bosses[bossSelecting].name;
		
		if (SCR_Profile.bosses[bossSelecting].unlocked == 1) {
			btnPlay.SetActive (true);
			btnBuy.SetActive (false);
			inpName.GetComponent<InputField>().interactable = true;
		}
		else {
			btnPlay.SetActive (false);
			btnBuy.SetActive (true);
			inpName.GetComponent<InputField>().interactable = false;
			
			btnBuy.transform.GetChild(0).gameObject.GetComponent<Text>().text = SCR_Profile.bosses[bossSelecting].cost.ToString();
			btnBuy.transform.GetChild(1).gameObject.GetComponent<Text>().text = SCR_Profile.bosses[bossSelecting].cost.ToString();
			if (SCR_Profile.money >= SCR_Profile.bosses[bossSelecting].cost) {
				btnBuy.GetComponent<Button>().interactable = true;
			}
			else {
				btnBuy.GetComponent<Button>().interactable = false;
			}
		}
	}
	
	public void OnUpdateBossName () {
		SCR_Profile.ChangeName (bossSelecting, inpName.GetComponent<InputField>().text);
	}
	
	public void UpdateMoneyNumber () {
		txtMoney.GetComponent<Text>().text = SCR_Profile.money.ToString();
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
		UpdateMoneyNumber();
		UpdateBoss();
	}
	
	public void OnCheatMoney () {
		SCR_Profile.CheatMoney();
		SCR_Audio.PlayClickSound();
		UpdateMoneyNumber();
		UpdateBoss();
	}
	
	private void Update () {
		if (!musicPlayed && SCR_WaitMusic.ready && SCR_PunchMusic.ready) {
			musicPlayed = true;
			SCR_WaitMusic.Play();
			SCR_PunchMusic.Play();
		}
	}
}
