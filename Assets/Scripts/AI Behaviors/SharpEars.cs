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
                Scan();
                Debug.Log("In Scanning State.");
                if (CanHearTarget())
                {
                    noiseLocation.transform.position = target.transform.position;
                    ChangeState(AIState.SeekNoise);
                }
                else if (pawn.GetComponent<Health>().GetHealth() <= healthToFlee && IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.AttackWhileFleeing);
                }

                break;
            case AIState.SeekNoise:
                SeekNoise();
                Debug.Log("In Seek Noise State.");
                if (IsDistanceLessThan(noiseLocation, 8) && !CanSeeTarget())
                {
                    ChangeState(AIState.Scanning);
                }
                else if (CanSeeTarget())
                {
                    ChangeState(AIState.DistanceAttack);
                }

                break;
            case AIState.DistanceAttack:
                DistanceAttack();
                Debug.Log("In Distance Attack State.");
                if (!IsDistanceLessThan(target, fleeDistance))
                {
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
                AttackWhileFleeing();
                Debug.Log("In Attack While Fleeing State.");
                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Scanning);
                }

                break;
            default:
                Debug.LogError("The switch could not determine the current state.");
                break;
        }
    }
}