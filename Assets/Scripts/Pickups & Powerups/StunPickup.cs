using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunPickup : Pickup
{
    public StunPowerup powerup;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        powerup.SetName("Stun");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void OnTriggerEnter(Collider obj)
    {
        // Variable stores colliding object's PowerupManager
        PowerupManager powerupManager = obj.GetComponent<PowerupManager>();
        bool hasStunBullet = false;

        // Checks if object has powerup manager
        if (powerupManager != null)
        {
            foreach (Powerup powerup in powerupManager.powerups)
            {
                if (powerup.GetName() == "Stun")
                {
                    hasStunBullet = true;
                }
            }

            // Won't collect pickup if it's still in object's powerup manager
            if (!hasStunBullet)
            {
                PlayPickupSound();

                // Adds the powerup to the manager for it to be applied to the pawn
                powerupManager.Add(powerup);

                // Destroys the pickup when it's collected
                Destroy(gameObject);
            }
        }
    }
}