using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITankPawn : Pawn
{
    // For AI sight
    public GameObject raycastLocation;

    // Start is called before the first frame update
    public override void Start()
    {
        // Use base."Name"(); if you want to call the parent's function
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public void RotateTowards(Vector3 targetVector)
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        mover.RotateTowards(targetVector, turnSpeed);
    }
}