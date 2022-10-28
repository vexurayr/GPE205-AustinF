using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWheelAnimator : MonoBehaviour
{
    public float rotationSpeed = 1f;

    // Wheels for left tread
    public GameObject lWheel1;
    public GameObject lWheel2;
    public GameObject lWheel3;
    // Wheels for right tread
    public GameObject rWheel1;
    public GameObject rWheel2;
    public GameObject rWheel3;

    public void Forward()
    {
        lWheel1.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        lWheel2.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        lWheel3.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        rWheel1.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        rWheel2.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        rWheel3.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
    }

    public void Backwards()
    {
        lWheel1.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        lWheel2.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        lWheel3.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        rWheel1.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        rWheel2.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        rWheel3.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
    }

    public void Clockwise()
    {
        lWheel1.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        lWheel2.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        lWheel3.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        rWheel1.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        rWheel2.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        rWheel3.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
    }

    public void Counterclockwise()
    {
        lWheel1.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        lWheel2.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        lWheel3.transform.Rotate(new Vector3(-rotationSpeed, 0f, 0f));
        rWheel1.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        rWheel2.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
        rWheel3.transform.Rotate(new Vector3(rotationSpeed, 0f, 0f));
    }
}