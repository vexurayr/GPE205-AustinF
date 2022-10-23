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
                Idle();
                Debug.Log("In Idle State.");
                if (waypoints != null)
                {
                    ChangeState(AIState.Patrol);
                }

                break;
            case AIState.Patrol:
                Patrol();
                Debug.Log("In Patrol State.");
                if (CanHearTarget())
                {
                    noiseLocation.transform.position = target.transform.position;
                    ChangeState(AIState.FaceNoise);
                }
                else if (waypoints == null)
                {
                    ChangeState(AIState.Idle);
                }

                break;
            case AIState.FaceNoise:
                FaceNoise();
                Debug.Log("In Face Noise State.");
                if (CanSeeTarget())
                {
                    ChangeState(AIState.StationaryAttack);
                }
                else if (lastTimeStateChanged + 6 <= Time.time)
                {
                    ChangeState(AIState.Patrol);
                }

                break;
            case AIState.StationaryAttack:
                StationaryAttack();
                Debug.Log("In Stationary Attack State.");
                if (!IsDistanceLessThan(target, chaseDistance))
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