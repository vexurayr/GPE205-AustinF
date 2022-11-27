using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    //public TextMeshPro cooldownText;

    // Not target time, but waiting for something to drop to 0
    public float timerDelay;
    private float timeUntilNextEvent;

    // Start is called before the first frame update
    void Start()
    {
        timeUntilNextEvent = timerDelay;    
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilNextEvent -= Time.deltaTime;

        //cooldownText.text = timeUntilNextEvent.ToString("n2"); n0 to not display any decimals

        if (timeUntilNextEvent <= 0)
        {
            Debug.Log("Countdown expired");
            timeUntilNextEvent = timerDelay;
        }
    }
}
