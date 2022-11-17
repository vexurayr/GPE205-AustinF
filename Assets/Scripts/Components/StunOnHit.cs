using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunOnHit : DamageOnHit
{
    public int stunTime;

    public override void OnTriggerEnter(Collider other)
    {
        // Makes sure bullet didn't collide with tank that fired it
        if (other != owner)
        {
            // Gets components from colliding object
            Health otherHealth = other.gameObject.GetComponent<Health>();
            Pawn tankPawn = other.gameObject.GetComponent<Pawn>();

            // Deals damage to that object only if it has a health component
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(damageDone, owner);
            }

            // Stuns only if that object has a tank pawn
            if (tankPawn != null)
            {
                other.gameObject.GetComponent<Pawn>().SetStunnedTrue(stunTime);
            }

            Destroy(gameObject);
        }
    }
}