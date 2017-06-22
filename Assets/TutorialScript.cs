using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TutorialScript : MonoBehaviour {

    private Text tutorialText;
    private int tutorialNumber = 0;

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
        //Add timer here
        // e.g  effectTime -= Time.deltaTime;

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
        }

    }


}
