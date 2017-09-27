using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
	public GameObject fillBalloon;
	public GameObject shootBalloon;

	public static bool isBonusGame = false;
	public Text scoreUI;
	public float SyncTolerance = 0.3f;
	public float BreathSyncGameTime = 30f;
	public float FillBalloonGameTime = 30f;
	public float ShootBalloonGameTime = 30f;


	public enum Game {SyncBreath=0, FillBalloon=1, ShootBallon=2};

    static int score = 0;
	bool waitForInput = true;
	bool isGameActive = false;

	public GameObject playerCamera, playerBall, syncBall;

	public int gameMode = 1;

	GameObject menu,shooter;

	static GameLogic _instance;
	Vector3 originalPlayerPosition;

	void Awake() {
		if (_instance == null)
			_instance = this;
	}

	void Start () {
		menu = GameObject.Find ("StartMenu");
		shooter = GameObject.Find("Shooter");
		// disable all game mode objects
		syncBall.SetActive(true);
		fillBalloon.SetActive(false);
		shootBalloon.SetActive(false);

		originalPlayerPosition = playerCamera.transform.position;

	}
	
	void Update () {
		if (waitForInput)  
			if (PressureSensorUI.ButtonPressed ()) {
				waitForInput = false;
				StartGame ();
			}

		if ( isGameActive && remainingTime <= 0) {
			CycleThroughhGames();
		}
	}

	public void StartGame() {
		// Hide menu
		if (menu != null)
			menu.SetActive (false);
		isGameActive = true;
		ActivateBonusGame((Game)gameMode);

	}

	// Sync Game logic
		
	//Check if player matches the approxmimate size of the reference balloon at each check sync point
	public void CheckSync() {
		if (!isGameActive)
			return;
		float difference = 
			syncBall.transform.localScale.x - playerBall.transform.localScale.x;
		
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
	}

	float remainingTime;
	public IEnumerator CountDown(float countdownSeconds = 10f)
	{
		remainingTime = countdownSeconds;
		while (remainingTime > 0)
		{
			DebugText.show("Time: " + remainingTime);
			yield return new WaitForSeconds(1.0f);
			remainingTime--;
		}
		// toggle game mode

	}

	public void WaitInput() {
		waitForInput = true;
	}


	void CycleThroughhGames() {
		if ((int)gameMode > 2 || (int)gameMode < 0)
			gameMode = 0;
		 
		ActivateBonusGame( (Game)gameMode  ) ;
		gameMode += 1;

	}

	// Activate / deactivate different game items based on game mode
	void ActivateBonusGame(Game gameMode) {

		float gameTime = 10f;
		string gameTitle = "";
		playerCamera.transform.position = originalPlayerPosition;

		switch (gameMode) {

			case Game.SyncBreath:
				playerBall.SetActive(true);
				syncBall.SetActive(true);
				fillBalloon.SetActive(false);
				shootBalloon.SetActive(false);
				gameTime = BreathSyncGameTime;
				gameTitle = "Sync Breath";
				shooter.SetActive(false);

				break;
			
			case Game.FillBalloon:
				playerBall.SetActive(false);
				syncBall.SetActive(false);
				fillBalloon.SetActive(true);
				shootBalloon.SetActive(false);
				gameTime = FillBalloonGameTime;
				gameTitle = "Fill Balloon";
				shooter.SetActive(false);

				break;

			case Game.ShootBallon:
				playerCamera.transform.Translate(new Vector3(0,7,0));
				playerBall.SetActive(false);
				syncBall.SetActive(false);
			    fillBalloon.SetActive(false);
				shootBalloon.SetActive(true);
				gameTitle = "Shoot Balloon";
				gameTime = FillBalloonGameTime;
				shooter.SetActive(true);
				break;

		}
			
		StartCoroutine(CountDown( gameTime )); 
		DebugText.show(gameTitle,1);
	}
	

}
