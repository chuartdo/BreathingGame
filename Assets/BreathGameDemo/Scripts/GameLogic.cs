using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
	GameObject menu;
	public GameObject bonusGame;
	// Use this for initialization
	bool waitForInput = true;
	public static bool isBonusGame = false;
    static int score = 0;

	GameObject player, lung;
	public Text scoreUI;
	public float SyncTolerance = 0.3f;
	static GameLogic _instance;

	void Start () {
		menu = GameObject.Find ("StartMenu");
		player = GameObject.FindGameObjectWithTag("Player");
		lung = GameObject.Find("Lung");
		if (_instance == null)
			_instance = this;
	}
	
	void Update () {
		if (waitForInput)  
			if (PressureSensorUI.ButtonPressed ()) {
				waitForInput = false;
				StartGame ();
			}

	}

	public void StartGame() {
		// Hide menu
		if (menu != null)
			menu.SetActive (false);
	}

	// Sync Game logic
		
	public void CheckSync() {
		float difference = 
			lung.transform.localScale.x - player.transform.localScale.x;
		
		if (Mathf.Abs(difference) < 0.3) {
			AddScore(1);
		}
	  // no points

	}

	// UI functions

	static public void AddScore(int value) {
		score += value;
		if (_instance.scoreUI != null)
			_instance.scoreUI.text = score.ToString();

		if ( score > 5) {
			_instance.activateBonusGame(true);
		}
		if (score > 15) {
			_instance.activateBonusGame(false);

		}
			
	}

	public void WaitInput() {
		waitForInput = true;
	}

	void activateBonusGame(bool active) {
	    isBonusGame = active;
		if (bonusGame != null)
			bonusGame.SetActive(active);
		player.SetActive(!active);
		lung.SetActive(!active);
	}

}
