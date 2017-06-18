using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFailElement : MonoBehaviour
{

    GameObject gameBall;
    GameObject variableContianer;

    // Use this for initialization
    void Start()
    {
        gameBall = GameObject.Find("GameBall");
        variableContianer = GameObject.Find("ScriptContainer");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name.IndexOf("Ball") > -1 && gameBall.GetComponent<CollectPoints>().effect != "shieldActivate")
        {
            variableContianer.GetComponent<VariableScript>().gameOver();
        }
    }
}
