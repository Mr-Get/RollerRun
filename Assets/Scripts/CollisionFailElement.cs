using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionFailElement : MonoBehaviour {

    GameObject gameBall;

    // Use this for initialization
    void Start () {
        gameBall = GameObject.Find("GameBall");
    }

    // Update is called once per frame
    void Update () {
		
	}
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.IndexOf("Ball") > -1 && gameBall.GetComponent<CollectPoints>().effect != "shieldActivate")
        {
            SceneManager.LoadScene("Online");
        }
    }
}
