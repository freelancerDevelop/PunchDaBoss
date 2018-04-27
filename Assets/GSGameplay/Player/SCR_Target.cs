using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Target : MonoBehaviour {
	public const float TARGET_SCALE		= 0.7f;
	
	private void Start () {
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * TARGET_SCALE, SCR_Gameplay.SCREEN_SCALE * TARGET_SCALE, 1);
	}
	
	public void SetPosition (float x, float y) {
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y - SCR_Gameplay.instance.cameraHeight, transform.position.z);
	}

	private void Update () {
		
	}
}
