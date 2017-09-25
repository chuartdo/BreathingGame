using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartMotion: MonoBehaviour {
	public float thrust;
//	public GameObject target;
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
		Shoot();
	}

	void Shoot()
	{ 
	//	Vector3 shoot = (target.transform.position - transform.position).normalized;
		rb.AddForce(transform.forward * thrust  * BleController.x1);
	}


	void OnCollisionEnter(Collision collision) {
		if (tag == "Recycle") {
			if (collision.relativeVelocity.magnitude > 2)
				Destroy(this.gameObject);
		}
	}


}
