using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SCR_Background : MonoBehaviour {
	// Const
	private const int 	BACKGROUND_HEIGHT 			= 1920;
	private const int 	BACKGROUND_PART 			= 6;
	private const int 	FOREGROUND_PART 			= 2;
	
	
	private const int	LIGHTBAR_NUMBER 			= 40;

	// Prefab
	public GameObject 	PFB_Background				= null;
	public GameObject 	PFB_LightBar				= null;
	public GameObject 	PFB_Furniture				= null;
	
	public Sprite[]		SPR_Layer_Boss_1			= null;
	public Sprite[]		SPR_Layer_Boss_2			= null;
	public Sprite[]		SPR_Layer_Boss_3			= null;
	public Sprite[]		SPR_Layer_Boss_4			= null;
	public Sprite[]		SPR_Layer_Boss_5			= null;
	
	public Sprite[]		SPR_Layer_Trump_1			= null;
	public Sprite[]		SPR_Layer_Trump_2			= null;
	public Sprite[]		SPR_Layer_Trump_3			= null;
	public Sprite[]		SPR_Layer_Trump_4			= null;
	public Sprite[]		SPR_Layer_Trump_5			= null;
	
	public Sprite[]		SPR_Layer_Dictator_1		= null;
	public Sprite[]		SPR_Layer_Dictator_2		= null;
	public Sprite[]		SPR_Layer_Dictator_3		= null;
	public Sprite[]		SPR_Layer_Dictator_4		= null;
	public Sprite[]		SPR_Layer_Dictator_5		= null;
	
	
	public Sprite[]		SPR_Furniture				= null;
	
	// Object
	private GameObject[]	layer1					= null;
	private GameObject[]	layer2					= null;
	private GameObject[]	layer3					= null;
	private GameObject[]	layer4					= null;
	private GameObject[]	layer5					= null;
	
	private GameObject[]	lightBar				= null;
	private GameObject[] 	furniture				= null;
	
	// Offset
	private int 	layer_1_z						= 0;
	private int 	layer_2_z						= 0;
	private int 	layer_3_z						= 0;
	private int 	layer_4_z						= 0;
	private int 	layer_5_z						= 0;
	
	private float 	layer_1_scroll					= 0;
	private float 	layer_2_scroll					= 0;
	private float 	layer_3_scroll					= 0;
	private float 	layer_4_scroll					= 0;
	private float 	layer_5_scroll					= 0;
	
	private float 	layer_1_offset					= 0;
	private float 	layer_2_offset					= 0;
	private float 	layer_3_offset					= 0;
	private float 	layer_4_offset					= 0;
	private float 	layer_5_offset					= 0;
	
	// Instance
	public static SCR_Background	instance		= null;
	
	
	// Init
	private void Start () {
		instance = this;
		
		Sprite[] sprLayer1 = SPR_Layer_Boss_1;
		Sprite[] sprLayer2 = SPR_Layer_Boss_2;
		Sprite[] sprLayer3 = SPR_Layer_Boss_3;
		Sprite[] sprLayer4 = SPR_Layer_Boss_4;
		Sprite[] sprLayer5 = SPR_Layer_Boss_5;
		
		
		if (SCR_Profile.bossSelecting == (int)BossType.THE_BOSS || SCR_Profile.bossSelecting == (int)BossType.ERIX_LUKE) {
			// boss
			sprLayer1 = SPR_Layer_Boss_1;
			sprLayer2 = SPR_Layer_Boss_2;
			sprLayer3 = SPR_Layer_Boss_3;
			sprLayer4 = SPR_Layer_Boss_4;
			sprLayer5 = SPR_Layer_Boss_5;
			
			layer_1_offset = 800;
			layer_2_offset = 200;
			layer_3_offset = 80;
			layer_4_offset = -260;
			layer_5_offset = -150;
			
			layer_1_scroll = 0.02f;
			layer_2_scroll = 0.03f;
			layer_3_scroll = 0.05f;
			layer_4_scroll = 1.0f;
			layer_5_scroll = 1.0f;
			
			layer_1_z = 0;
			layer_2_z = -1;
			layer_3_z = -6;
			layer_4_z = -7;
			layer_5_z = -50;
			
			// Place furniture
			furniture = new GameObject[11];
			for (int i=0; i<furniture.Length; i++) {
				furniture[i] = Instantiate(PFB_Furniture);
			}
			// Fragment
			furniture[0].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[3];
			furniture[0].GetComponent<SCR_Furniture>().Spawn (-500, 1308);
			furniture[1].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[4];
			furniture[1].GetComponent<SCR_Furniture>().Spawn (-250, 1308);
			furniture[2].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[5];
			furniture[2].GetComponent<SCR_Furniture>().Spawn (0, 1308);
			furniture[3].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[6];
			furniture[3].GetComponent<SCR_Furniture>().Spawn (250, 1308);
			furniture[4].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[7];
			furniture[4].GetComponent<SCR_Furniture>().Spawn (500, 1308);
			// Desk, chair, and laptop
			furniture[5].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[0];
			furniture[5].GetComponent<SCR_Furniture>().Spawn (-450, 1470);
			furniture[6].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[1];
			furniture[6].GetComponent<SCR_Furniture>().Spawn (-150, 1470);
			furniture[7].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[0];
			furniture[7].GetComponent<SCR_Furniture>().Spawn (200, 1470);
			furniture[8].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[2];
			furniture[8].GetComponent<SCR_Furniture>().Spawn (450, 1470);
			furniture[9].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[8];
			furniture[9].GetComponent<SCR_Furniture>().Spawn (-450, 1570);
			furniture[10].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[8];
			furniture[10].GetComponent<SCR_Furniture>().Spawn (250, 1570);
		}
		else if (SCR_Profile.bossSelecting == (int)BossType.MR_PRESIDENT
		||       SCR_Profile.bossSelecting == (int)BossType.THE_GENERAL) {
			// Trump
			sprLayer1 = SPR_Layer_Trump_1;
			sprLayer2 = SPR_Layer_Trump_2;
			sprLayer3 = SPR_Layer_Trump_3;
			sprLayer4 = SPR_Layer_Trump_4;
			sprLayer5 = SPR_Layer_Trump_5;
			
			layer_1_offset = 500;
			layer_2_offset = -150;
			layer_3_offset = 130;
			layer_4_offset = 0;
			layer_5_offset = 0;
			
			layer_1_scroll = 0.02f;
			layer_2_scroll = 0.03f;
			layer_3_scroll = 0.25f;
			layer_4_scroll = 1.0f;
			layer_5_scroll = 1.0f;
			
			layer_1_z = 0;
			layer_2_z = -1;
			layer_3_z = -6;
			layer_4_z = -7;
			layer_5_z = -50;
			
			furniture = new GameObject[15];
			for (int i=0; i<furniture.Length; i++) {
				furniture[i] = Instantiate(PFB_Furniture);
			}
			// Branch
			furniture[0].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[9];
			furniture[0].GetComponent<SCR_Furniture>().Spawn (-500, 1600);
			furniture[1].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[10];
			furniture[1].GetComponent<SCR_Furniture>().Spawn (-250, 1600);
			furniture[2].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[11];
			furniture[2].GetComponent<SCR_Furniture>().Spawn (0, 1600);
			furniture[3].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[12];
			furniture[3].GetComponent<SCR_Furniture>().Spawn (250, 1600);
			furniture[11].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[9];
			furniture[11].GetComponent<SCR_Furniture>().Spawn (-500, 1600);
			furniture[12].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[10];
			furniture[12].GetComponent<SCR_Furniture>().Spawn (-250, 1600);
			furniture[13].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[11];
			furniture[13].GetComponent<SCR_Furniture>().Spawn (0, 1600);
			furniture[14].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[12];
			furniture[14].GetComponent<SCR_Furniture>().Spawn (250, 1600);
			// Leaf
			furniture[4].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[13];
			furniture[4].GetComponent<SCR_Furniture>().Spawn (500, 1600);
			furniture[5].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[13];
			furniture[5].GetComponent<SCR_Furniture>().Spawn (-450, 1600);
			furniture[6].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[13];
			furniture[6].GetComponent<SCR_Furniture>().Spawn (-150, 1600);
			furniture[7].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[13];
			furniture[7].GetComponent<SCR_Furniture>().Spawn (200, 1600);
			furniture[8].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[13];
			furniture[8].GetComponent<SCR_Furniture>().Spawn (450, 1600);
			furniture[9].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[14];
			furniture[9].GetComponent<SCR_Furniture>().Spawn (-450, 1600);
			furniture[10].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[14];
			furniture[10].GetComponent<SCR_Furniture>().Spawn (250, 1600);
		}
		else if (SCR_Profile.bossSelecting == (int)BossType.THE_DICTATOR) {
			// dictator
			sprLayer1 = SPR_Layer_Dictator_1;
			sprLayer2 = SPR_Layer_Dictator_2;
			sprLayer3 = SPR_Layer_Dictator_3;
			sprLayer4 = SPR_Layer_Dictator_4;
			sprLayer5 = SPR_Layer_Dictator_5;
			
			layer_1_offset = 800;
			layer_2_offset = 200;
			layer_3_offset = 80;
			layer_4_offset = 0;
			layer_5_offset = -150;
			
			layer_1_scroll = 0.02f;
			layer_2_scroll = 0.03f;
			layer_3_scroll = 0.05f;
			layer_4_scroll = 1.0f;
			layer_5_scroll = 1.0f;
			
			layer_1_z = 0;
			layer_2_z = -1;
			layer_3_z = -6;
			layer_4_z = -7;
			layer_5_z = -50;
			
			// Place furniture
			furniture = new GameObject[11];
			for (int i=0; i<furniture.Length; i++) {
				furniture[i] = Instantiate(PFB_Furniture);
			}
			// Fragment
			furniture[0].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[15];
			furniture[0].GetComponent<SCR_Furniture>().Spawn (-500, 1310);
			furniture[1].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[16];
			furniture[1].GetComponent<SCR_Furniture>().Spawn (-250, 1310);
			furniture[2].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[17];
			furniture[2].GetComponent<SCR_Furniture>().Spawn (0, 1310);
			furniture[3].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[18];
			furniture[3].GetComponent<SCR_Furniture>().Spawn (250, 1310);
			furniture[4].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[19];
			furniture[4].GetComponent<SCR_Furniture>().Spawn (500, 1310);
			// Flower pot
			furniture[5].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[20];
			furniture[5].GetComponent<SCR_Furniture>().Spawn (-496, 1438);
			furniture[6].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[20];
			furniture[6].GetComponent<SCR_Furniture>().Spawn (-278, 1438);
			furniture[7].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[20];
			furniture[7].GetComponent<SCR_Furniture>().Spawn (-68, 1438);
			furniture[8].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[20];
			furniture[8].GetComponent<SCR_Furniture>().Spawn (109, 1438);
			furniture[9].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[20];
			furniture[9].GetComponent<SCR_Furniture>().Spawn (314, 1438);
			furniture[10].GetComponent<SpriteRenderer>().sprite = SPR_Furniture[20];
			furniture[10].GetComponent<SCR_Furniture>().Spawn (526, 1438);
		}
		
		layer1 = new GameObject[sprLayer1.Length];
		for (var i=0; i<sprLayer1.Length; i++) {
			layer1[i] = Instantiate (PFB_Background);
			layer1[i].GetComponent<SpriteRenderer>().sprite = sprLayer1[i];
			layer1[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		}
		
		layer2 = new GameObject[sprLayer2.Length];	
		for (var i=0; i<sprLayer2.Length; i++) {
			layer2[i] = Instantiate (PFB_Background);
			layer2[i].GetComponent<SpriteRenderer>().sprite = sprLayer2[i];
			layer2[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		}
		
		layer3 = new GameObject[sprLayer3.Length];
		for (var i=0; i<sprLayer3.Length; i++) {
			layer3[i] = Instantiate (PFB_Background);
			layer3[i].GetComponent<SpriteRenderer>().sprite = sprLayer3[i];
			layer3[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		}
		
		layer4 = new GameObject[sprLayer4.Length];
		for (var i=0; i<sprLayer4.Length; i++) {
			layer4[i] = Instantiate (PFB_Background);
			layer4[i].GetComponent<SpriteRenderer>().sprite = sprLayer4[i];
			layer4[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		}
		
		layer5 = new GameObject[sprLayer5.Length];
		for (var i=0; i<sprLayer5.Length; i++) {
			layer5[i] = Instantiate (PFB_Background);
			layer5[i].GetComponent<SpriteRenderer>().sprite = sprLayer5[i];
			layer5[i].transform.localScale = new Vector3 (SCR_Gameplay.SCREEN_SCALE, SCR_Gameplay.SCREEN_SCALE, 1);
		}
		
		
		lightBar = new GameObject[LIGHTBAR_NUMBER];
		for (int i=0; i<LIGHTBAR_NUMBER; i++) {
			lightBar[i] = Instantiate (PFB_LightBar);
		}
		
		SetCameraY (0);
	}
	
	
	public static void BreakFurniture (float x, float y, float force) {
		for (int i=0; i<instance.furniture.Length; i++) {
			instance.furniture[i].GetComponent<SCR_Furniture>().Break (x, y, force);
		}
		
		if (SCR_Profile.bossSelecting == (int)BossType.THE_BOSS || SCR_Profile.bossSelecting == (int)BossType.THE_DICTATOR) {
			SCR_Audio.PlayBreakSound(0); // Furniture breaking
		}
		else if (SCR_Profile.bossSelecting == (int)BossType.MR_PRESIDENT
		||       SCR_Profile.bossSelecting == (int)BossType.THE_GENERAL
		||       SCR_Profile.bossSelecting == (int)BossType.ERIX_LUKE) {
			SCR_Audio.PlayBreakSound(1); // Leaf break
		}
	}
	// Set camera position
	public static void SetCameraY (float cameraY) {
		float layer1Y = cameraY * instance.layer_1_scroll - instance.layer_1_offset;
		for (int i=0; i<instance.layer1.Length; i++) {
			instance.layer1[i].SetActive (false);
		}
		if (layer1Y < 0) {
			instance.layer1[0].SetActive (true);
			instance.layer1[0].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer1Y + cameraY, instance.layer_1_z);
		}
					
		if (layer1Y < (instance.layer1.Length-1) * BACKGROUND_HEIGHT) {
			for (int i=0; i<instance.layer1.Length-1; i++) {
				if (layer1Y >= i * BACKGROUND_HEIGHT && layer1Y < (i + 1) * BACKGROUND_HEIGHT) {
					instance.layer1[i + 0].SetActive (true);
					instance.layer1[i + 1].SetActive (true);
					
					layer1Y = layer1Y % BACKGROUND_HEIGHT;
					instance.layer1[i + 0].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer1Y + cameraY, instance.layer_1_z);
					instance.layer1[i + 1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer1Y + cameraY + BACKGROUND_HEIGHT, instance.layer_1_z);
				}
			}
		}
		else {
			layer1Y = layer1Y % BACKGROUND_HEIGHT;
			instance.layer1[instance.layer1.Length-2].SetActive (true);
			instance.layer1[instance.layer1.Length-1].SetActive (true);
			instance.layer1[instance.layer1.Length-2].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer1Y + cameraY, instance.layer_1_z);
			instance.layer1[instance.layer1.Length-1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer1Y + cameraY + BACKGROUND_HEIGHT, instance.layer_1_z);
		}
		
		
		float layer2Y = cameraY * instance.layer_2_scroll - instance.layer_2_offset;
		for (int i=0; i<instance.layer2.Length; i++) {
			instance.layer2[i].SetActive (false);
		}
		if (layer2Y < 0) {
			instance.layer2[0].SetActive (true);
			instance.layer2[0].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer2Y + cameraY, instance.layer_2_z);
		}
		else {
			for (int i=0; i<instance.layer2.Length; i++) {
				if (layer2Y >= i * BACKGROUND_HEIGHT && layer2Y < (i + 1) * BACKGROUND_HEIGHT) {
					layer2Y = layer2Y % BACKGROUND_HEIGHT;
					instance.layer2[i].SetActive (true);
					instance.layer2[i].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer2Y + cameraY, instance.layer_2_z);
					
					if (i < instance.layer2.Length - 1) {
						instance.layer2[i + 1].SetActive (true);
						instance.layer2[i + 1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer2Y + cameraY + BACKGROUND_HEIGHT, instance.layer_2_z);
					}
				}
			}
		}
		
		float layer3Y = cameraY * instance.layer_3_scroll - instance.layer_3_offset;
		for (int i=0; i<instance.layer3.Length; i++) {
			instance.layer3[i].SetActive (false);
		}
		if (layer3Y < 0) {
			instance.layer3[0].SetActive (true);
			instance.layer3[0].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer3Y + cameraY, instance.layer_3_z);
		}
		else {
			for (int i=0; i<instance.layer3.Length; i++) {
				if (layer3Y >= i * BACKGROUND_HEIGHT && layer3Y < (i + 1) * BACKGROUND_HEIGHT) {
					layer3Y = layer3Y % BACKGROUND_HEIGHT;
					instance.layer3[i].SetActive (true);
					instance.layer3[i].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer3Y + cameraY, instance.layer_3_z);
					
					if (i < instance.layer3.Length - 1) {
						instance.layer3[i + 1].SetActive (true);
						instance.layer3[i + 1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer3Y + cameraY + BACKGROUND_HEIGHT, instance.layer_3_z);
					}
				}
			}
		}
		
		float layer4Y = cameraY * instance.layer_4_scroll - instance.layer_4_offset;
		for (int i=0; i<instance.layer4.Length; i++) {
			instance.layer4[i].SetActive (false);
		}
		if (layer4Y < 0) {
			instance.layer4[0].SetActive (true);
			instance.layer4[0].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer4Y + cameraY, instance.layer_4_z);
		}
		else {
			for (int i=0; i<instance.layer4.Length; i++) {
				if (layer4Y >= i * BACKGROUND_HEIGHT && layer4Y < (i + 1) * BACKGROUND_HEIGHT) {
					layer4Y = layer4Y % BACKGROUND_HEIGHT;
					instance.layer4[i].SetActive (true);
					instance.layer4[i].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer4Y + cameraY, instance.layer_4_z);
					
					if (i < instance.layer4.Length - 1) {
						instance.layer4[i + 1].SetActive (true);
						instance.layer4[i + 1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer4Y + cameraY + BACKGROUND_HEIGHT, instance.layer_4_z);
					}
				}
			}
		}
		
		float layer5Y = cameraY * instance.layer_5_scroll - instance.layer_5_offset;
		for (int i=0; i<instance.layer5.Length; i++) {
			instance.layer5[i].SetActive (false);
		}
		if (layer5Y < 0) {
			instance.layer5[0].SetActive (true);
			instance.layer5[0].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer5Y + cameraY, instance.layer_5_z);
		}
		else {
			for (int i=0; i<instance.layer5.Length; i++) {
				if (layer5Y >= i * BACKGROUND_HEIGHT && layer5Y < (i + 1) * BACKGROUND_HEIGHT) {
					layer5Y = layer5Y % BACKGROUND_HEIGHT;
					instance.layer5[i].SetActive (true);
					instance.layer5[i].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer5Y + cameraY, instance.layer_5_z);
					
					if (i < instance.layer5.Length - 1) {
						instance.layer5[i + 1].SetActive (true);
						instance.layer5[i + 1].transform.position = new Vector3 (SCR_Gameplay.SCREEN_W * 0.5f, -layer5Y + cameraY + BACKGROUND_HEIGHT, instance.layer_5_z);
					}
				}
			}
		}
	}
	

	private void Update () {
		
	}
	
}
