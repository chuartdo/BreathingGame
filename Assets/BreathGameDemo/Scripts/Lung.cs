/**
Indictor with animated scale based on 
configure rete of pause between breath.,

Created by: Leon Hong Chu @chuartdo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lung : Balloon {

	bool breathing = true;

	public float inhalePauseTime = 1.5f;
	public float exhalePauseTime = 0.5f;
   
	void Start () {
		StartCoroutine(animateBreath());
	}
		
	void Update () {		 
		currentVolume = transform.localScale.magnitude;	 
	    transform.localScale *= 1+  (volume - currentVolume) * Time.deltaTime;		 
	}
		
	IEnumerator animateBreath() {
		while (breathing) {
			while (currentVolume <  capacity-0.4f) {
				volume = capacity ;
				yield return null;
			}
			SendMessageUpwards("CheckSync");

			yield return new WaitForSeconds (inhalePauseTime);
			SendMessageUpwards("CheckSync");
			while (currentVolume > 0.4f) {
				volume = -0.3f;
				yield return null;
			}
			SendMessageUpwards("CheckSync");

			yield return new WaitForSeconds (exhalePauseTime);
			SendMessageUpwards("CheckSync");

		}
	}
		
}




