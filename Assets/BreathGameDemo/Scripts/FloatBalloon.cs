// Ballon with fixed maximum volume. It will slowly deflate if no pressure supplied
// Balloon pops when reaching maximum voume.
//
//  Created by: Leon Hong Chu @chuartdo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBalloon : Balloon { 

	public bool canFloat = true;
	bool isPopped = false;
	float randomPos;
	float recycleHeight = 25f;
	float minCapacity;
 
	void Start () {
		randomPos = Random.Range(-4f,4f);
	     minCapacity = capacity * 1.5f ;
	}

	void Update () {
		if (!isPopped) {
			currentVolume = transform.localScale.magnitude;

			/*
			if (Input.GetMouseButtonDown(0))
				volume += 0.2f;
			*/

			// slowly deflate balloon if no continous pressure applied
			if (deflateRate > 0) { 
				if (  BleController.x1 > 0.1f)
					inflate( BleController.x1 * fillRate ) ; 

				if (volume < minCapacity && volume > 1f)
					deflate( deflateRate );
			} else
				inflate(  BleController.x1 * fillRate ) ; 

			volume = Mathf.Clamp (volume, 0.5f, minCapacity  );

			if (fillRate <=0)
				return;
			
			if ( canFloat) {   
				
				if (volume < minCapacity )
					transform.localScale *= 1+  (volume - currentVolume) * Time.deltaTime;
				else {
					isPopped = true;
					SendMessageUpwards ("createNewBalloon",SendMessageOptions.DontRequireReceiver);
					transform.parent = null;
	 			}

			} else {   
				if (currentVolume < minCapacity )
					transform.localScale *= 1+  (volume - currentVolume) * Time.deltaTime;
			}

		} else {  
			// Float the balloon in random direction and recycle after reaching height off screen
			transform.Translate(new Vector3(
				randomPos * 0.3f *Time.deltaTime,
				recycleHeight * 0.3f *Time.deltaTime, 
				0));
			if (transform.position.y > recycleHeight) {
				pop();
			}
		}

	}

	void pop() {
		ParticleSystem explosion = this.gameObject.GetComponentInChildren<ParticleSystem>();
		if (explosion != null)
			explosion.Play();
		this.gameObject.GetComponent<Renderer>().enabled = false;


		Destroy(gameObject,2);
	}


	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			pop();
			GameLogic.AddScore(3);
		}
	}


}
