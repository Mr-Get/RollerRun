using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPoints : MonoBehaviour {

    //public Variables
    public float doublePointsTime = 10f;
    public float triplePointsTime = 10f;
    public float shieldTime = 10f;
    public float switchControlTime = 10f;
    public string effect = "";


    //Variable ContainerScript
    private GameObject scriptContainer;
    private float effectTime = 0f;
   

    //Colliding Object
    private GameObject collidingObject;

	// Use this for initialization
	void Start () {
        scriptContainer = GameObject.Find("ScriptContainer");
    }

    //Declaration of colors
    Color cPurple = new Color(0.706F, 0.247F, 1.0F);
    Color cDoublePoints = new Color(0, 171.0F/255.0F, 1.0F);
    Color cTriplePoints = new Color(244.0F /255.0F, 146.0F / 255.0F, 0F);
    Color cShield = new Color(21.0F / 255.0F, 175.0F / 255.0F, 11.0F / 255.0F);
    Color cSwitchControl = new Color(1.0F, 0, 0);


    // Update is called once per frame
    void Update () {
		
	}

    private void FixedUpdate()
    {
        effectTime -= Time.deltaTime;

        if (effectTime <= 0)
        {
            itemEffect(false);
            effect = "";
        }

    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "givePoints")
        {
            collidingObject = collision.gameObject;
            if(effect == "doublePoints" && effectTime > 0)
            {
                scriptContainer.GetComponent<VariableScript>().addScore(collidingObject.GetComponent<ContainScoreValue>().scoreValue*2);
            }
            else if (effect == "triplePoints" && effectTime > 0)
            {
                scriptContainer.GetComponent<VariableScript>().addScore(collidingObject.GetComponent<ContainScoreValue>().scoreValue * 3);
            }
            else
            {
                scriptContainer.GetComponent<VariableScript>().addScore(collidingObject.GetComponent<ContainScoreValue>().scoreValue);
            }
            Destroy(collidingObject);
        }

        else if (collision.gameObject.tag == "doublePoints")
        {
            collidingObject = collision.gameObject;
            effect = "doublePoints";
            effectTime = doublePointsTime;
            itemEffect(true);
            Destroy(collidingObject);
        }

        else if (collision.gameObject.tag == "triplePoints")
        {
            collidingObject = collision.gameObject;
            effect = "triplePoints";
            effectTime = triplePointsTime;
            itemEffect(true);
            Destroy(collidingObject);
        }

        else if (collision.gameObject.tag == "shieldActivate")
        {
            collidingObject = collision.gameObject;
            effect = "shieldActivate";
            effectTime = shieldTime;
            itemEffect(true);
            Destroy(collidingObject);
        }

        else if (collision.gameObject.tag == "switchControl")
        {
            collidingObject = collision.gameObject;
            effect = "switchControl";
            effectTime = switchControlTime;
            itemEffect(true);
            Destroy(collidingObject);
        }

        else if (collision.gameObject.tag == "obstacle" && effect == "shieldActivate")
        {
            collidingObject = collision.gameObject;
            Destroy(collidingObject);
        }
    }

    //Controls the color of the effects 
    public void itemEffect (bool lightswitch)
    {
        switch (effect)
        {
            case "doublePoints":
                this.GetComponent<Light>().color = cDoublePoints;
                break;
            case "triplePoints":
                this.GetComponent<Light>().color = cTriplePoints;
                break;
            case "shieldActivate":
                this.GetComponent<Light>().color = cShield;
                break;
            case "switchControl":
                this.GetComponent<Light>().color = cSwitchControl;
                break;
        }
        
        if (lightswitch)
            this.GetComponent<Light>().intensity = 3;
        else
            this.GetComponent<Light>().intensity = 0;

    }


}
