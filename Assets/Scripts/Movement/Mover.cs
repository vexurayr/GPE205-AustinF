using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    // Start is called before the first frame update
    public abstract void Start();

    public abstract void Move(Vector3 direction, float speed);
    public abstract void Rotate(float turnSpeed);
    public abstract void RotateBody(GameObject tankBodyPivotPoint, float turnSpeed);
    public abstract void RotateHead(GameObject tankHead, float turnSpeed);
    public abstract void RotateTowards(Vector3 targetPosition, float turnSpeed);
}