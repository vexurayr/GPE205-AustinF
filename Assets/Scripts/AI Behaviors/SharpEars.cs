using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpEars : AIController
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
        ChangeState(AIState.Scanning);
    }

    public void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Scanning:

                break;
            case AIState.SeekNoise:

                break;
            case AIState.DistanceAttack:

                break;
            case AIState.BackwardsAttackFlee:

                break;
            default:
                Debug.LogError("The switch could not determine the current state.");
                break;
        }
    }
}