using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
	GameObject menu;
	public GameObject bonusGame;

	public static bool isBonusGame = false;
	public Text scoreUI;
	public float SyncTolerance = 0.3f;
	public float BreathSyncGameTime = 30f;
	public float BlowBalloonGameTime = 10f;


	public enum Game {SyncBreath=0, BlowBalloon=1, ShootBallon=2};
	public int gameMode = 1;

    static int score = 0;
	bool waitForInput = true;
	GameObject player, syncObject;

	static GameLogic _instance;

	void Awake() {
		if (_instance == null)
			_instance = this;
	}

	void Start () {
		menu = GameObject.Find ("StartMenu");
		player = GameObject.Find("PlayerBalloon");
		syncObject = GameObject.Find("SyncBalloon");
		if (bonusGame != null)
			bonusGame.SetActive(false);
		ActivateBonusGame((Game)gameMode);
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

//		if ( score > 5) {
			if ( remainingTime <= 0) {
				CycleThroughhGames();
				
			}
//		}

	}

	float remainingTime;
	public IEnumerator CountDown(float countdownSeconds = 10f)
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


	void CycleThroughhGames() {
 		if ((int)gameMode > 2)
			gameMode = 0;
		else 
			gameMode += 1;
		ActivateBonusGame( (Game)gameMode  ) ;

	}

	// Activate / deactivate different game items based on game mode
	void ActivateBonusGame(Game gameMode) {

	
		float gameTime = 10f;

		switch (gameMode) {

			case Game.SyncBreath:
				player.SetActive(true);
				syncObject.SetActive(true);
				bonusGame.SetActive(false);
				gameTime = BlowBalloonGameTime;
				break;
			
			case Game.BlowBalloon:
				player.SetActive(false);
				syncObject.SetActive(false);
				bonusGame.SetActive(true);
				gameTime = BlowBalloonGameTime;
				break;

			case Game.ShootBallon:
				player.SetActive(false);
				syncObject.SetActive(false);
			    bonusGame.SetActive(true);
				break;
		}
			
		StartCoroutine(CountDown( gameTime )); 

	}
	

}
