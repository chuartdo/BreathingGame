package com.chuart.breathgame;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.Context;

import com.infineon.sen.comm.Model.Mode;
import com.infineon.sen.comm.Model.Sensor;
import com.infineon.sen.comm.SensorEvent;
import com.infineon.sen.comm.SensorHub;
import com.infineon.sen.comm.SensorHubListener;
import com.infineon.sen.util.Log;
import com.unity3d.player.UnityPlayer;

import java.util.HashSet;
import java.util.Set;

/**
 * Expose Java classes for Unity to access pressure value from Infineon Sensor Hub Bluetooth Evaluation kit
 *
 */

public final class SensorPlugin {
    static SensorHub mSensorHub;
    static final String TAG = "BreathGame";

    Context context;

    public static SensorPlugin getInstance() {

        SensorPlugin plugin = instance;
        if (plugin == null) {
            synchronized (SINGLETON_LOCK) {
                plugin = instance;
                if (plugin == null) {
                    dataBuffer = new byte[20];
                    plugin = new SensorPlugin();

                    BluetoothDevice sensor = bluetoothSetup();
                    if (sensor == null) {
                        return null;
                    }

                    setupSensorHub(sensor);

                    instance = plugin;
                }
            }
        }
        return plugin;
    }

    static double roomPressure = 0;
    static boolean calibration = true;
    static int calibCount = 0;
    static double sumSamples = 0;

    private static void setupSensorHub(BluetoothDevice sensor) {
        mSensorHub = new SensorHub(UnityPlayer.currentActivity.getApplicationContext(), sensor.getAddress());
        mSensorHub.addSensorHubListener(new SensorHubListener() {
            @Override
            public void onConnected(SensorHub s) {
                Log.d(TAG, "Connected");
                UnityPlayer.UnitySendMessage("BleController", "BleStatus", "connected");
                sensorSetting();
                mSensorHub.start();
            }

            @Override
            public void onDisconnected(SensorHub s) {
                Log.d(TAG, "Disconnected");
            }

            @Override
            public void onConnectionError(SensorHub s) {
                Log.d(TAG, "Connection Error");
                UnityPlayer.UnitySendMessage("BleController", "BleStatus", "failed");
            }

            @Override
            public void onSensorDataReceived(SensorHub s, SensorEvent e) {
                if (!e.getDataId().equals("p"))
                    return;

                if (calibration) {
                    if (calibCount++ < 500f) {
                        if (calibCount % 5 == 0 ) {
                            sumSamples += e.getSensorValue();
                        }
                    }
                    else {
                        roomPressure = sumSamples / 100f;
                        calibration = false;
                        UnityPlayer.UnitySendMessage("BleController", "BleStatus", "ready");
                    }
                }
                pressureValue = e.getSensorValue() - roomPressure;

            }
            @Override
            public void onModeChanged(SensorHub s, Mode m) {
                Log.d(TAG, m.getSensorId() + " : " + m.getModeId() + " : " +
                        m.getValue());
            }
        });
    }

    static public  BluetoothDevice bluetoothSetup() {
        // Get the local Bluetooth adapter
        BluetoothAdapter mBtAdapter;
        Set<BluetoothDevice> pairedDevices = new HashSet<BluetoothDevice>();

        mBtAdapter = BluetoothAdapter.getDefaultAdapter();
        // Get a set of currently paired devices
        if (mBtAdapter == null) {
            Log.d(TAG,"null Bluetooth Adaptor");
            return null;
        }
        Set<BluetoothDevice> allPairedDevices = mBtAdapter.getBondedDevices();
        for(BluetoothDevice d : allPairedDevices){
            if(isIfxDevice(d)) pairedDevices.add(d);
        }

        if (pairedDevices.isEmpty()) {
            Log.d(TAG,"No sensor found. try again");
            return null;
        }
        BluetoothDevice hub = (BluetoothDevice)pairedDevices.toArray()[0];
        return hub;
    }

    static private boolean isIfxDevice(BluetoothDevice device) {
        String deviceName = device.getName().toLowerCase();
        if(deviceName.equals("ifx_nanohub") ||
                deviceName.equals("ifx_senhub"))
            return true;
        return false;
    }

    static private void sensorSetting() {  //todo - add configurable parameters
        if(mSensorHub.getSensorList().size() == 0){
            Log.d(TAG, "Error: no sensor detected");
            return;
        }
        Sensor sensor = mSensorHub.getSensorList().get(0);
        mSensorHub.setMode(sensor.getId(), new Mode("mode", "bg"));
        mSensorHub.setMode(sensor.getId(), new Mode("prs_mr", "64"));
        mSensorHub.setMode(sensor.getId(), new Mode("prs_osr", "8"));
//      mSensorHub.setMode(sensor.getId(), new Mode("temp_mr", "2"));
//      mSensorHub.setMode(sensor.getId(), new Mode("temp_osr", "32"));

    }

    public void disconnect() {
        Log.d(TAG, "Terminating SensorHub Connection.");
        if (mSensorHub != null)
            mSensorHub.disconnect();
    }

    public void connectBLEController(String options) {
        Log.d(TAG, "Register bleManager callback");
        if (mSensorHub != null)
            mSensorHub.connect();
        else
            Log.d(TAG,"NULL sensor");
    }

    public double getPressure() {
        if (mSensorHub == null || !mSensorHub.isConnected()) {
            return 0f;
        }

        return pressureValue;
    }

    public void sendData(byte[] command) {
        Log.d(TAG, "send " + command.length);
        mSensorHub.setSelectedSensor(command.toString());
    }

    /* create singleton object */
    static private byte[] dataBuffer;
    private static volatile SensorPlugin instance;
    private static final Object SINGLETON_LOCK = new Object();
    static private double pressureValue = 0f;

    /**
     * Stores callback information
     */
    private static class UnityCallback {
        private final String gameObjectName;
        private final String methodName;

        UnityCallback(String gameObjectName, String methodName) {
            this.gameObjectName = gameObjectName;
            this.methodName = methodName;
        }

        public String getGameObjectName() {
            return gameObjectName;
        }

        public String getMethodName() {
            return methodName;
        }

        @Override
        public int hashCode() {
            return (gameObjectName + methodName).hashCode();
        }
    }

}
