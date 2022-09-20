using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    // Ideal move speed = 12
    // Ideal turn speed = .25

    // Start is called before the first frame update
    protected override void Start()
    {
        // Use base."Name"(); if you want to call the parent's function
        base.Start();
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

    public override void SwitchCamera()
    {
        if (firstPersonCamera.enabled)
        {
            // Changes camera to third person
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
        }
        else
        {
            // Changes camera to first person
            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;
        }
    }
}
