using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    //Variable to start Automovement
    public bool start = false;
    public GameObject player01;

    // Use this for initialization
    void Start () {
        player01 = GameObject.Find("GameBall");
    }
	
	// Update is called once per frame
	void Update () {

        //Hit Space in the game to start moving forward
        if (Input.GetKeyDown(KeyCode.Space))
            StartGameNow();

    }

    private void StartGameNow()
    {
        player01.GetComponent<MovePlayer>().start = true;
        GameObject.Find("StartInfoCanvas").SetActive(false);
    }


}
