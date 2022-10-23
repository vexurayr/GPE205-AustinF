using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingDuck : AIController
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
                // Does the actions of the state
                Idle();
                Debug.Log("In Idle State.");
                // Check for transitions
                if (IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Chase);
                }
                else if (pawn.GetComponent<Health>().GetHealth() <= healthToFlee && IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Flee);
                }
                break;
            case AIState.Chase:
                // Do state actions
                Seek(target);
                Debug.Log("In Chase State.");
                // Check state transitions
                if (!IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Idle);
                }
                // AI has been in this state for secondsToAttackPlayer amount of time
                else if (lastTimeStateChanged <= Time.time - secondsToAttackPlayer)
                {
                    ChangeState(AIState.SeekAndAttack);
                }
                // AI has low health and is not already far away from the player
                else if (pawn.GetComponent<Health>().GetHealth() <= healthToFlee && IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Flee);
                }
                break;
            case AIState.SeekAndAttack:
                SeekAndAttack();
                Debug.Log("In Seek And Attack State.");
                if (!IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Idle);
                }
                else if (pawn.GetComponent<Health>().GetHealth() <= healthToFlee && IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Flee);
                }
                break;
            case AIState.Flee:
                Flee();
                Debug.Log("In Flee State.");
                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Idle);
                }
                break;
            default:
                Debug.LogError("The switch could not determine the current state.");
                break;
        }
    }
}
