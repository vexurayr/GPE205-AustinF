using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    // Ideal move speed = 12
    // Ideal turn speed = .25

    // Start is called before the first frame update
    public override void Start()
    {
        // Use base."Name"(); if you want to call the parent's function
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (timeUntilNextEvent >= 0)
        {
            timeUntilNextEvent -= Time.deltaTime;
            timeUntilNextEvent = Mathf.Clamp(timeUntilNextEvent, 0, secondsPerShot);
        }
        cooldownText.text = "Shoot Cooldown: " + timeUntilNextEvent.ToString("n2");
    }

    // Inherits mover object from Pawn class and gets the component from Pawn's Start()
    public override void MoveForward()
    {
        // Calls the Move function in the mover class, but the function in TankMover is run because that is the script attached to the pawn
        mover.Move(transform.forward, moveSpeed);
    }

    public override void MoveBackward()
    {
        mover.Move(transform.forward, -moveSpeed);
    }

    public override void RotateClockwise()
    {
        mover.Rotate(turnSpeed);
    }

    public override void RotateCounterclockwise()
    {
        mover.Rotate(-turnSpeed);
    }

    public override void Shoot()
    {
        if (timeUntilNextEvent <= 0)
        {
            // Calls a function in Shooter using the reference in Pawn and gives the data saved in Pawn
            shooter.Shoot(shellPrefab, fireForce, damageDone, shellLifeSpan);

            // Starts the cooldown after the player has shot
            ShootCooldown();
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

    public override void SwitchCamera()
    {
        // Makes sure nothing happens if the pawn only has one camera
        if (firstPersonCamera.enabled && thirdPersonCamera != null)
        {
            // Changes camera to third person
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
        }
        else if (thirdPersonCamera.enabled && firstPersonCamera != null)
        {
            // Changes camera to first person
            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;
        }
    }
}