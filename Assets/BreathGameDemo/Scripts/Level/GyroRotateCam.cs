using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
public class GyroRotateCam : MonoBehaviour {

		void Start ()  {
			Input.gyro.enabled = true;
		}

		void Update ()  {
		transform.Rotate (Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);
		}
}

#endif