using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss {
	public string	name		= "";
	public int 		cost 		= 0;
	public int	 	unlocked	= 0;
	
	public Boss (string n, int c) {
		name = n;
		cost = c;
	}
}

public class SCR_Profile {
	public static int 				money 			= 0;
	public static int 				highScore 		= 0;
	public static int				showTutorial 	= 1;
	public static int				soundOn 		= 1;
	public static int				bossSelecting 	= 0;
	
	public static Boss[] 			bosses;
	
	
	public static void Init() {
		if (bosses == null) {
			bosses = new Boss[4];
			bosses[0] 	= new Boss ("The boss", 		    0);
			bosses[1] 	= new Boss ("The dictator", 	10000);
			bosses[2] 	= new Boss ("Mr President",		20000);
			bosses[3] 	= new Boss ("The general", 		30000);
			//bosses[4] 	= new Boss ("Erix luke", 		50000);
		}
	}
	
	
	public static bool BuyBoss (int index) {
		if (bosses[index].unlocked == 0 && money >= bosses[index].cost) {
			money -= bosses[index].cost;
			bosses[index].unlocked = 1;
			SaveProfile();
			return true;
		}
		return false;
	}
	
	public static void ChangeName (int index, string name) {
		bosses[index].name = name;
		SaveProfile();
	}
	
	public static void SelectBoss (int index) {
		if (bosses[index].unlocked == 1) {
			bossSelecting = index;
			PlayerPrefs.SetInt("bossSelecting", bossSelecting);
		}
	}
	
	public static void AddMoney (int m) {
		money += m;
		PlayerPrefs.SetInt("money", money);
	}
	
	public static void ReportScore (int score) {
		if (score > highScore) {
			highScore = score;
			PlayerPrefs.SetInt("highScore", highScore);
		}
	}
	
	public static void ToggleSound () {
		soundOn = 1 - soundOn;
		PlayerPrefs.SetInt("soundOn", soundOn);
	}
	
	public static void SaveProfile () {
		PlayerPrefs.SetInt("money", money);
		PlayerPrefs.SetInt("highScore", highScore);
		PlayerPrefs.SetInt("showTutorial", showTutorial);
		PlayerPrefs.SetInt("bossSelecting", bossSelecting);
		PlayerPrefs.SetInt("soundOn", soundOn);
		
		for (int i=0; i<bosses.Length; i++) {
			PlayerPrefs.SetInt("bossesUnlocked" + i.ToString(), bosses[i].unlocked);
			PlayerPrefs.SetString("bossesName" + i.ToString(), bosses[i].name);
		}
	}
	
	public static void LoadProfile () {
		money = PlayerPrefs.GetInt("money", 0);
		highScore = PlayerPrefs.GetInt("highScore", 0);
		showTutorial = PlayerPrefs.GetInt("showTutorial", 1);
		bossSelecting = PlayerPrefs.GetInt("bossSelecting", 0);
		soundOn = PlayerPrefs.GetInt("soundOn", 1);
		
		for (int i=0; i<bosses.Length; i++) {
			bosses[i].unlocked = PlayerPrefs.GetInt("bossesUnlocked" + i.ToString(), 0);
			//bosses[i].name = PlayerPrefs.GetString("bossesName" + i.ToString(), bosses[i].name);
		}
		bosses[0].unlocked = 1;
	}
	
	public static void ResetProfile () {
		showTutorial = 1;
		money = 0;
		highScore = 0;
		bossSelecting = 0;
		soundOn = 1;
		for (int i=1; i<bosses.Length; i++) {
			bosses[i].unlocked = 0;
		}
		
		SaveProfile ();
	}
	
	public static void CheatMoney () {
		money = money + 10000;
		PlayerPrefs.SetInt("money", money);
	}
}
