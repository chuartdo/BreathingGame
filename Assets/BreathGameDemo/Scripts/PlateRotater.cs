using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateRotater : MonoBehaviour {
	public float rotationRate = 100;
	private float lastVal = 0;
	bool isLoaded = false;

	void Start () {
	}

	void Update () {
		if (!isLoaded) {
			if (BleController.x1 < -0.1f) {
				float yAxis = transform.localRotation.y + BleController.x1 *  rotationRate;
				if (yAxis < lastVal)
					transform.localRotation  = Quaternion.Euler(new Vector3(0, lastVal + yAxis,0));
				lastVal = yAxis;
			}	 
		} 
	}
}
