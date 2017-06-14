﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreFabPositions : MonoBehaviour
{

    public bool leftPos;
    public bool midPos;
    public bool rightPos;
    float objectPosition;
    VariableScript script;
    GameObject gameBall;

    // Use this for initialization
    void Start()
    {
        script = GameObject.Find("ScriptContainer").GetComponent<VariableScript>();
        gameBall = GameObject.Find("GameBall");
        script.addRow(this.gameObject);
        objectPosition = this.gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameBall.transform.position.z - 30 > objectPosition)
        {
            script.removeRow(this.gameObject);
            Destroy(this.gameObject);
        }

    }
}
