using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreElement : MonoBehaviour {

    public int rowCount;
    public GameObject[] children;

	// Use this for initialization
	void Start () {
        children = this.gameObject.GetComponentsInChildren<GameObject>();
        int i = 0;
        Debug.Log(children.Length);
        foreach(GameObject g in children)
        {
            Debug.Log(i + " " + g.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
