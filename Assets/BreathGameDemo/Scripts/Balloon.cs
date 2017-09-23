using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

	public float capacity;
	public float fillRate;
	public float deflateRate;

	bool isPopped;

	protected float volume = 1;
	protected float currentVolume = 1;

	public void inflate (float amount) {
		volume += amount;
	}

	public void deflate(float amount) {
		volume -= amount;
	}

}
