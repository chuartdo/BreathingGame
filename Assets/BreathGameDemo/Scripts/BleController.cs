//  Created by: Leon Hong Chu @chuartdo

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

// Include this script inside gameObject named "BleController" for proper
// Callback when Ble connection is established

public class BleController : MonoBehaviour
{
	const int ButtonCount = 4;

    private AndroidJavaObject plugin;
    private bool refresh = false;
    
 
	static BleController _instance = null;

	static public float x1;
	static public float y1;
	static public float x2,y2;
	static public bool b1,b2,b3,b4;
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
		//Invoke("BleStatus", 2f);
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
		//	byte[] temp = new byte[20];

			// Convert serial bytes to Joystick axis and buttons controls 
			/*
			if (!BitConverter.IsLittleEndian) {  
				// Reverse byte order
				Array.Copy(recvBuff, temp, recvBuff.Length);
				Array.Reverse(temp, 0 ,16);
				//y2 = BitConverter.ToSingle(temp,0);
				//x2 = BitConverter.ToSingle(temp,4);
				//y1 = BitConverter.ToSingle(temp,8);
				x1 = (float) BitConverter.ToDouble(temp,8);

			} else { 
				x1 = (float) BitConverter.ToDouble(recvBuff,0);
				//y1 = BitConverter.ToSingle(recvBuff,4);
				x2 = BitConverter.ToSingle(recvBuff,8);
				y2 = BitConverter.ToSingle(recvBuff,12);
 
			}
			*/
				
			DebugText.show ("Ps: "+x1 ,1);

		} else 
#endif
		{   // For game testing in Editor without controller
			b1 = Input.GetButton("Fire1");
			b2 = Input.GetButton("Fire2");
			b3 = Input.GetButton("Fire3");
			b4 = Input.GetButton("Jump");
		//	x1 = Input.GetAxis("Horizontal");
			y1 = Input.GetAxis("Vertical");
			x2 = Input.GetAxis("RJoyX") ;
			y2 = Input.GetAxis("RJoyY") ;
		}
		buttonStates[0] = b1;
		buttonStates[1] = b2;
		buttonStates[2] = b3;
		buttonStates[3] = b4;
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