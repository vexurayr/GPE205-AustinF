using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaxHealthPowerup : Powerup
{
    public float healingAmount;

    // This will give the target the healingAmount to their health component
    public override void Apply(PowerupManager target)
    {
        // Gets Health Component
        Health targetHealth = target.GetComponent<Health>();

        // Increases max health and current health by healing amount
        if (targetHealth != null)
        {
            targetHealth.IncreaseMaxHealth(healingAmount);
            targetHealth.Heal(healingAmount);
        }
    }

    // We don't want to take max health away, so this is empty
    // The powerup can be safely removed from the PowerupManager
    public override void Remove(PowerupManager target)
    {}
}