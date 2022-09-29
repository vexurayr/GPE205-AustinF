using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreWeThereYet : MonoBehaviour
{
    // Example of printing something every x number of seconds
    // Time between iterations

    public float timeDelay;
    private float nextEventTime;

    // Start is called before the first frame update
    void Start()
    {
        nextEventTime = Time.time + timeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextEventTime)
        {
            Debug.Log("Time is up!");
            nextEventTime = Time.time + timeDelay;
        }
    }
}
