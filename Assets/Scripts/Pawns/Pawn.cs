using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract means an object of this class will never be made or a function will never be called within that class
public abstract class Pawn : MonoBehaviour
{
    /// <summary>
    /// Variables that will determine how fast an inhereting object will move
    /// </summary>
    public float moveSpeed = 1;
    public float turnSpeed = 1;

    // Gives pawn objects a reference to the mover class
    protected Mover mover;

    // Objects to allow the player to change perspectives
    [SerializeField] protected Camera firstPersonCamera;
    [SerializeField] protected Camera thirdPersonCamera;

    // Start is called before the first frame update
    // Virtual means child classes can override this method
    // Protected keyword is necessary because no access keyword defaults to private
    // And we don't want outside scripts calling these functions
    protected virtual void Start()
    {
        mover = GetComponent<Mover>();
        firstPersonCamera.enabled = false;
    }

    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void RotateClockwise();
    public abstract void RotateCounterclockwise();
    public abstract void SwitchCamera();
}