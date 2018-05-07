using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class MartialMove {
	public string	name		= "";
	public int 		cost 		= 0;
	public float 	range 		= 0;
	public float 	force 		= 0;
	public float 	speed 		= 0;
	
	public int		unlocked 	= 0;
	
	public MartialMove (string n, int c, float r, float f, float s) {
		name = n;
		cost = c;
		range = r;
		force = f;
		speed = s;
	}
}

public class SCR_Profile {
	public static int 				money = 0;
	public static int				martialEquip = 0;
	public static MartialMove[] 	martialMoves;
	
	
	
	public static void Init() {
		if (martialMoves == null) {
			martialMoves = new MartialMove[6];
			martialMoves[0] = new MartialMove ("Basic punch", 	0, 		200.0f, 	5000.0f, 	5000.0f);
			martialMoves[1] = new MartialMove ("Sucker punch", 	100, 	150.0f, 	5000.0f, 	5000.0f);
			martialMoves[2] = new MartialMove ("Tai-chi punch", 400, 	200.0f, 	10000.0f, 	1000.0f);
			martialMoves[3] = new MartialMove ("Kung F-U", 		700, 	150.0f, 	7000.0f, 	6000.0f);
			martialMoves[4] = new MartialMove ("Cyka punch", 	1000, 	100.0f, 	8000.0f, 	4000.0f);
			martialMoves[5] = new MartialMove ("Shuryuken", 	2000, 	150.0f, 	10000.0f, 	8000.0f);
		}
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
		
		//money = 2000;
		martialMoves[0].unlocked = 1;
	}
	
	public static void ResetProfile () {
		SaveProfile ();
	}
}
