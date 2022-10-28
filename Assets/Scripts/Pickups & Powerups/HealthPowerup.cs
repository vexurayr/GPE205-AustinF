using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthPowerup : Powerup
{
    public float healingAmount;

    // This will give the target the healingAmount to their health component
    public override void Apply(PowerupManager target)
    {
        // Gets Health Component
        Health targetHealth = target.GetComponent<Health>();

        // Only heals if it has a health component and they aren't already at max health
        if (targetHealth != null && targetHealth.GetHealth() != targetHealth.maxHealth)
        {
            targetHealth.Heal(healingAmount);
        }
    }

    // We don't want to take health away, so this is empty
    // The powerup can be safely removed from the PowerupManager
    public override void Remove(PowerupManager target)
    {}
}