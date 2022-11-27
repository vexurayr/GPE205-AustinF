using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public HealthPowerup powerup;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        powerup.SetName("Health");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void OnTriggerEnter(Collider obj)
    {
        base.OnTriggerEnter(obj);

        // Variable stores colliding object's PowerupManager
        PowerupManager powerupManager = obj.GetComponent<PowerupManager>();

        // Checks if object has powerup manager and is not already at max health
        if (powerupManager != null && powerupManager.GetComponent<Health>().GetHealth() != 
            powerupManager.GetComponent<Health>().maxHealth)
        {
            // Adds the powerup to the manager for it to be applied to the pawn
            powerupManager.Add(powerup);

            // Remove this pickup from the game manager
            GameManager.instance.pickups.Remove(obj.gameObject);

            // Destroys the pickup when it's collected
            Destroy(gameObject);
        }
    }
}