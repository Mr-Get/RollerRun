using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    //-------------------------------------------------------------------------
    //                    Initialise Variables
    //-------------------------------------------------------------------------

    //Variables for Gameobject and Scrikt for NetworkDataController
    private GameObject networkDataControllerGameObject;
    private NetworkDataController networkDataController;
    private NetworkManagerServerController networkController;

    //Variable for sensor data of one player
    private SensorDataStorage sensorDataStoragePlayer01;

    //Variable to start Automovement
    public bool start = false;

    //Variable for horizontal UserInput
    float h;

    // Variables for User Control Configuration
    public float autoSpeed = 10; //How fast the ball moves forward
    public float sensivity = 0.25F; //How far can you tilt the phone bevor it influences the ball
    public float moveSpeed = 5F; //How fast does the ball move from one line to another

    private int mPosition = 0; //Current line the ball is one (-1=left, 0 = middle, 1= right)
    private float linewidth = 2.0F; //Max x value the ball can move left or right



    //-------------------------------------------------------------------------
    //                    Basic Classes
    //-------------------------------------------------------------------------

    void Start () {
        //--- Android Input
        //Get the GameObject where the NetworkDatdaController is attached to
        networkDataControllerGameObject = GameObject.Find("NetworkDataStorageController");
        //Get the script component of NetworkDataController
        networkDataController = networkDataControllerGameObject.GetComponent<NetworkDataController>();

        //Get the Network Controller Component
        networkController = GameObject.Find("NetworkController").GetComponent<NetworkManagerServerController>();
        //Define which kind of data from the smartphone should be used
        //Can also be defined at the Inspector of NetworkController GameObject in Scene init
        networkController.isGyroGravityRequested = true;
        networkController.isAcceleratorRequested = false;
        networkController.isCompassRequested = false;
        networkController.isGyroAccelerationRequested = false;
        networkController.isGyroRotationRequested = false;
        networkController.isMicrophoneRequested = false;
        //Get the data storage for player 01
        sensorDataStoragePlayer01 = networkDataController.GetSensordataStorageForPlayer(1);
        
	}
	
	void Update () {

        
    }

    private void FixedUpdate()
    {
        //Use values from data storage of player one ‐ choose Gyro Gravity!
        h = sensorDataStoragePlayer01.gyroGravity.x;

        if(start)
        {
            if (h < -sensivity)
            {
                mPosition = -1;
            }
            else if (h > sensivity)
            {
                mPosition = 1;
            }
            else
                mPosition = 0;

            MoveNow();
        }

    }



    //-------------------------------------------------------------------------
    //                    Special Classes
    //-------------------------------------------------------------------------

    private void MoveNow()
    {

            switch (mPosition){
                case -1:
                    if (this.transform.position.x > -linewidth)
                        this.transform.position += new Vector3(-moveSpeed, 0f, 0f) * Time.deltaTime;
                    break;
                case 0:
                    if (this.transform.position.x > 0.05)
                        this.transform.position += new Vector3(-moveSpeed, 0f, 0f) * Time.deltaTime;
                    else if (this.transform.position.x < -0.05)
                        this.transform.position += new Vector3(moveSpeed, 0f, 0f) * Time.deltaTime;
                    break;
                case 1:
                    if (this.transform.position.x < linewidth)
                        this.transform.position += new Vector3(moveSpeed, 0f, 0f) * Time.deltaTime;
                    break;
            }

        this.transform.position += new Vector3(0f, 0f, 1.0F * autoSpeed) * Time.deltaTime;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }


}
