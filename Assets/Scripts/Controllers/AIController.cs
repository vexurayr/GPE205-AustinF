using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AIController : Controller
{
    #region Variables
    // Last time the AI changed states
    protected float lastTimeStateChanged;

    // All states in finite state machine
    public enum AIState { Idle, Scanning, Chase, SeekAndAttack, Flee, RandomMovement, RandomObserve, AttackThenFlee, Patrol, FaceNoise,
        StationaryAttack, SeekNoise, DistanceAttack, AttackWhileFleeing };

    // State the fsm is currently in
    public AIState currentState;

    // AI controller's target, likely the player
    public GameObject target;

    // Variables for affecting AI sight and hearing
    public float aIFOV;
    public float eyesightDistance;
    public float earshotDistance;

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
    public List<GameObject> waypoints;
    public bool isWaypointPathLooping;
    public float waypointStopDistance;
    protected int currentWaypoint = 0;
    protected bool isBacktrackingWaypoints = false;

    // Used for scan method
    private Vector3 startDirection;
    private bool isRotatingClockwise = true;
    private bool isDegreeLargerThan90 = false;
    private float rotateTimer = 0f;

    // Used for random movement & random observe
    private Vector3 randomLocation;
    protected bool hasDoneRandomTask = true;
    private int randomRotation;

    // Used for shoot and flee
    protected float currentTime = 0;

    // Used for Face Noise and Seek Noise
    protected GameObject noiseLocation;

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

        startDirection = pawn.transform.forward;

        noiseLocation = new GameObject();
    }

    #endregion MonoBehavior

    #region Conditions & Transitions
    public bool IsDistanceLessThan(GameObject target, float distance)
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

    public bool CanHearTarget()
    {
        // If the target has no instance of noise emitter, can't continue
        // because NoiseEmitter holds the necessary data for calculations
        if (target.GetComponent<NoiseEmitter>() == null)
        {
            return false;
        }

        // Player could still be "heard" in if statement below if earshotDistance alone were greater than distance between self
        // and target, so must guarentee they can't be heard if they are emitting no noise
        if (target.GetComponent<NoiseEmitter>().GetCurrentNoiseDistance() == 0)
        {
            return false;
        }

        // If distance between self and target is less than the distance of sound the target is currently emitting
        // plus the distance self can hear from, AI can hear the sound
        if (IsDistanceLessThan(target, target.GetComponent<NoiseEmitter>().GetCurrentNoiseDistance() + earshotDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSee(GameObject obj)
    {
        // Gets the vector between self and target
        Vector3 selfToTargetVector = obj.transform.position - transform.position;

        // Gets angle between that vector and the direction self is facing
        float angleToTarget = Vector3.Angle(selfToTargetVector, pawn.transform.forward);

        // Angle is less than AI POV and distance to target is less than eyesight distance
        // meaning the player is within the cone the AI can see
        if (angleToTarget < aIFOV && IsDistanceLessThan(obj, eyesightDistance))
        {
            // Now must check if the player is in line of sight
            // Sends ray from self in direction of target location
            RaycastHit targetToHit;
            Ray rayToTarget = new Ray(pawn.raycastLocation.transform.position, selfToTargetVector);

            // Ray is able to hit something
            if (Physics.Raycast(rayToTarget, out targetToHit, eyesightDistance))
            {
                // Check if ray hit target
                if (targetToHit.collider == obj.GetComponent<Collider>())
                {
                    Debug.Log("I can see the " + obj + "!");
                    return true;
                }
                // Ray didn't hit target
                else
                {
                    Debug.Log("Ray didn't hit target");
                    return false;
                }
            }
            // Ray didn't hit anything
            else
            {
                Debug.Log("Ray didn't hit anything");
                return false;
            }
        }
        // Target was not in cone of vision
        else
        {
            Debug.Log("Target was not in cone of vision");
            return false;
        }
    }

    public bool CanSeeTarget()
    {
        return CanSee(target);
    }

    // Have to not see pickups that the AI can't pick up
    public bool CanSeePickup()
    {
        List<GameObject> pickups = GameManager.instance.pickups;

        if (pickups != null)
        {
            foreach (GameObject pickup in pickups)
            {
                if (pickup != null && CanSee(pickup))
                {
                    // Will chase health pickup if less than max health
                    if (pickup.GetComponent<HealthPickup>() != null && 
                        pawn.GetComponent<Health>().maxHealth != pawn.GetComponent<Health>().GetHealth())
                    {
                        Target(pickup);
                        return true;
                    }
                    // Won't target speed pickup if it already has one in its powerup manager
                    else if (pickup.GetComponent<SpeedPickup>() != null && 
                        !pawn.GetComponent<PowerupManager>().HasSpeedPowerup())
                    {
                        Target(pickup);
                        return true;
                    }
                    // Won't target stun pickup if it already has one in its powerup manager
                    else if (pickup.GetComponent<StunPickup>() != null &&
                        !pawn.GetComponent<PowerupManager>().HasStunPowerup())
                    {
                        Debug.Log("Why are you doing this?");
                        Target(pickup);
                        return true;
                    }
                }
            }
        }
        return false;
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
    public void Idle()
    {}

    // Polymorphism at its finest
    public void Seek(Vector3 targetVector)
    {
        pawn.RotateTowards(targetVector);

        // Keeps the AI from getting too close to the player
        if (!IsDistanceLessThan(target, nextToPlayerDistance))
        {
            pawn.MoveForward();
        }
        // For cases when AI is trying to flee but it is too close to the player to move
        else if (IsDistanceLessThan(target, nextToPlayerDistance) && currentState == AIState.Flee)
        {
            pawn.MoveForward();
        }
        else if (IsDistanceLessThan(target, nextToPlayerDistance) && currentState == AIState.AttackThenFlee)
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

    public void SeekExactXAndZ(Vector3 targetVector)
    {
        targetVector.y = 0;
        pawn.RotateTowards(targetVector);
        pawn.MoveForward();
    }

    public void SeekExactXAndZ(Transform targetTransform)
    {
        SeekExactXAndZ(targetTransform.position);
    }

    public void SeekExactXAndZ(GameObject target)
    {
        SeekExactXAndZ(target.transform);
    }

    public void SeekExactXAndZ(Pawn targetPawn)
    {
        SeekExactXAndZ(targetPawn.transform);
    }

    public void Flee()
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
    public virtual void SeekAndAttack()
    {
        // Continually chases and shoots at target
        Seek(target);

        // AI also limited by shoot timer, only shoots if barrel is lined up with the player
        RaycastHit targetToHit;
        Ray rayToTarget = new Ray(pawn.raycastLocation.transform.position, pawn.raycastLocation.transform.forward);

        if (Physics.Raycast(rayToTarget, out targetToHit, eyesightDistance))
        {
            // Ray hit the player
            if (targetToHit.collider == target.GetComponent<Collider>())
            {
                pawn.Shoot();
            }
        }
    }

    public virtual void Patrol()
    {
        if (isWaypointPathLooping)
        {
            PatrolLoop();
        }
        else
        {
            PatrolLinear();
        }
    }

    // When reaching final waypoint, start backtracking
    public virtual void PatrolLinear()
    {
        // Can't backtrack further at the first waypoint
        if (currentWaypoint == 0)
        {
            isBacktrackingWaypoints = false;
        }

        // Goes to next waypoint in the array of waypoints
        if (waypoints.Count > currentWaypoint)
        {
            Seek(waypoints[currentWaypoint]);

            // If less than x distance away from the next waypoint, move on to the next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].transform.position) < waypointStopDistance
                && !isBacktrackingWaypoints)
            {
                currentWaypoint++;
            }
            // Same as above but in opposite direction
            else if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].transform.position) < waypointStopDistance
                && isBacktrackingWaypoints)
            {
                currentWaypoint--;
            }
        }
        // Only begins backtracking when the whole waypoint array has been iterated over
        else
        {
            isBacktrackingWaypoints = true;
            currentWaypoint--;
        }
    }

    // When reaching final waypoint, go back to first waypoint
    public virtual void PatrolLoop()
    {
        if (waypoints.Count > currentWaypoint)
        {
            Seek(waypoints[currentWaypoint]);

            // If less than x distance away from the next waypoint, move on to the next waypoint
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

    public void RestartPatrol()
    {
        currentWaypoint = 0;
    }

    public void Scan()
    {
        Vector3 currentDirection = pawn.transform.forward;

        // This will pause the tank, won't move until enough time has passed
        if (rotateTimer > 0)
        {
            rotateTimer = rotateTimer - Time.deltaTime;
        }
        else
        {
            // This detects if the tank needs to pause and start rotating the other direction
            if (Vector3.Angle(startDirection, currentDirection) >= 90 && !isDegreeLargerThan90)
            {
                ChangeScanRotation();
                isDegreeLargerThan90 = true;
                rotateTimer = 2;
            }

            if (isRotatingClockwise)
            {
                pawn.RotateClockwise();
            }
            else
            {
                pawn.RotateCounterclockwise();
            }

            // This resets the variable used to determine if the tank needs to rotate the other way
            if (Vector3.Angle(startDirection, currentDirection) < 90)
            {
                isDegreeLargerThan90 = false;
            }
        }
    }

    public void ChangeScanRotation()
    {
        if (isRotatingClockwise)
        {
            isRotatingClockwise = false;
        }
        else
        {
            isRotatingClockwise = true;
        }
    }

    public Vector3 GetRandomPositionAroundSelf()
    {
        // This will pick a direction away from the player on the x and z axis
        Vector3 targetPosition = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));

        // Add those values to the pawn's current position to make a target point centered around the pawn
        targetPosition.x = targetPosition.x + pawn.transform.position.x;
        targetPosition.y= targetPosition.y + pawn.transform.position.y;
        targetPosition.z = targetPosition.z + pawn.transform.position.z;

        return targetPosition;
    }

    public void RandomMovement()
    {
        // This will prevent this tank from constantly looking for a new position to travel to
        if (hasDoneRandomTask)
        {
            Vector3 targetPosition = GetRandomPositionAroundSelf();
            randomLocation = targetPosition;
            hasDoneRandomTask = false;
        }
        
        // The pawn must move and is rotating towards a position so close to itself
        // This usually causes the pawn to drive in circles
        pawn.RotateTowards(randomLocation);

        pawn.MoveForward();
    }

    public void RandomObserve()
    {
        // Gives this tank a new direction to rotate only on entering the Random Observe state
        if (hasDoneRandomTask)
        {
            randomRotation = Random.Range(0, 1);
            hasDoneRandomTask = false;
        }

        if (randomRotation == 0)
        {
            pawn.RotateClockwise();
        }
        else
        {
            pawn.RotateCounterclockwise();
        }
    }

    public void AttackThenFlee()
    {
        // Gives tank enough time to point barrel at player
        if (currentTime >= 0 && currentTime < 2)
        {
            pawn.RotateTowards(target.transform.position);
            currentTime = currentTime + Time.deltaTime;
        }
        // With cooldown and this time window tank should only shoot once
        else if (currentTime >=2 && currentTime < 4)
        {
            pawn.Shoot();
            currentTime = currentTime + Time.deltaTime;
        }
        // Then the tank will attempt to run away
        else
        {
            Flee();
        }
    }

    public void FaceNoise()
    {
        // Simply looks towards the source of the noise
        pawn.RotateTowards(noiseLocation.transform.position);
    }

    public void StationaryAttack()
    {
        // Tank won't move but it will face the target and shoot
        pawn.RotateTowards(target.transform.position);

        // AI also limited by shoot timer, only shoots if barrel is lined up with the player
        RaycastHit targetToHit;
        Ray rayToTarget = new Ray(pawn.raycastLocation.transform.position, pawn.raycastLocation.transform.forward);

        if (Physics.Raycast(rayToTarget, out targetToHit, eyesightDistance))
        {
            // Ray hit the player
            if (targetToHit.collider == target.GetComponent<Collider>())
            {
                pawn.Shoot();
            }
        }
    }

    public void SeekNoise()
    {
        pawn.RotateTowards(noiseLocation.transform.position);

        // This will prevent the tank from moving when it reaches the source of the noise
        if (pawn.transform.position != noiseLocation.transform.position)
        {
            pawn.MoveForward();
        }
    }

    public void DistanceAttack()
    {
        pawn.RotateTowards(target.transform.position);

        // This tank will always try to remain Chase Distance away from the player as it shoots
        // +/- 1 so that the tank isn't jittery while trying to remain in one place
        if (IsDistanceLessThan(target, chaseDistance - 1))
        {
            pawn.MoveBackward();
        }
        else if (!IsDistanceLessThan(target, chaseDistance + 1))
        {
            pawn.MoveForward();
        }

        // AI also limited by shoot timer, only shoots if barrel is lined up with the player
        RaycastHit targetToHit;
        Ray rayToTarget = new Ray(pawn.raycastLocation.transform.position, pawn.raycastLocation.transform.forward);

        if (Physics.Raycast(rayToTarget, out targetToHit, eyesightDistance))
        {
            // Ray hit the player
            if (targetToHit.collider == target.GetComponent<Collider>())
            {
                pawn.Shoot();
            }
        }
    }

    public void AttackWhileFleeing()
    {
        // Continues shooting the player while moving backwards
        pawn.RotateTowards(target.transform.position);

        pawn.MoveBackward();

        // AI also limited by shoot timer, only shoots if barrel is lined up with the player
        RaycastHit targetToHit;
        Ray rayToTarget = new Ray(pawn.raycastLocation.transform.position, pawn.raycastLocation.transform.forward);

        if (Physics.Raycast(rayToTarget, out targetToHit, eyesightDistance))
        {
            // Ray hit the player
            if (targetToHit.collider == target.GetComponent<Collider>())
            {
                pawn.Shoot();
            }
        }
    }

    #endregion States

    #region Targeting Options
    public virtual void Target(GameObject newTarget)
    {
        target = newTarget;
        Debug.Log("Target set to: " + newTarget);
    }

    public virtual void Target(Object obj)
    {
        Target(obj.GetComponent<GameObject>());
    }

    public virtual void TargetPlayerOne()
    {
        // Must be instance of game manager and list of players, and have players in the list
        if (GameManager.instance != null)
        {
            if (GameManager.instance.players != null)
            {
                if (GameManager.instance.players.Count > 0)
                {
                    Target(GameManager.instance.players[0].pawn.gameObject);
                }
            }
        }
    }

    public virtual void TargetNearestTank()
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

                    Target(closestTank.gameObject);
                }
            }
        }
    }

    public bool HasTarget()
    {
        return target != null;
    }

    #endregion Targeting Options
}