using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damageDone;
    public Pawn owner;

    // Built in function that detects when colliders of pawns are overlapping
    public void OnTriggerEnter(Collider other)
    {
        // Gets health component from colliding objects
        Health otherHealth = other.gameObject.GetComponent<Health>();

        // Deals damage to that object only if it has a health component
        if (otherHealth != null)
        {
            otherHealth.TakeDamage(damageDone, owner);
        }

        Destroy(gameObject);
    }
}