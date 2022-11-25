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
                if (CanHearTarget())
                {
                    noiseLocation.transform.position = target.transform.position;
                    ChangeState(AIState.SeekNoise);
                }
                else if (pawn.GetComponent<Health>().GetHealth() <= healthToFlee && IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.AttackWhileFleeing);
                }
                else if (CanSeePickup())
                {
                    ChangeState(AIState.SeekPowerup);
                }

                break;
            case AIState.SeekNoise:
                if (target == null)
                {
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else
                {
                    SeekNoise();
                    //Debug.Log("In Seek Noise State.");
                }

                if (IsDistanceLessThan(noiseLocation, 8) && !CanSeeTarget())
                {
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else if (CanSeeTarget())
                {
                    ChangeState(AIState.DistanceAttack);
                }
                else if (CanSeePickup())
                {
                    ChangeState(AIState.SeekPowerup);
                }

                break;
            case AIState.DistanceAttack:
                if (target == null)
                {
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else
                {
                    DistanceAttack();
                    //Debug.Log("In Distance Attack State.");
                }

                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else if (lastTimeStateChanged + 20 <= Time.time)
                {
                    ChangeState(AIState.SeekNoise);
                }
                else if (pawn.GetComponent<Health>().GetHealth() <= healthToFlee && IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.AttackWhileFleeing);
                }

                break;
            case AIState.AttackWhileFleeing:
                if (target == null)
                {
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else
                {
                    AttackWhileFleeing();
                    //Debug.Log("In Attack While Fleeing State.");
                }

                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    startDirection = pawn.transform.forward;
                    ChangeState(AIState.Scanning);
                }
                else if (CanSeePickup())
                {
                    ChangeState(AIState.SeekPowerup);
                }

                break;
            // In this case, the chase state is used to get the AI to move towards a powerup
            case AIState.SeekPowerup:
                if (targetPickup != null && !IsDistanceLessThan(targetPickup, .5f))
                {
                    SeekExactXAndZ(targetPickup);
                    //Debug.Log("In Seek Powerup State.");
                }
                else
                {
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