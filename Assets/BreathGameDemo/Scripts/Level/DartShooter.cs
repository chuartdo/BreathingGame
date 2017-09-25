//  Pump fills ballon with random color with air flow based on pressure value 
//  Created by: Leon Hong Chu @chuartdo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartShooter : MonoBehaviour {
	public GameObject dartPrefab;
	bool isLoaded = false;

    Color[] dartColor = { Color.blue, Color.yellow, Color.red, Color.green, Color.cyan, Color.magenta }; 

	void Start () {
		 
	}

	void Update() {
		if (BleController.x1 < -0.1f)
			isLoaded = true;
		if(Input.GetMouseButtonDown(0)) {
			ShootDart();
		}
		#if UNITY_ANDROID

		#else


		#endif
	}

	public void ShootDart() {
		if (isLoaded) {
	 		GameObject dart = Instantiate(dartPrefab,transform.position,transform.rotation);
			dart.transform.parent = null;
			dart.GetComponent<Renderer>().material.color = dartColor[Random.Range(0,5)];
			isLoaded = false;
		}
 	}

}
