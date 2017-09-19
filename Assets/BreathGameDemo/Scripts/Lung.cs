// Ballon with fixed maximum volume. It will slowly deflate if no pressure supplied
// Balloon pops when reaching maximum voume.
//
//  Created by: Leon Hong Chu @chuartdo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lung : MonoBehaviour {

	bool breathing = true;

	public float inhalePauseTime = 1.5f;
	public float exhalePauseTime = 0.5f;

	public float capacity = 5;
	public float fillRate = 0.1f;
	public float deflateRate = 0.01f;

	bool isPopped = false;
	float volume = 1;
	float currentVolume = 1;

	float randomPos;

	public void inflate (float amount) {
		volume += amount;
	}

	public void deflate(float amount) {
		volume -= amount;
	}

	void Start () {
		//randomPos = Random.Range(-4f,4f);
		StartCoroutine(animateBreath());
	}

	float offsetDistance = 15f;
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
			yield return new WaitForSeconds (inhalePauseTime);
			while (currentVolume > 0.4f) {
				volume = -0.3f;
				yield return null;
			}
			yield return new WaitForSeconds (exhalePauseTime);

		}
	}




	}




