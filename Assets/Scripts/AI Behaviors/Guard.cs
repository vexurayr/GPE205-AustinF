using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AIController;

public class Guard : AIController
{
    public override void Start()
    {
        base.Start();

        StartingState();
    }

    public override void Update()
    {
        base.Update();

        MakeDecisions();
    }

    public void StartingState()
    {
        ChangeState(AIState.Idle);
    }

    public void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Idle:

                break;
            case AIState.Patrol:

                break;
            case AIState.FaceNoise:

                break;
            case AIState.StationaryAttack:

                break;
            default:
                Debug.LogError("The switch could not determine the current state.");
                break;
        }
    }
}