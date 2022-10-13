using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    #region Variables
    // Last time the AI changed states
    private float lastTimeStateChanged;

    // All states in finite state machine
    public enum AIState { Guard, Idle, Chase, Attack, Flee };

    // State the fsm is currently in
    public AIState currentState;

    // AI controller's target, likely the player
    public GameObject target;

    // Time before the AI switches from chasing the player to attacking them
    public float secondsToAttackPlayer;

    // Health the AI must be at or below before it flees
    public float healthToFlee;

    // Distance to prevent the AI from running into the player
    public float nextToPlayerDistance;

    // AI will chase the player if they are within this distance
    public float chaseDistance;

    // AI will move at least this far away from the player when fleeing
    public float fleeDistance;

    // Used for patrol state
    public GameObject[] waypoints;
    public float waypointStopDistance;
    private int currentWaypoint = 0;

    #endregion Variables

    #region MonoBehavior
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // Adds itself to the game manager
        if (GameManager.instance != null && GameManager.instance.aIPlayers != null)
        {
            GameManager.instance.aIPlayers.Add(this);
        }

        // AI starts in idle state
        ChangeState(AIState.Idle);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        MakeDecisions();
    }

    #endregion MonoBehavior

    public void MakeDecisions()
    {
        switch(currentState)
        {
            case AIState.Idle:
                // Does the actions of the state
                Idle();
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
                // Check state transitions
                if (!IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Idle);
                }
                // AI has been in this state for secondsToAttackPlayer amount of time
                else if (lastTimeStateChanged <= Time.time - secondsToAttackPlayer)
                {
                    ChangeState(AIState.Attack);
                }
                // AI has low health and is not already far away from the player
                else if (pawn.GetComponent<Health>().GetHealth() <= healthToFlee && IsDistanceLessThan(target, chaseDistance))
                {
                    ChangeState(AIState.Flee);
                }
                break;
            case AIState.Attack:
                Attack();

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

    #region Conditions & Transitions
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

    // Option to override later for AI tanks with different personalities
    public virtual void ChangeState(AIState newState)
    {
        // Change current state
        currentState = newState;

        // Saves the time this state changed
        lastTimeStateChanged = Time.time;
    }

    #endregion Conditions & Transitions

    #region States
    protected void Idle()
    {
        Debug.Log("Doing nothing.");
    }

    // Polymorphism at its finest
    public void Seek(Vector3 targetVector)
    {
        pawn.RotateTowards(targetVector);

        // Keeps the AI from getting too close to the player
        if (!IsDistanceLessThan(target, nextToPlayerDistance))
        {
            pawn.MoveForward();
        }
    }

    public void Seek(Transform targetTransform)
    {
        Seek(targetTransform.position);
    }

    public void Seek(GameObject target)
    {
        Seek(target.transform);
    }

    public void Seek(Pawn targetPawn)
    {
        Seek(targetPawn.transform);
    }

    protected void Flee()
    {
        // Gets vector to target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;

        // Calculates vector away from target (opposite direction of going to target)
        Vector3 vectorAwayFromTarget = -vectorToTarget;

        // Find how far away the AI will travel
        // Compare how close the player is at the beginning of the flee to always be the flee distance away from the player
        // at the end of the flee
        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);
        float percentOfFleeDistance = targetDistance / fleeDistance;
        // Clamps it between 0 and 1
        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);
        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;

        Vector3 fleeVector = vectorAwayFromTarget.normalized * flippedPercentOfFleeDistance;

        // Seek the point the AI needs to flee to
        Seek(pawn.transform.position + fleeVector);
    }

    // Virtual so that other scripts can override this for AI personalities
    protected virtual void Attack()
    {
        // Continually chases and shoots at target
        Seek(target);
        // AI also limited by shoot timer
        pawn.Shoot();
    }

    protected virtual void Patrol()
    {
        if (waypoints.Length > currentWaypoint)
        {
            Seek(waypoints[currentWaypoint]);

            // If less than that distance away from the next waypoint, skip that waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].transform.position) < waypointStopDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            RestartPatrol();
        }
    }

    protected void RestartPatrol()
    {
        currentWaypoint = 0;
    }

    #endregion States

    #region Targeting Options
    protected virtual void TargetPlayerOne()
    {
        // Must be instance of game manager and list of players, and have players in the list
        if (GameManager.instance != null)
        {
            if (GameManager.instance.players != null)
            {
                if (GameManager.instance.players.Count > 0)
                {
                    target = GameManager.instance.players[0].pawn.gameObject;
                }
            }
        }
    }

    protected virtual void TargetNearestTank()
    {
        Pawn closestTank;
        float closestTankDistance;

        if (GameManager.instance != null)
        {
            if (GameManager.instance.players != null)
            {
                if (GameManager.instance.players.Count > 0)
                {
                    // Assumes first player in list is closest
                    closestTank = GameManager.instance.players[0].pawn;
                    closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);

                    // Checking all player controller objects in the list of players in the instance of the game manager
                    foreach (PlayerController player in GameManager.instance.players)
                    {
                        if (Vector3.Distance(pawn.transform.position, player.pawn.transform.position) <= closestTankDistance)
                        {
                            closestTank = player.pawn;
                            closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);
                        }
                    }

                    target = closestTank.gameObject;
                }
            }
        }
    }

    protected bool HasTarget()
    {
        return target != null;
    }

    #endregion Targeting Options
}