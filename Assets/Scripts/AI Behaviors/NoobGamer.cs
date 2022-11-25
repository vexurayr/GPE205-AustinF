using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

        if (target != null)
        {
            MakeDecisions();
        }
        else
        {
            TargetNearestTank();
        }
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
                //Debug.Log("In Scanning State.");
                if (lastTimeStateChanged + 6 <= Time.time)
                {
                    hasDoneRandomTask = true;
                    ChangeState(AIState.RandomMovement);
                }
                else if (CanHearTarget())
                {
                    hasDoneRandomTask = true;
                    ChangeState(AIState.RandomObserve);
                }
                else if (CanSeeTarget())
                {
                    hasDoneRandomTask = true;
                    ChangeState(AIState.AttackThenFlee);
                }

                break;
            case AIState.RandomMovement:
                if (target == null)
                {
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else
                {
                    RandomMovement();
                    //Debug.Log("In Random Movement State.");
                }

                if (CanHearTarget())
                {
                    hasDoneRandomTask = true;
                    ChangeState(AIState.RandomObserve);
                }
                else if (IsDistanceLessThan(randomLocation, waypointStopDistance))
                {
                    hasDoneRandomTask = true;
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else if (lastTimeStateChanged + 4 <= Time.time)
                {
                    hasDoneRandomTask = true;
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }

                break;
            case AIState.RandomObserve:
                if (target == null)
                {
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else
                {
                    RandomObserve();
                    //Debug.Log("In Random Observe State.");
                }

                if (CanSeeTarget())
                {
                    hasDoneRandomTask = true;
                    ChangeState(AIState.AttackThenFlee);
                }
                else if (lastTimeStateChanged + 4 <= Time.time)
                {
                    hasDoneRandomTask = true;
                    ChangeState(AIState.RandomMovement);
                }

                break;
            case AIState.AttackThenFlee:
                if (target == null)
                {
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else
                {
                    AttackThenFlee();
                    //Debug.Log("In Attack Then Flee State.");
                }

                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    hasDoneRandomTask = true;
                    currentTime = 0;
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else if (CanSeePickup())
                {
                    hasDoneRandomTask = true;
                    currentTime = 0;
                    ChangeState(AIState.SeekPowerup);
                }
                else if (currentTime > 8)
                {
                    hasDoneRandomTask = true;
                    currentTime = 0;
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }

                break;
            case AIState.SeekPowerup:
                if (targetPickup != null && !IsDistanceLessThan(targetPickup, .5f))
                {
                    SeekExactXAndZ(targetPickup);
                    //Debug.Log("In Seek Powerup State.");
                }
                else
                {
                    hasDoneRandomTask = true;
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }

                break;
            default:
                Debug.LogError("The switch could not determine the current state.");
                break;
        }
    }
}