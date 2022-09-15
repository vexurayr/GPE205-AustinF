using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    // Start is called before the first frame update
    protected override void Start()
    {
        // Use base."Name"(); if you want to call the parent's function
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void MoveForward()
    {
        Debug.Log("Move Forwards");
    }

    public override void MoveBackward()
    {
        Debug.Log("Move Backwards");
    }

    public override void RotateClockwise()
    {
        Debug.Log("Turn Clockwise");
    }

    public override void RotateCounterclockwise()
    {
        Debug.Log("Turn Counterclockwise");
    }
}
