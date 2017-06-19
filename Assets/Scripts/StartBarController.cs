using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBarController : MonoBehaviour
{
    public GameObject emptyBar;
    private float emptyX = -1356.71f;
    private float fullX = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateStartBar(float percent)
    {
        if (percent < 0 || percent > 100)
        {
            //Debug.Log("Percent not between 0 and 100!!");
            return;
        }
        float onePercent = (fullX - emptyX) / 100;
        emptyBar.transform.localPosition = new Vector3(emptyX + (onePercent * percent), 0, 0);
    }
}
