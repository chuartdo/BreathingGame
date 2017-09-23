using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
	GameObject menu;
	public GameObject bonusGame;
 	bool waitForInput = true;
	public static bool isBonusGame = false;
    static int score = 0;

	GameObject player, syncObject;
	public Text scoreUI;
	public float SyncTolerance = 0.3f;
	static GameLogic _instance;


	void Awake() {
		if (_instance == null)
			_instance = this;
	}

	void Start () {
		menu = GameObject.Find ("StartMenu");
		player = GameObject.FindGameObjectWithTag("Player");
		syncObject = GameObject.Find("SyncBalloon");
		if (bonusGame != null)
			bonusGame.SetActive(false);
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
		
	//Check if player matches the approxmimate size of the reference balloon at each check sync point
	public void CheckSync() {
		float difference = 
			syncObject.transform.localScale.x - player.transform.localScale.x;
		
		if (Mathf.Abs(difference) < 0.3) {
			AddScore(1);
		}
 
	}

	// UI functions

	static public void AddScore(int value) {
		_instance.incScore(value);
			
	}

	void incScore(int value) {
		score += value;
		if (scoreUI != null)
			scoreUI.text = score.ToString();

		if ( score > 5) {
			if ( remainingTime <= 0) {
				// toggle game modes
				isBonusGame=!isBonusGame;
				ActivateBonusGame(isBonusGame);
				StartCoroutine(Countdown(isBonusGame?10f:30f));
			}
		}

	}

	float remainingTime;
	public IEnumerator Countdown(float countdownSeconds = 10f)
	{
		remainingTime = countdownSeconds;
		while (remainingTime > 0)
		{
			DebugText.show("Remain: " + remainingTime);
			yield return new WaitForSeconds(1.0f);
			remainingTime--;
		}
		// toggle game mode

	}

	public void WaitInput() {
		waitForInput = true;
	}

	void ActivateBonusGame(bool active) {
 
		// disable sync game
		player.SetActive(!active);
		syncObject.SetActive(!active);
	

		if (bonusGame != null)
			bonusGame.SetActive(active);
	}
	

}
