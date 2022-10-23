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
                Scan();
                Debug.Log("In Scanning State.");
                if (lastTimeStateChanged + 6 <= Time.time)
                {
                    ChangeState(AIState.RandomMovement);
                }

                break;
            case AIState.RandomMovement:
                RandomMovement();
                Debug.Log("In Random Movement State.");
                if (CanHearTarget())
                {
                    ChangeState(AIState.RandomObserve);
                }
                else if (lastTimeStateChanged + 6 <= Time.time)
                {
                    hasDoneRandomTask = true;
                    ChangeState(AIState.Scanning);
                }

                break;
            case AIState.RandomObserve:
                RandomObserve();
                Debug.Log("In Random Observe State.");
                if (CanSeeTarget())
                {
                    ChangeState(AIState.AttackThenFlee);
                }
                else if (lastTimeStateChanged + 10 <= Time.time)
                {
                    hasDoneRandomTask = true;
                    ChangeState(AIState.RandomMovement);
                }

                break;
            case AIState.AttackThenFlee:
                AttackThenFlee();
                Debug.Log("In Attack Then Flee State.");
                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    currentTime = 0;
                    ChangeState(AIState.Scanning);
                }

                break;
            default:
                Debug.LogError("The switch could not determine the current state.");
                break;
        }
    }
}