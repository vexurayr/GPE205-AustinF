using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damageDone;
    public Pawn owner;

    public virtual void Start()
    {
        AudioManager.instance.PlaySound("All Tank Shoot", gameObject.transform);
    }

    // Built in function that detects when colliders of pawns are overlapping
    public virtual void OnTriggerEnter(Collider other)
    {
        // Makes sure bullet didn't collide with tank that fired it
        if (other != owner)
        {
            AudioManager.instance.PlaySound("All Bullet Hit", gameObject.transform);

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
}