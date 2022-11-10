using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    // Transform where projectile will spawn
    public Transform firepointTransform;

    // Creates a new shell object, saves the damage it'll do and which object fired it, applies a force to the rigidbody to make it fly
    // and destroys it after a set period of time
    public override void Shoot(GameObject shellPrefab, float fireForce, float lifeSpan)
    {
        GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation);
        DamageOnHit doh = newShell.GetComponent<DamageOnHit>();

        if (doh != null)
        {
            // With multiple projectiles available, best to keep damage numbers with the bullets
            doh.owner = GetComponent<Pawn>();
            //Debug.Log("The owner of the bullet is: " + doh.owner);
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