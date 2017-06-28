using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TutorialScript : MonoBehaviour {

    private Text tutorialText;
    private int tutorialNumber = 0;
    private float timer = 0;

    //Public variabl to start the Tutorial. Will be activated by the StartGame Script
    public bool startVariable;


	// Use this for initialization
	void Start () {
        tutorialText = GameObject.Find("InfoText").GetComponent<Text>();
        setTutorialText(0);

    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void FixedUpdate()
    {

        //Check if the startGame Variable on the Gameball is active
        if (startVariable)
        {
            timer += Time.deltaTime;

            if (timer < 5)
                setTutorialText(1);
            else if (timer > 5 && timer < 14)
                setTutorialText(2);
            else if (timer > 14 && timer < 32)
                setTutorialText(3);
            else if (timer > 32 && timer < 35)
                setTutorialText(4);
            else if (timer > 35 && timer < 38)
                setTutorialText(5);
            else if (timer > 38 && timer < 50)
                setTutorialText(6);

        }

    }


    void setTutorialText(int pagenumber)
    {

        switch (pagenumber)
        {
            case 0:
                tutorialText.text = "Welcome to RollerRun \nUser your RollerBone to collect Points and evade obstacles. \n Tilt your board to the right to start.";
                break;
            case 1:
                tutorialText.text = "Don't worry about falling off. \nYou can only move within the three lanes.";
                break;
            case 2:
                tutorialText.text = "Are you ready for the first obstacle?\nHere it comes!";
                break;
            case 3:
                tutorialText.text = "Great! Here are a few more";
                break;
            case 4:
                tutorialText.text = "Well Done";
                break;
            case 5:
                tutorialText.text = "Fill your score and get into the Hall of Fame by collecting points.";
                break;
            case 6:
                tutorialText.text = "There are three types of cubes:\n\nGreen: 1 Point\nBlue: 5Points\nRed:30Points";
                break;
        }

    }


}
