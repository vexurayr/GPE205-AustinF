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

        if (target != null)
        {
            MakeDecisions();
            //Debug.Log(target);
        }
        else
        {
            TargetNearestTank();
        }
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
                //Debug.Log("In Idle State.");

                TargetNearestTank();

                // Check for transitions
                if (IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Chase);
                }
                else if (CanSeePickup())
                {
                    ChangeState(AIState.SeekPowerup);
                }
                else if (pawn.GetComponent<Health>().GetHealth() <= healthToFlee && IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Flee);
                }

                break;
            case AIState.Chase:
                // Do state actions
                if (target == null)
                {
                    ChangeState(AIState.Idle);
                }
                else
                {
                    Seek(target);
                    //Debug.Log("In Chase State.");
                }

                // Check state transitions
                if (!IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Idle);
                }
                else if (CanSeePickup())
                {
                    ChangeState(AIState.SeekPowerup);
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
                if (target == null)
                {
                    ChangeState(AIState.Idle);
                }
                else
                {
                    SeekAndAttack();
                    //Debug.Log("In Seek And Attack State.");
                }

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
                if (target == null)
                {
                    ChangeState(AIState.Idle);
                }
                else
                {
                    Flee();
                }

                //Debug.Log("In Flee State.");
                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Idle);
                }
                else if (CanSeePickup())
                {
                    ChangeState(AIState.SeekPowerup);
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
                    ChangeState(AIState.Idle);
                }

                break;
            default:
                Debug.LogError("The switch could not determine the current state.");

                break;
        }
    }
}
