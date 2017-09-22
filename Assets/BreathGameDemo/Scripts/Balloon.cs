using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

	public float capacity = 5;
	public float fillRate = 0.1f;
	public float deflateRate = 0.01f;

	bool isPopped = false;

	protected float volume = 1;
	protected float currentVolume = 1;

	public void inflate (float amount) {
		volume += amount;
	}

	public void deflate(float amount) {
		volume -= amount;
	}

}
