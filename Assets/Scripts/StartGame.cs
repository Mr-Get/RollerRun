using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    //Variable to start Automovement
    public bool start = false;
    public GameObject player01;
    public float timeToCrouch = 3.0f;
    public GameObject progressBar;

    float crouchTimer = 0;
    float sensitivity;

    //Variable um Online von Testszene zu unterscheiden
    private bool firstRowGeneration;


    // Use this for initialization
    void Start()
    {
        player01 = GameObject.Find("GameBall");
        sensitivity = GameObject.Find("GameBall").GetComponent<MovePlayer>().getSensitivity();
        firstRowGeneration = GameObject.Find("ScriptContainer").GetComponent<VariableScript>().firstRowGeneration;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player01.GetComponent<MovePlayer>().start)
        {
            float h = GameObject.Find("GameBall").GetComponent<MovePlayer>().getH();
            if (h > sensitivity)
            {
                crouchTimer += Time.deltaTime;
                progressBar.GetComponent<StartBarController>().updateStartBar(crouchTimer * 100 / timeToCrouch);

                if (crouchTimer > timeToCrouch)
                {
                    StartGameNow();
                }
            }
            else
            {
                resetTimer();
            }
        }
    }

    void resetTimer()
    {
        progressBar.GetComponent<StartBarController>().updateStartBar(0);
        crouchTimer = 0;
    }

    private void StartGameNow()
    {
        player01.GetComponent<MovePlayer>().start = true;

        if (firstRowGeneration)
        {
            GameObject.Find("StartInfoCanvas").SetActive(false);
        }
        else
        {
            GameObject.Find("StartBar").SetActive(false);
            GameObject.Find("ScriptContainer").GetComponent<TutorialScript>().startVariable = true;
        }

    }


}
