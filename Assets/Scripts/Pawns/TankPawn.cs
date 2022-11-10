using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    // Helps determine the right tank to destroy in the game manager
    public bool isPlayer;

    // For camera movement
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    [Range(0.0f, 1.0f)] public float mouseSmoothTime;

    // To track camera data
    private Vector3 cameraZoom = new Vector3(1, 1, 1);
    private float cameraPitch = 0.0f;
    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    // For disabling controls if the pawn is stunned
    protected bool isStunned = false;
    protected float stunTime = 0f;

    // Start is called before the first frame update
    public override void Start()
    {
        // Use base."Name"(); if you want to call the parent's function
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        Stun();

        if (timeUntilNextEvent >= 0)
        {
            timeUntilNextEvent -= Time.deltaTime;
            timeUntilNextEvent = Mathf.Clamp(timeUntilNextEvent, 0, secondsPerShot);
        }
    }

    public void SetStunnedTrue(float stunTime)
    {
        isStunned = true;
        this.stunTime = stunTime;
    }

    public void Stun()
    {
        if (isStunned)
        {
            // Counts down until tank is no longer stunned
            stunTime -= Time.deltaTime;
        }
        if (stunTime <= 0f)
        {
            isStunned = false;
        }
    }

    // Inherits mover object from Pawn class and gets the component from Pawn's Start()
    public override void MoveForward()
    {
        if (!isStunned)
        {
            // Calls the Move function in the mover class, but the function in TankMover is run because that is the script attached to the pawn
            mover.Move(tankBody.transform.forward, moveSpeed);
        }
    }

    public override void MoveBackward()
    {
        if (!isStunned)
        {
            mover.Move(tankBody.transform.forward, -moveSpeed);
        }
    }

    public override void RotateClockwise()
    {
        mover.Rotate(turnSpeed);
    }

    public override void RotateCounterclockwise()
    {
        mover.Rotate(-turnSpeed);
    }

    // These two only work for the player for some reason
    public override void RotateBodyClockwise()
    {
        if (!isStunned)
        {
            mover.RotateBody(tankBodyPivotPoint, turnSpeed);
            // Makes sure tank head stays attached to the tank body in the right place
            tankHead.transform.position = tankBodyHeadConnection.transform.position;
        }
    }

    public override void RotateBodyCounterclockwise()
    {
        if (!isStunned)
        {
            mover.RotateBody(tankBodyPivotPoint, -turnSpeed);
            tankHead.transform.position = tankBodyHeadConnection.transform.position;
        }
    }

    public override void RotateTowards(Vector3 targetVector)
    {
        if (!isStunned)
        {
            mover.RotateTowards(targetVector, turnSpeed);
            tankHead.transform.position = tankBodyHeadConnection.transform.position;
        }
    }

    public override void Shoot()
    {
        if (!isStunned)
        {
            if (timeUntilNextEvent <= 0)
            {
                // Calls a function in Shooter using the reference in Pawn and gives the data saved in Pawn
                shooter.Shoot(shellPrefab, fireForce, shellLifeSpan);

                // Starts the cooldown after the player has shot
                ShootCooldown();

                if (GetComponent<PowerupManager>() != null)
                {
                    GetComponent<PowerupManager>().RemoveStunPowerup();
                }
            }
        }
    }

    // A countdown method timer
    public override void ShootCooldown()
    {
        if (timeUntilNextEvent <= 0)
        {
            timeUntilNextEvent = secondsPerShot;
        }
    }

    public override void MoveCamera()
    {
        // Saves the x and y position of the mouse on the screen
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Smooths camera movement
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        // Saves the camera's vertical rotation as the inverse of mouse movement
        cameraPitch -= currentMouseDelta.y * mouseSensitivityY;

        // Guarentees the camera does not rotate up or down beyond a certain point
        cameraPitch = Mathf.Clamp(cameraPitch, maxPitchAngleUp, maxPitchAngleDown + maxPitchAngleUp);

        // Rotates the camera vertically
        cameraPivotPoint.transform.localEulerAngles = Vector3.right * cameraPitch;

        // Rotates the top of the tank left and right based on the mouse's x (horizontal) position
        tankHead.transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivityX);

        cameraZoom.y += (Input.mouseScrollDelta.y * -1);
        cameraZoom.z += (Input.mouseScrollDelta.y * -1);

        if (cameraZoom.y <= 1 || cameraZoom.z <= 1)
        {
            cameraZoom.y = 1;
            cameraZoom.z = 1;
        }
        else if (cameraZoom.y >= 4 || cameraZoom.z >= 4)
        {
            cameraZoom.y = 4;
            cameraZoom.z = 4;
        }

        cameraPivotPoint.transform.localScale = cameraZoom;
    }

    public float GetTimeUntilNextEvent()
    {
        return timeUntilNextEvent;
    }

    public override void Die()
    {
        base.Die();
    }
}