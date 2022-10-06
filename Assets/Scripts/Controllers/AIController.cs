using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    // Last time the AI changed states
    private float lastTimeStateChanged;

    // All states in finite state machine
    public enum AIState { Guard, Idle, Seek, Chase, Attack };

    // State the fsm is currently in
    public AIState currentState;

    // AI controller's target, likely the player
    public GameObject target;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // AI starts in idle state
        ChangeState(AIState.Idle);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        MakeDecisions();
    }

    public void MakeDecisions()
    {
        switch(currentState)
        {
            case AIState.Idle:
                // Does the actions of the state
                DoIdleState();
                // Check for transitions
                if (IsDistanceLessThan(target, pawn.distanceToSeePlayer))
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.Chase:
                // Do state actions
                DoChaseState();
                // Check state transitions
                if (!IsDistanceLessThan(target, pawn.distanceToSeePlayer))
                {
                    ChangeState(AIState.Idle);
                }
                break;
            default:
                Debug.LogError("The switch could not determine the current state.");
                break;
        }
    }

    // Option to override later for AI tanks with different personalities
    public virtual void ChangeState(AIState newState)
    {
        // Change current state
        currentState = newState;

        // Saves the time this state changed
        lastTimeStateChanged = Time.time;
    }

    public void DoSeekState()
    {
        Seek(target);
    }

    // Polymorphism at its finest
    public void Seek(GameObject target)
    {
        pawn.RotateTowards(target.transform.position);

        pawn.MoveForward();
    }

    public void Seek(Transform targetTransform)
    {
        Seek(targetTransform.gameObject);
    }

    public void Seek(Pawn targetPawn)
    {
        Seek(targetPawn.gameObject);
    }

    protected void DoChaseState()
    {
        Seek(target);
    }

    protected void DoIdleState()
    {
        Debug.Log("Doing nothing.");
    }

    protected bool IsDistanceLessThan(GameObject target, float distance)
    {
        // Checks distance between 2 vectors
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
