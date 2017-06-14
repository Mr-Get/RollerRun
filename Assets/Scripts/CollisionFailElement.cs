using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFailElement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.IndexOf("Ball") > -1)
        {
            Destroy(col.gameObject);
        }
    }
}
