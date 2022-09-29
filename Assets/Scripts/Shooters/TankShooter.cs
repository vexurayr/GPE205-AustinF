using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    // Transform where projectile will spawn
    public Transform firepointTransform;

    // Creates a new shell object, saves the damage it'll do and which object fired it, applies a force to the rigidbody to make it fly
    // and destroys it after a set period of time
    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifeSpan)
    {
        GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation);
        DamageOnHit doh = newShell.GetComponent<DamageOnHit>();

        if (doh != null)
        {
            doh.damageDone = damageDone;
            doh.owner = GetComponent<Pawn>();
        }

        Rigidbody rb = newShell.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firepointTransform.up * fireForce);
        }

        // If the object exists after 2nd variable in seconds, destroy object
        Destroy(newShell, lifeSpan);
    }
}