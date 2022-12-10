using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : Pickup
{
    public SpeedPowerup powerup;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        powerup.SetName("Speed");
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

        // Checks if object has powerup manager
        if (powerupManager != null)
        {
            PlayPickupSound();

            // Adds the powerup to the manager for it to be applied to the pawn
            powerupManager.Add(powerup);

            // Destroys the pickup when it's collected
            Destroy(gameObject);
        }
    }
}