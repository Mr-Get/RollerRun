using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



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
            else if (timer > 38 && timer < 46)
                setTutorialText(6);
            else if (timer > 46 && timer < 52)
                setTutorialText(7);
            else if (timer > 52 && timer < 56)
                setTutorialText(8);
            else if (timer > 56 && timer < 60)
                setTutorialText(9);
            else if (timer > 60 && timer < 64)
                setTutorialText(10);
            else if (timer > 64 && timer < 72)
                setTutorialText(11);
            else if (timer > 72 && timer < 76)
                setTutorialText(12);
            else if (timer > 76)
                SceneManager.LoadScene("Online"); 

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
                tutorialText.text = "There are three types of cubes:\nGreen: 1 Point\nBlue: 5 Points\nRed: 20 Points";
                break;
            case 7:
                tutorialText.text = "Collect this item to double the points you get.";
                break;
            case 8:
                tutorialText.text = "Or even triple them with this item.";
                break;
            case 9:
                tutorialText.text = "This item will shield you from danger!";
                break;
            case 10:
                tutorialText.text = "Avoid this one. You'll see why...";
                break;
            case 11:
                tutorialText.text = "Be careful. You can only have one item at the same time. If you hit two in a row, the previous one will be lost.";
                break;
            case 12:
                tutorialText.text = "Good luck!";
                break;
        }

    }


}
