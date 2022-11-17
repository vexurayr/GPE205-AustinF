using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StunPowerup : Powerup
{
    // The length of the stun is stored in the projectile
    public GameObject stunShell;

    // Used to instantiate normal projectiles again
    private GameObject normalShell;

    // This will make the target's next shot stun whatever it hits
    public override void Apply(PowerupManager target)
    {
        // If this wasn't done through the projectile, it would simply stun whoever grabbed the pickup
        normalShell = target.GetComponent<Pawn>().shellPrefab;
        target.GetComponent<Pawn>().shellPrefab = stunShell;
    }

    // This will remove the ability for the tank pawn to stun
    public override void Remove(PowerupManager target)
    {
        target.GetComponent<Pawn>().shellPrefab = normalShell;
    }
}