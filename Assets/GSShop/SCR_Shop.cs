﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SCR_Shop : MonoBehaviour {
	// Prefab
	public GameObject 	PFB_PunchEntry;
	public Sprite[] 	SPR_PunchIcon;
	
	// Object
	public Transform shopContent;
	public GameObject txtMoney;
	private GameObject[] entries;

	// Instance
	public static SCR_Shop instance = null;

	
	private void Start () {
		// Don't do anything if menu state is not the first state
		// Load the menu instead
		if (SCR_Menu.menuLoaded == false) {
			SceneManager.LoadScene("GSMenu/SCN_Menu");
			return;
		}
		
		// Assign instance
		instance = this;
		
		// Create the shop
		txtMoney.GetComponent<Text>().text = SCR_Profile.money.ToString() + "$";
		
		entries = new GameObject[SCR_Profile.martialMoves.Length];
		for (int i=0; i<SCR_Profile.martialMoves.Length; i++) {
			GameObject entry = Instantiate(PFB_PunchEntry);
			entry.transform.GetChild(0).gameObject.GetComponent<Text>().text = SCR_Profile.martialMoves[i].name;
			entry.transform.GetChild(1).gameObject.GetComponent<Text>().text = SCR_Profile.martialMoves[i].cost.ToString() + "$";
			entry.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = SPR_PunchIcon[i];
			
			if (SCR_Profile.money < SCR_Profile.martialMoves[i].cost) {
				entry.GetComponent<Button>().interactable = false;
			}
			else {
				entry.GetComponent<Button>().interactable = true;
			}
			
			int param = i;
			entry.GetComponent<Button>().onClick.AddListener(delegate{BuyPunch(param);});
			
			entry.transform.SetParent (shopContent);
			entry.transform.localScale = new Vector3(1, 1, 1);
			
			entries[i] = entry;
		}
	}
	
	
	private void Update () {
		
	}
	
	
	public void BuyPunch (int index) {
		/*
		if (SCR_Profile.BuyPunch (index)) {
			RefreshShop();
		}
		else if (SCR_Profile.EquipPunch (index)) {
			RefreshShop();
		}
		*/
	}
	
	public void RefreshShop () {
		txtMoney.GetComponent<Text>().text = SCR_Profile.money.ToString() + "$";
		
		for (int i=0; i<entries.Length; i++) {
			if (SCR_Profile.money < SCR_Profile.martialMoves[i].cost) {
				entries[i].GetComponent<Button>().interactable = false;
			}
			else {
				entries[i].GetComponent<Button>().interactable = true;
			}
		}
	}
	
	public void OnBack () {
		// Load latest level
		SceneManager.LoadScene("GSMenu/SCN_Menu");
	}
}