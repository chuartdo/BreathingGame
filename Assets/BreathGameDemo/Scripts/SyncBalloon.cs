/**
Balloon with animated size indicator simulate expanding and contracting lung.
for player to follow.  
Configure rete of pause between breath.,

Created by: Leon Hong Chu @chuartdo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncBalloon : Balloon {

	bool breathing = true;

	public float inhalePauseTime = 1.5f;
	public float exhalePauseTime = 0.5f;
   
	void Start () {
		StartCoroutine(animateBreath());
	}

	void OnEnable() {
		StartCoroutine(animateBreath());
	}
		
	void Update () {		 
		currentVolume = transform.localScale.magnitude;	 
	    transform.localScale *= 1+  (volume - currentVolume) * Time.deltaTime;		 
	}
		
	IEnumerator animateBreath() {
		while (breathing) {
			while (currentVolume <  capacity-fillRate) {
				volume = capacity ;
				yield return null;
			}
			SendMessageUpwards("CheckSync");

			yield return new WaitForSeconds (inhalePauseTime);
			SendMessageUpwards("CheckSync");

			while (currentVolume > 0.4f) {
				volume = -deflateRate;
				yield return null;
			}

			SendMessageUpwards("CheckSync");

			yield return new WaitForSeconds (exhalePauseTime);
			SendMessageUpwards("CheckSync");

		}
	}
		
}




