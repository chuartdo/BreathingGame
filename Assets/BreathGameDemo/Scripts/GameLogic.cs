using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
	GameObject menu;
	// Use this for initialization
	bool waitForInput = true;

	void Start () {
		menu = GameObject.Find ("StartMenu");
	}
	
	// Update is called once per frame
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
}
