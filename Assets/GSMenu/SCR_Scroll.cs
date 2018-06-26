using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Scroll : MonoBehaviour {
	public RectTransform[] bosses;
	public RectTransform[] shadows;
	public RectTransform[] comingSoon;
	public SCR_Menu scrMenu;
	
	private const float BOSS_DISTANCE = 1000;
	private const float SCROLL_ZONE_TOP = 0.6f;
	private const float SCROLL_ZONE_BOTTOM = 0.25f;
	private const float SCROLL_ZONE_LEFT = 0.2f;
	private const float SCROLL_ZONE_RIGHT = 0.8f;
	private const float SPEED_SNAP = 4000;
	
	private RectTransform rectTransform = null;
	private float lastX = 0;
	private float speedDrag = 0;
	
	private int snapIndex = 0;
	private int currentBoss = 0;
	
	private bool dragging = false;
	public bool tweening = false;
	
	private int[] profileIndexes;
	
	private int lastBossDisplaying = 0;
	
	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform>();
		
		profileIndexes = new int[bosses.Length];
		for (int i = 0; i < bosses.Length; i++) {
			profileIndexes[i] = i;
		}
		
		SelectProfileIndex(SCR_Profile.bossSelecting);
	}
	
	// Update is called once per frame
	void Update () {
		// Mouse down
		if (Input.GetMouseButtonDown(0)) {
			Vector2 local;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out local);
			
			float top = (SCROLL_ZONE_TOP - 0.5f) * rectTransform.rect.height;
			float bottom = (SCROLL_ZONE_BOTTOM - 0.5f) * rectTransform.rect.height;
			
			float left = (SCROLL_ZONE_LEFT - 0.5f) * rectTransform.rect.width;
			float right = (SCROLL_ZONE_RIGHT - 0.5f) * rectTransform.rect.width;
			
			if (local.y > bottom && local.y < top && local.x > left && local.x < right) {
				dragging = true;
				lastX = local.x;
				iTween.Stop(gameObject);
				tweening = false;
			}
		}
		
		if (dragging) {
			// Mouse move
			if (Input.GetMouseButton(0)) {
				Vector2 local;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out local);
				
				float x = local.x;
				float dx = (x - lastX) * 2;
				
				for (var i = 0; i < bosses.Length; i++) {
					bosses[i].anchoredPosition = new Vector2(bosses[i].anchoredPosition.x + dx, bosses[i].anchoredPosition.y);
					shadows[i].anchoredPosition = new Vector2(shadows[i].anchoredPosition.x + dx, shadows[i].anchoredPosition.y);
					comingSoon[i].anchoredPosition = new Vector2(comingSoon[i].anchoredPosition.x + dx, comingSoon[i].anchoredPosition.y);
				}
				
				speedDrag = dx / Time.deltaTime;
				
				lastX = x;
			}
			
			// Mouse up
			if (Input.GetMouseButtonUp(0)) {
				dragging = false;
				Snap();
			}
		}
		
		if (!tweening) {
			AlignBosses();
		}
		
		// Update name & price
		int bossDisplaying = 0;
		for (int i = 0; i < bosses.Length; i++) {
			if (bosses[i].anchoredPosition.x >= -BOSS_DISTANCE * 0.5 && bosses[i].anchoredPosition.x < BOSS_DISTANCE * 0.5) {
				bossDisplaying = i;
				break;
			}
		}
		if (bossDisplaying != lastBossDisplaying) {
			scrMenu.SelectBoss(profileIndexes[bossDisplaying]);
			lastBossDisplaying = bossDisplaying;
		}
	}
	
	private void AlignBosses() {
		// Move last boss to first position
		if (bosses[bosses.Length - 1].anchoredPosition.x > 2 * BOSS_DISTANCE) {
			MoveLastToFirst();
		}
		
		// Move first boss to last position
		if (bosses[0].anchoredPosition.x < -2 * BOSS_DISTANCE) {
			MoveFirstToLast();
		}
	}
	
	private void MoveLastToFirst() {
		Vector2 pos = bosses[0].anchoredPosition;
		bosses[bosses.Length - 1].anchoredPosition = new Vector2(pos.x - BOSS_DISTANCE, pos.y);
		
		RectTransform rt = bosses[bosses.Length - 1];
		RectTransform srt = shadows[bosses.Length - 1];
		RectTransform crt = comingSoon[bosses.Length - 1];
		int pi = profileIndexes[bosses.Length - 1];
		for (int i = bosses.Length - 1; i >= 1; i--) {
			bosses[i] = bosses[i - 1];
			shadows[i] = shadows[i - 1];
			comingSoon[i] = comingSoon[i - 1];
			profileIndexes[i] = profileIndexes[i - 1];
		}
		bosses[0] = rt;
		shadows[0] = srt;
		comingSoon[0] = crt;
		profileIndexes[0] = pi;
		
		currentBoss++;
	}
	
	private void MoveFirstToLast() {
		Vector2 pos = bosses[bosses.Length - 1].anchoredPosition;
		bosses[0].anchoredPosition = new Vector2(pos.x + BOSS_DISTANCE, pos.y);
		
		RectTransform rt = bosses[0];
		RectTransform srt = shadows[0];
		RectTransform crt = comingSoon[0];
		int pi = profileIndexes[0];
		for (int i = 0; i < bosses.Length - 1; i++) {
			bosses[i] = bosses[i + 1];
			shadows[i] = shadows[i + 1];
			comingSoon[i] = comingSoon[i + 1];
			profileIndexes[i] = profileIndexes[i + 1];
		}
		bosses[bosses.Length - 1] = rt;
		shadows[bosses.Length - 1] = srt;
		comingSoon[bosses.Length - 1] = crt;
		profileIndexes[bosses.Length - 1] = pi;
		
		currentBoss--;
	}
	
	private void Snap() {
		snapIndex = 0;
		for (int i = 0; i < bosses.Length; i++) {
			if (bosses[i].anchoredPosition.x >= -BOSS_DISTANCE * 0.5 && bosses[i].anchoredPosition.x < BOSS_DISTANCE * 0.5) {
				snapIndex = i;
			}
		}
		
		if (snapIndex == currentBoss) {
			if (speedDrag > SPEED_SNAP) {
				snapIndex--;
			}
			
			if (speedDrag < -SPEED_SNAP) {
				snapIndex++;
			}
			
			if (snapIndex < 0) snapIndex = 0;
			if (snapIndex > bosses.Length - 1) snapIndex = bosses.Length - 1;
		}
		
		TweenBosses();
	}
	
	public void ForceSnapProfileIndex(int profileIndex) {
		for (var i = 0; i < bosses.Length; i++) {
			if (profileIndexes[i] == profileIndex) {
				snapIndex = i;
				break;
			}
		}
		
		TweenBosses();
	}
	
	private void TweenBosses() {
		tweening = true;
		iTween.ValueTo(gameObject, iTween.Hash("from", bosses[snapIndex].anchoredPosition.x, "to", 0, "time", 0.5f, "onupdate", "UpdateBossPosition", "oncomplete", "CompleteSnap", "easetype", "easeOutSine"));
	}
	
	private void UpdateBossPosition(float x) {
		bosses[snapIndex].anchoredPosition = new Vector2(x, bosses[snapIndex].anchoredPosition.y);
		shadows[snapIndex].anchoredPosition = new Vector2(x, shadows[snapIndex].anchoredPosition.y);
		comingSoon[snapIndex].anchoredPosition = new Vector2(x, comingSoon[snapIndex].anchoredPosition.y);
		
		for (int i = 0; i < bosses.Length; i++) {
			if (i != snapIndex) {
				bosses[i].anchoredPosition = new Vector2(x + (i - snapIndex) * BOSS_DISTANCE, bosses[i].anchoredPosition.y);
				shadows[i].anchoredPosition = new Vector2(bosses[i].anchoredPosition.x, shadows[i].anchoredPosition.y);
				comingSoon[i].anchoredPosition = new Vector2(bosses[i].anchoredPosition.x, comingSoon[i].anchoredPosition.y);
			}
		}
	}
	
	private void CompleteSnap() {
		currentBoss = snapIndex;
		tweening = false;
		AlignBosses();
		scrMenu.SelectBoss(profileIndexes[currentBoss]);
	}
	
	// Select boss immediately
	private void SelectProfileIndex(int profileIndex) {
		lastBossDisplaying = profileIndex;
		
		int bossIndex = 0;
		for (var i = 0; i < bosses.Length; i++) {
			if (profileIndexes[i] == profileIndex) {
				bossIndex = i;
				break;
			}
		}
		SelectRuntimeIndex(bossIndex);
	}
	
	private void SelectRuntimeIndex(int runtimeIndex) {
		bosses[runtimeIndex].anchoredPosition = new Vector2(0, bosses[runtimeIndex].anchoredPosition.y);
		shadows[runtimeIndex].anchoredPosition = new Vector2(0, shadows[runtimeIndex].anchoredPosition.y);
		comingSoon[runtimeIndex].anchoredPosition = new Vector2(0, comingSoon[runtimeIndex].anchoredPosition.y);
		
		for (int i = 0; i < bosses.Length; i++) {
			if (i != runtimeIndex) {
				bosses[i].anchoredPosition = new Vector2((i - runtimeIndex) * BOSS_DISTANCE, bosses[i].anchoredPosition.y);
				shadows[i].anchoredPosition = new Vector2(bosses[i].anchoredPosition.x, shadows[i].anchoredPosition.y);
				comingSoon[i].anchoredPosition = new Vector2(bosses[i].anchoredPosition.x, comingSoon[i].anchoredPosition.y);
			}
		}
	}
}
