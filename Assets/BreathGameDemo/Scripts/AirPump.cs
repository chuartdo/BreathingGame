//  Pump fills ballon with random color with air flow based on pressure value 
//  Created by: Leon Hong Chu @chuartdo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirPump : MonoBehaviour {
	public GameObject pumpItem;
  
    Color[] balloonColor = { Color.blue, Color.yellow, Color.red, Color.green, Color.cyan, Color.magenta }; 

	 
	void Start () {
		createNewBalloon();
	}

	public void createNewBalloon() {
		GameLogic.AddScore(1);
		createBalloon();
	}

	public virtual GameObject createBalloon() {
		GameObject balloon = Instantiate(pumpItem,transform.position,transform.rotation);
		balloon.transform.parent = transform;
		balloon.GetComponent<Renderer>().material.color = balloonColor[Random.Range(0,5)];
		balloon.GetComponent<Balloon>().capacity= Random.Range(5,8);
		return balloon;
	}

}
