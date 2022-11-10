using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPickup : Pickup
{
    public MaxHealthPowerup powerup;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        powerup.SetName("MaxHealth");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public virtual void OnTriggerEnter(Collider obj)
    {
        // Variable stores colliding object's PowerupManager
        PowerupManager powerupManager = obj.GetComponent<PowerupManager>();

        // Can pick this item up no matter what
        if (powerupManager != null)
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