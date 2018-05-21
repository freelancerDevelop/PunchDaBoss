using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PunchType {
	BASIC = 0,
	SLAV,
	SUCKER,
	JUDO,
	KUNGFU,
	NINJUTSU,
	TAICHI,
	PLACEHOLDER_1,
	PLACEHOLDER_2,
	COUNT
}



public class MartialMove {
	public string	name		= "";
	public int 		cost 		= 0;
	public float 	range 		= 0;
	public float 	force 		= 0;
	public float 	speed 		= 0;
	public float 	cooldown	= 0;
	public string	desc		= "";
	public int		unlocked 	= 0;
	
	public MartialMove (string n, int c, float r, float f, float s, float cool, string d) {
		name = n;
		cost = c;
		range = r;
		force = f;
		speed = s;
		cooldown = cool;
		desc = d;
	}
}

public class SCR_Profile {
	public static int 				money = 0;
	public static int 				highScore = 0;
	public static int				martialEquip = 0;
	public static MartialMove[] 	martialMoves;
	public static int				showTutorial = 1;
	public static int				soundOn = 1;
	
	
	public static void Init() {
		if (martialMoves == null) {
			martialMoves = new MartialMove[(int)PunchType.COUNT];
			martialMoves[(int)PunchType.BASIC] 		= new MartialMove ("Basic punch", 	0, 		200.0f, 	2600.0f, 	3700.0f,	1.2f,	"Your basic street punch. Not too special, but did its job every time.");
			martialMoves[(int)PunchType.SLAV] 		= new MartialMove ("Gopnik punch", 	500, 	200.0f, 	4500.0f, 	5000.0f, 	1.0f,	"Just drink enough Vodka and you can execute this. Very powerful but wildly inaccurate. (Vodka, you know...)");
			martialMoves[(int)PunchType.SUCKER] 	= new MartialMove ("Bat punch", 	750, 	160.0f, 	3000.0f, 	4200.0f, 	1.7f,	"This is not the punch your boss deserves, but the one he needs! Better than basic, but has higher cooldown.");
			martialMoves[(int)PunchType.JUDO] 		= new MartialMove ("Judo punch", 	1500, 	180.0f, 	3500.0f, 	4000.0f, 	1.5f,	"What? Last time I checked, you don't punch in Judo?");
			martialMoves[(int)PunchType.KUNGFU] 	= new MartialMove ("Kung F-U", 		2000, 	130.0f, 	4600.0f, 	6000.0f, 	2.6f,	"There is no better way to say 'F-U' to someone than this. A fast punch, but require high preccision.");
			martialMoves[(int)PunchType.NINJUTSU] 	= new MartialMove ("Ninjutsu", 		2500, 	250.0f, 	4200.0f, 	5500.0f, 	2.9f,	"The time that you're supposed to work, you spent on learning something else. An ez to hit punch with a long cooldown.");
			martialMoves[(int)PunchType.TAICHI] 	= new MartialMove ("Tai-chi punch", 750, 	220.0f, 	6000.0f, 	2000.0f, 	3.6f,	"When your boss said 'You are a slow worker!', show him this. Extremely powerful punch, but with a pathetic speed.");
			martialMoves[(int)PunchType.PLACEHOLDER_1] 	= new MartialMove ("Placeholder", 0, 	220.0f, 	6000.0f, 	2000.0f, 	3.6f,	"Placeholder.");
			martialMoves[(int)PunchType.PLACEHOLDER_2] 	= new MartialMove ("Placeholder", 0, 	220.0f, 	6000.0f, 	2000.0f, 	3.6f,	"Placeholder.");
		}
	}
	
	
	
	public static string GetPunchName() {
		return martialMoves[martialEquip].name;
	}
	
	public static float GetPunchRange() {
		return martialMoves[martialEquip].range;
	}
	
	public static float GetPunchForce() {
		return martialMoves[martialEquip].force;
	}
	
	public static float GetPunchSpeed() {
		return martialMoves[martialEquip].speed;
	}
	
	public static float GetPunchCooldown() {
		return martialMoves[martialEquip].cooldown;
	}
	
	
	
	
	public static bool BuyPunch (int index) {
		if (martialMoves[index].unlocked == 0 && money >= martialMoves[index].cost) {
			money -= martialMoves[index].cost;
			martialMoves[index].unlocked = 1;
			martialEquip = index;
			SaveProfile();
			return true;
		}
		return false;
	}
	
	public static bool EquipPunch (int index) {
		if (martialMoves[index].unlocked == 1) {
			martialEquip = index;
			PlayerPrefs.SetInt("martialEquip", martialEquip);
			return true;
		}
		return false;
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
		PlayerPrefs.SetInt("martialEquip", martialEquip);
		PlayerPrefs.SetInt("soundOn", soundOn);
		
		for (int i=0; i<martialMoves.Length; i++) {
			PlayerPrefs.SetInt("martialUnlocked" + i.ToString(), martialMoves[i].unlocked);
		}
	}
	
	public static void LoadProfile () {
		money = PlayerPrefs.GetInt("money", 0);
		highScore = PlayerPrefs.GetInt("highScore", 0);
		showTutorial = PlayerPrefs.GetInt("showTutorial", 1);
		martialEquip = PlayerPrefs.GetInt("martialEquip", 0);
		soundOn = PlayerPrefs.GetInt("soundOn", 1);
		
		for (int i=0; i<martialMoves.Length; i++) {
			martialMoves[i].unlocked = PlayerPrefs.GetInt("martialUnlocked" + i.ToString(), 0);
		}
		
		martialMoves[0].unlocked = 1;
	}
	
	public static void ResetProfile () {
		showTutorial = 1;
		money = 0;
		highScore = 0;
		martialEquip = 0;
		for (int i=1; i<martialMoves.Length; i++) {
			martialMoves[i].unlocked = 0;
		}
		
		SaveProfile ();
	}
}
