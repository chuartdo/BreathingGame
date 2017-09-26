// Expose pressure value as static x1 from Bluetooth pressure sensor 
// In Unity Editor mode, mouse wheel simulate pressure value
//  Created by: Leon Hong Chu @chuartdo

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class BleController : MonoBehaviour
{
	const int ButtonCount = 4;

    private AndroidJavaObject plugin;
    private bool refresh = false;
    
 
	static BleController _instance = null;

	static public float x1;

	static bool[] buttonStates = new bool[ButtonCount];
	static bool[] prevButtonState = new bool[ButtonCount];


	static public bool isActive() {
		return _instance.refresh;
	}		

	public bool debug = false;

    void Start()
    {
		if (_instance == null) 
			_instance = this;

#if UNITY_ANDROID
		try {
			plugin  = new AndroidJavaClass("com.chuart.breathgame.SensorPlugin").CallStatic<AndroidJavaObject>("getInstance");
			DebugText.show ("Plugin loaded");
			plugin.Call("connectBLEController", "connect");
			DebugText.show ("Waiting connection ..");
 		} catch (Exception e) {
			DebugText.show("Error init Class");
 		}
		if (debug)
			refresh = true;
 #else
		refresh = true;

 #endif
		 
    }

	// Call back invoked by the Java Plugin.  
	public void BleStatus(String status = null)
    {
		if (status != null) {
			DebugText.show ("Ble status: " + status);

			if (status.StartsWith("ready"))
        		refresh = true;
			else 
				refresh = false;
		}
    }
	 
    void OnApplicationQuit()
    {
#if UNITY_ANDROID
        if (plugin != null)
        {
            plugin.Call("disconnect");
            plugin = null;
        }
#endif
    }

    void Update()
    {
        if (!refresh)
            return;

		for (int i=0; i< ButtonCount; i++)
			prevButtonState[i] = buttonStates[i];
		
#if UNITY_ANDROID
        if (plugin != null)
        {
			x1 = (float)  plugin.Call<double>("getPressure");
			DebugText.show ("Ps: "+x1 ,2);

		} else 
#endif
		{   // For game testing in Editor without controller
			float val = Input.GetAxis("Mouse ScrollWheel");
 			x1 = Mathf.Clamp(val+x1,-1f,1f);
		}
    }

	public static bool GetButtonDown(int index) {
		return buttonStates[index] && (buttonStates[index] != prevButtonState[index]) ;
	}

	public static bool GetButtonUp(int index) {
		return !buttonStates[index] && (buttonStates[index] != prevButtonState[index])  ;
	}

		
  static bool[] relayState = new bool[8];

  static public void ActivateRelay(int id, Boolean isOn) {
		Debug.Log("Relay "+id + "  " + (isOn?"ON":"OFF"));

		// send state change command only if status has changed.
		if (relayState[id] == isOn)
			return;
		
		relayState[id] = isOn;

	}

}