using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Trail : MonoBehaviour {
	public const float TRAIL_SCALE			= 1.0f;
	public const float TRAIL_EMISSION_RATE 	= 200.0f;
	
	private void Start () {
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * TRAIL_SCALE, SCR_Gameplay.SCREEN_SCALE * TRAIL_SCALE, SCR_Gameplay.SCREEN_SCALE * TRAIL_SCALE);	
	}
	
	public void TurnParticleOn () {
		ParticleSystem.EmissionModule emission = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().emission;
		emission.rateOverTime = TRAIL_EMISSION_RATE;
	}
	
	public void TurnParticleOff () {
		ParticleSystem.EmissionModule emission = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().emission;
		emission.rateOverTime = 0;
	}
	
	public void JumpTo (float x, float y) {
		gameObject.SetActive (false);
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
		gameObject.SetActive (true);
	}
	
	public void MoveTo (float x, float y) {
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y, transform.position.z);
	}
	
	private void Update () {
		
	}
}
