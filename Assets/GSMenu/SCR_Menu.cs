using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Menu : MonoBehaviour {
	public static bool menuLoaded = false;
	
	private void Start () {
		// Set up game's stuff
		Application.targetFrameRate = 60;
		Screen.orientation = ScreenOrientation.Portrait;
		Screen.SetResolution(540, 960, false);
	
		// Load profile
		SCR_Profile.LoadProfile ();
		
		// OK, confirm that menu is now the first state
		menuLoaded = true;
	}
	
	public void OnPlay () {
		// Load latest level
		SceneManager.LoadScene("GSGameplay/SCN_Gameplay");
	}
}
