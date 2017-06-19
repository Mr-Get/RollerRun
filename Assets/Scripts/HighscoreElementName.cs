using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreElementName : MonoBehaviour {

    public int rowCount;

	// Use this for initialization
	void Start () {
        GameObject variableContainer = GameObject.Find("MenuController");
        HighscoreClass highscoreEntry = variableContainer.GetComponent<MenuController>().getHighScoreElement(rowCount);
        this.gameObject.GetComponent<Text>().text = highscoreEntry.name.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
