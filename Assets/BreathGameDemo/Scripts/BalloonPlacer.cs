//  Pump fills ballon with random color with air flow based on pressure value 
//  Created by: Leon Hong Chu @chuartdo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonPlacer : AirPump {
	public GameObject spawnLoc;
	public Vector3 randomOffset;
 
 	public int maxCount;
	 
	void Start () {

		// calculate how many current balloons 
		int count = transform.childCount;
		if (randomOffset == Vector3.zero) {
			randomOffset = new Vector3(-10,-4,-3);
		}

		for (int i=count; i<maxCount; i++) {
			GameObject balloon = createBalloon();
			balloon.transform.parent = spawnLoc.transform;
			balloon.transform.Translate(
				new Vector3( Random.Range(-randomOffset.x,randomOffset.x),
					i * 10f/maxCount  + Random.Range(-randomOffset.y,randomOffset.y),
					i * 50f/maxCount + Random.Range(-randomOffset.z,randomOffset.z)
				)
			);
		}
 	}

	 

}
