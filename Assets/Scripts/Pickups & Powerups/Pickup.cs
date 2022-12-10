using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    // Tells the object how fast it'll spin
    public float rotationSpeed;
    // How fast the object will move up and down
    public float bounceSlowness;
    // How far the object will move up and down
    public float bounceDistance;
    private float bounceMax;
    private float bounceMin;
    private bool isMovingUp = true;

    // Need inverse of bounceSlowness because this way of doing it requires very small numbers
    private float inverseBounceSpeed;

    public virtual void Start()
    {
        bounceMax = this.transform.position.y + bounceDistance;
        bounceMin = this.transform.position.y - bounceDistance;
        inverseBounceSpeed = 1 / bounceSlowness;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // Will rotate entire object
        this.transform.Rotate(new Vector3(0f, rotationSpeed, 0f));
        Bounce();
    }

    public virtual void Bounce()
    {
        // Gets current position separated
        float xPos = this.transform.position.x;
        float yPos = this.transform.position.y;
        float zPos = this.transform.position.z;

        // Start moving down at peak, start moving up at valley
        if (yPos > bounceMax)
        {
            isMovingUp = false;
        }
        else if (yPos < bounceMin)
        {
            isMovingUp = true;
        }

        if (isMovingUp)
        {
            this.transform.position = new Vector3(xPos, yPos + inverseBounceSpeed, zPos);
        }
        else
        {
            this.transform.position = new Vector3(xPos, yPos - inverseBounceSpeed, zPos);
        }
    }

    public void PlayPickupSound()
    {
        AudioManager.instance.PlaySound("All Pickup Taken", gameObject.transform);
    }
}