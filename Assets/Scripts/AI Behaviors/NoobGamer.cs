using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoobGamer : AIController
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
            case AIState.RandomMovement:
                
                break;
            case AIState.RandomObserve:
                
                break;
            case AIState.ShootAndFlee:
                
                break;
            default:
                Debug.LogError("The switch could not determine the current state.");
                break;
        }
    }
}