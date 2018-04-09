using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Helper : MonoBehaviour {
	public static float RAD_TO_DEG = 57.29577951308231f;
	public static float DEG_TO_RAD = 0.0174532925199433f;

	public static float DistanceBetweenTwoPoint (float x1, float y1, float x2, float y2) {
		return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
	}

	public static float AngleBetweenTwoPoint (float x1, float y1, float x2, float y2) {
		float angle = 0;
		if (y2 == y1) {
			if (x2 > x1)
				angle = 90;
			else if (x2 < x1)
				angle = 270;
		}
		else {
			angle = Mathf.Atan((x2 - x1) / (y2 - y1)) * RAD_TO_DEG;
			if (y2 < y1) {
				angle += 180;
			}
			if (angle < 0) angle += 360;
		}

		return angle;
	}
	
	public static float AngleBetweenAngle (float a1, float a2) {
		if (Mathf.Abs(a1 - a2) < 180) {
			return a1 - a2;
		}
		else if (a1 > 180) {
			return a1 - a2 - 360;
		}
		else {
			return a1 - a2 + 360;
		}
	}

	public static float Sin (float angle) {
		return Mathf.Sin(angle * DEG_TO_RAD);
	}

	public static float Cos (float angle) {
		return Mathf.Cos(angle * DEG_TO_RAD);
	}

}
