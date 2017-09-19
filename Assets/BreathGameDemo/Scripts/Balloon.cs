// Ballon with fixed maximum volume. It will slowly deflate if no pressure supplied
// Balloon pops when reaching maximum voume.
//
//  Created by: Leon Hong Chu @chuartdo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour { 
	
	public float capacity = 5;
	public float fillRate = 0.1f;
	public float deflateRate = 0.01f;
	public bool canFloat = true;

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
		randomPos = Random.Range(-4f,4f);
	}

	float offsetDistance = 15f;
	void Update () {
		if (!isPopped) {
			currentVolume = transform.localScale.magnitude;
			float minCapacity = capacity * 2;

			/*
			if (Input.GetMouseButtonDown(0))
				volume += 0.2f;
			*/

			// slowly deflate balloon if no continous pressure applied
			if (deflateRate > 0) { 
				if (  BleController.x1 > 0.1f)
					volume +=  BleController.x1 * fillRate ; //rate

				if (volume < minCapacity && volume > 1f)
					volume -= deflateRate;
			} else
				volume +=  BleController.x1 * fillRate ; 

			volume = Mathf.Clamp (volume, 0.5f, minCapacity);

			if (volume < minCapacity )
				transform.localScale *= 1+  (volume - currentVolume) * Time.deltaTime;
			else {
				if (canFloat) {
					isPopped = true;
					SendMessageUpwards ("createNewBalloon");
					transform.parent = null;
				}
			}

		} else {
			transform.Translate(new Vector3(
				randomPos * 0.3f *Time.deltaTime,
				offsetDistance * 0.3f *Time.deltaTime, 
				0));
			if (transform.position.y > offsetDistance) {
				Destroy(gameObject);
			}
		}
			
	}
}
