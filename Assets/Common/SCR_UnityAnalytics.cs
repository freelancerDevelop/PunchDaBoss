using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SCR_UnityAnalytics {
	private static System.DateTime startTimeGame;
	private static System.DateTime startTimeTutorial;
	
	public static void StartGame() {
		startTimeGame = System.DateTime.Now;
		Analytics.CustomEvent("StartGame", null);
	}
	
	public static void FinishGame(int score) {
		System.DateTime finishTime = System.DateTime.Now;
		float deltaSeconds = (float)(finishTime - startTimeGame).TotalSeconds;
		Analytics.CustomEvent("FinishGame", new Dictionary<string, object>
		{
			{ "Score", score },
			{ "TimeInSeconds", deltaSeconds }
		});
	}
	
	public static void StartTutorial() {
		startTimeTutorial = System.DateTime.Now;
		Analytics.CustomEvent("StartTutorial", null);
	}
	
	public static void FinishTutorial() {
		System.DateTime finishTime = System.DateTime.Now;
		float deltaSeconds = (float)(finishTime - startTimeTutorial).TotalSeconds;
		Analytics.CustomEvent("FinishTutorial", new Dictionary<string, object>
		{
			{ "TimeInSeconds", deltaSeconds }
		});
	}	
}
