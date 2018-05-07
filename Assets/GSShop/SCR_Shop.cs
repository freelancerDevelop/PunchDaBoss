using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SCR_Shop : MonoBehaviour {
	// Prefab
	public GameObject PFB_PunchEntry;
	
	// Object
	public Transform shopContent;
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
		entries = new GameObject[SCR_Profile.martialMoves.Length];
		for (int i=0; i<SCR_Profile.martialMoves.Length; i++) {
			GameObject entry = Instantiate(PFB_PunchEntry);
			entry.transform.GetChild(4).gameObject.GetComponent<Text>().text = SCR_Profile.martialMoves[i].name;
			entry.transform.GetChild(5).gameObject.GetComponent<Text>().text = SCR_Profile.martialMoves[i].cost.ToString();
			
			if (SCR_Profile.martialMoves[i].unlocked == 1) {
				entry.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color (0.4f, 0.8f, 0.4f, 0.4f);
				entry.transform.GetChild(2).gameObject.SetActive (false);
				entry.transform.GetChild(3).gameObject.SetActive (true);
				if (SCR_Profile.martialEquip == i) {
					entry.transform.GetChild(3).gameObject.GetComponent<Button>().interactable = false;
				}
				else {
					entry.transform.GetChild(3).gameObject.GetComponent<Button>().interactable = true;
				}
			}
			else if (SCR_Profile.money < SCR_Profile.martialMoves[i].cost) {
				entry.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color (0.8f, 0.4f, 0.4f, 0.4f);
				entry.transform.GetChild(3).gameObject.SetActive (false);
				entry.transform.GetChild(2).gameObject.SetActive (true);
				entry.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
			}
			else {
				entry.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color (0.8f, 0.8f, 0.8f, 0.4f);
				entry.transform.GetChild(3).gameObject.SetActive (false);
				entry.transform.GetChild(2).gameObject.SetActive (true);
				entry.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
			}
			
			int param = i;
			entry.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate{BuyPunch(param);});
			entry.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(delegate{EquipPunch(param);});
			
			entry.transform.SetParent (shopContent);
			entry.transform.localScale = new Vector3(1, 1, 1);
			
			entries[i] = entry;
		}
	}
	
	
	private void Update () {
		
	}
	
	
	public void BuyPunch (int index) {
		if (SCR_Profile.BuyPunch (index)) {
			RefreshShop();
		}
	}
	public void EquipPunch (int index) {
		if (SCR_Profile.EquipPunch (index)) {
			RefreshShop();
		}
	}
	public void RefreshShop () {
		for (int i=0; i<entries.Length; i++) {
			if (SCR_Profile.martialMoves[i].unlocked == 1) {
				entries[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color (0.4f, 0.8f, 0.4f, 0.4f);
				entries[i].transform.GetChild(2).gameObject.SetActive (false);
				entries[i].transform.GetChild(3).gameObject.SetActive (true);
				if (SCR_Profile.martialEquip == i) {
					entries[i].transform.GetChild(3).gameObject.GetComponent<Button>().interactable = false;
				}
				else {
					entries[i].transform.GetChild(3).gameObject.GetComponent<Button>().interactable = true;
				}
			}
			else if (SCR_Profile.money < SCR_Profile.martialMoves[i].cost) {
				entries[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color (0.8f, 0.4f, 0.4f, 0.4f);
				entries[i].transform.GetChild(3).gameObject.SetActive (false);
				entries[i].transform.GetChild(2).gameObject.SetActive (true);
				entries[i].transform.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
			}
			else {
				entries[i].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color (0.8f, 0.8f, 0.8f, 0.4f);
				entries[i].transform.GetChild(3).gameObject.SetActive (false);
				entries[i].transform.GetChild(2).gameObject.SetActive (true);
				entries[i].transform.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
			}
		}
	}
	
	public void OnBack () {
		// Load latest level
		SceneManager.LoadScene("GSMenu/SCN_Menu");
	}
}