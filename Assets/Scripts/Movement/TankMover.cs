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
        // Tells the game object how to rotate
        transform.Rotate(0, turnSpeed, 0);
    }
}