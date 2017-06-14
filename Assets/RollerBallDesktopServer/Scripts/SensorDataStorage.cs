using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
    Autor: Robert Strohmaier <br />
    Letzte Änderung: 30.03.2017    
    
    Funktion
    --------
    Sensor Daten speichern.

    Die Sensor Daten der Smartphone Clients werden in dieser Klasse gespeichert. 
    
    Die Daten sind über das Prefab des NetworkDataController zugänglich.
    
*/

public class SensorDataStorage : MonoBehaviour {
    
    //public Vector3 gyroGravity { get; set; }
    public Vector3 gyroGravity;
    public Vector3 gyroAcceleration;
    public Vector3 gyroRotation;

    public float microphoneLevel;

    public Vector3 accelerator;
    
    //public CompassSensorValues compass = new CompassSensorValues();
    public float compassMagneticHeading = -99.99f;
    public float compassTrueHeading = -99.99f;
    //public Vector3 compassRaw;
    public float compassAccuracy = -99.99f;
    public double compassLastDataGot = -99.99;

    public bool[] isPressedButtonBluetooth = new bool[1];
    public int[] buttonCountBluetooth = new int[1];

    public bool[] isPressedButtonTouchscreen = new bool[1];
    public int[] buttonCountTouchscreen = new int[1];
    

    void Awake() {
        DontDestroyOnLoad(this);
    }

}

/*
public class CompassSensorValues:MonoBehaviour {
    public float compassMagneticHeading = -99.99f;
    public float compassTrueHeading = -99.99f;
    //public Vector3 compassRaw;
    public float compassAccuracy = -99.99f;
    public double compassLastDataGot = -99.99;
}
*/
