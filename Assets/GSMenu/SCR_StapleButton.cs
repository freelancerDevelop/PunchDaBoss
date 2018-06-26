using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SCR_StapleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
	private bool hold = false;
	
	public void OnPointerDown (PointerEventData eventData) {
		if (GetComponent<Button>().interactable == true) {
			transform.GetChild(0).gameObject.SetActive (false);
			transform.GetChild(1).gameObject.SetActive (true);
			
			hold = true;
		}
	}
	
	public void OnPointerUp (PointerEventData eventData) {
		if (GetComponent<Button>().interactable == true) {
			transform.GetChild(0).gameObject.SetActive (true);
			transform.GetChild(1).gameObject.SetActive (false);
			
			hold = false;
		}
	}
	
	public void OnPointerEnter (PointerEventData eventData) {
		if (GetComponent<Button>().interactable == true && hold == true) {
			transform.GetChild(0).gameObject.SetActive (false);
			transform.GetChild(1).gameObject.SetActive (true);
		}
	}
	
	public void OnPointerExit (PointerEventData eventData) {
		if (GetComponent<Button>().interactable == true && hold == true) {
			transform.GetChild(0).gameObject.SetActive (true);
			transform.GetChild(1).gameObject.SetActive (false);
		}
	}
}
