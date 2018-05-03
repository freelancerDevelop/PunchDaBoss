using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Target : MonoBehaviour {
	public const float TARGET_SCALE		= 0.7f;
	public const float LINE_SCALE		= 0.8f;
	
	private void Start () {
		transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE * TARGET_SCALE, SCR_Gameplay.SCREEN_SCALE * TARGET_SCALE, 1);
		
		gameObject.GetComponent<LineRenderer>().widthMultiplier = SCR_Gameplay.SCREEN_SCALE * LINE_SCALE;
	}
	
	public void SetPosition (float x, float y) {
		transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x, y - SCR_Gameplay.instance.cameraHeight, transform.position.z);
	}
	
	public void SetLine (float x1, float y1, float x2, float y2) {
		gameObject.GetComponent<LineRenderer>().enabled = true;
		
		var points = new Vector3[2];
		points[0] = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x1, y1 - SCR_Gameplay.instance.cameraHeight, -20);
		points[1] = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f + x2, y2 - SCR_Gameplay.instance.cameraHeight, -20);
		
        gameObject.GetComponent<LineRenderer>().SetPositions(points);
	}
	
	public void HideLine () {
		gameObject.GetComponent<LineRenderer>().enabled = false;
	}

	private void Update () {
		
	}
}
