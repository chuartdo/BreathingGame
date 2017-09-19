//  Created by: Leon Hong Chu @chuartdo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressureSensorUI : MonoBehaviour {
	public Text tPressure;
	public Slider uiSlider;

 	void Start () {
		
	}
	
	// Update UI Slider indicator. Show normalized pressure value between 1 and -1
	void Update () {
		float pressure = BleController.x1;
		if (pressure > uiSlider.maxValue)
			uiSlider.maxValue = pressure;
		if (pressure < uiSlider.minValue)
			uiSlider.minValue = pressure;
		uiSlider.value = pressure;
		tPressure.text = pressure.ToString();
	}


	public void changePressure(float value) {
		if (BleController.isActive())
			BleController.x1 = value;
	}

	static public bool ButtonPressed() {
		return (BleController.isActive() && (BleController.x1 > 0.4f));
	}
}
