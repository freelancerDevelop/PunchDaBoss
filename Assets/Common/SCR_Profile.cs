using System.Collections;
using System.Collections.Generic;
using UnityEngine;





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
	public static int				martialEquip = 0;
	public static MartialMove[] 	martialMoves;
	
	
	
	public static void Init() {
		if (martialMoves == null) {
			martialMoves = new MartialMove[6];
			martialMoves[0] = new MartialMove ("Basic punch", 	0, 		200.0f, 	5000.0f, 	5000.0f,	2, 		"Your basic punch. Not too special, but did its job every time.");
			martialMoves[1] = new MartialMove ("Sucker punch", 	100, 	150.0f, 	5500.0f, 	5500.0f, 	3,		"Exclusively for your boss! Better than basic punch, but with higher cooldown.");
			martialMoves[2] = new MartialMove ("Slav punch", 	200, 	150.0f, 	6000.0f, 	4500.0f, 	2.2f,	"Just drink enough Vodka and you can execute this. Very powerful but wildly inaccurate. (Vodka, you know...)");
			martialMoves[3] = new MartialMove ("Tai-chi punch", 600, 	200.0f, 	8000.0f, 	1500.0f, 	4,		"When your boss said 'You are a slow worker!', show him this. Extremely powerful punch, but with a pathetic speed.");
			martialMoves[4] = new MartialMove ("Kung F-U", 		1000, 	100.0f, 	7000.0f, 	8000.0f, 	3.3f,	"There is no better way to say 'F-U' to someone than this. A fast punch, but require high preccision.");
			martialMoves[5] = new MartialMove ("Ninjutsu", 		2000, 	300.0f, 	6500.0f, 	6000.0f, 	4,		"The time that you're supposed to work, you spent on learning something else. An ez to hit punch with a long cooldown.");
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
			SaveProfile();
			martialEquip = index;
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
	
	
	
	
	
	public static void SaveProfile () {
		PlayerPrefs.SetInt("money", money);
		PlayerPrefs.SetInt("martialEquip", martialEquip);
		
		for (int i=0; i<martialMoves.Length; i++) {
			PlayerPrefs.SetInt("martialUnlocked" + i.ToString(), martialMoves[i].unlocked);
		}
	}
	
	public static void LoadProfile () {
		money = PlayerPrefs.GetInt("money", 0);
		martialEquip = PlayerPrefs.GetInt("martialEquip", 0);
		
		for (int i=0; i<martialMoves.Length; i++) {
			martialMoves[i].unlocked = PlayerPrefs.GetInt("martialUnlocked" + i.ToString(), 0);
		}
		
		money = 100000;
		martialMoves[0].unlocked = 1;
	}
	
	public static void ResetProfile () {
		money = 0;
		martialEquip = 0;
		for (int i=0; i<martialMoves.Length; i++) {
			PlayerPrefs.SetInt("martialUnlocked" + i.ToString(), martialMoves[i].unlocked);
		}
		
		SaveProfile ();
	}
}
