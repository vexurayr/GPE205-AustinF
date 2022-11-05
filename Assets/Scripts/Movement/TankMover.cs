using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{
    // Gets a reference to the rigidbody component
    private Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    public override void Start()
    {
        m_Rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    public override void Move(Vector3 direction, float speed)
    {
        // Gives a value in units per second
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        // Applies movement to rigidbody instead of transform to make sure collisions are consistant
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveVector);
    }

    public override void Rotate(float turnSpeed)
    {
        m_Rigidbody.transform.Rotate(0, turnSpeed, 0);
    }

    public override void RotateBody(GameObject tankBodyPivotPoint, float turnSpeed)
    {
        // Tells the game object how to rotate
        tankBodyPivotPoint.transform.Rotate(0, turnSpeed, 0);
    }

    public override void RotateHead(GameObject tankHead, float turnSpeed)
    {
        tankHead.transform.Rotate(0, turnSpeed, 0);
    }

    // Method more so for AI tanks (For some reason, trying to rotate only the head or body just doesn't work)
    public override void RotateTowards(Vector3 targetPosition, float turnSpeed)
    {
        // Find the vector from current position to the target's position
        Vector3 vectorToTarget = targetPosition - transform.position;

        // Find the rotation necessary to look down the direction of the vector above
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);

        // Rotate accordingly, closer to the vector
        // turnSpeed * Time.deltaTime made this move far differently than rotate clockwise/counterclockwise
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed);
    }
}