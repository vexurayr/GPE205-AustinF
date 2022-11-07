using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract means an object of this class will never be made or a function will never be called within that class
public abstract class Pawn : MonoBehaviour
{
    /// <summary>
    /// Variables that will determine how fast an inhereting object will move
    /// </summary>
    public float moveSpeed;
    public float turnSpeed;

    protected Mover mover;
    protected Shooter shooter;
    protected Controller playerController;

    // For rotating the tank segments individually
    public GameObject tankBody;
    public GameObject tankHead;
    public GameObject tankBodyHeadConnection;
    public GameObject tankBodyPivotPoint;

    // For AI sight
    public GameObject raycastLocation;

    // For camera movement
    public GameObject cameraPivotPoint;
    [Range(0, 90)] public float maxPitchAngleUp;
    [Range(0, 90)] public float maxPitchAngleDown;

    // For shooting
    public GameObject shellPrefab;
    public float fireForce;
    public float shellLifeSpan;
    public float shotsPerSecond;
    protected float secondsPerShot;
    protected float timeUntilNextEvent;

    // Start is called before the first frame update
    // Virtual means child classes can override this method
    // Protected keyword is necessary because no access keyword defaults to private
    // And we don't want outside scripts calling these functions
    public virtual void Start()
    {
        // Gives pawn objects a reference to the mover class
        mover = GetComponent<Mover>();
        
        // Takes a reference of the Shooter class or its children to call its functions
        shooter = GetComponent<Shooter>();

        secondsPerShot = 1 / shotsPerSecond;

        timeUntilNextEvent = 0;
    }

    public virtual void Update()
    {}

    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void RotateClockwise();
    public abstract void RotateCounterclockwise();
    public abstract void RotateBodyClockwise();
    public abstract void RotateBodyCounterclockwise();
    public abstract void RotateTowards(Vector3 targetVector);
    public abstract void Shoot();
    public abstract void ShootCooldown();
    public abstract void MoveCamera();
}