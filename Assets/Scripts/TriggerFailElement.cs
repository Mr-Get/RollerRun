using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFailElement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerExit(Collider col) { 
        if(col.gameObject.name.IndexOf("Ball")> -1)
        {
            Destroy(col.gameObject);
        }
    }
}
