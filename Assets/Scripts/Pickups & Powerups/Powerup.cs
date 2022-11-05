using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Doesn't need MonoBehaviour beceause this class won't be a game object
public abstract class Powerup
{
    public float powerupDuration;
    public bool isPermanent;
    protected string powerupName;

    // This method will be called when the item is picked up
    public virtual void Apply(PowerupManager target)
    {}

    // This method will be called when the item's effect wears off
    public virtual void Remove(PowerupManager target)
    {}

    public virtual string GetName()
    {
        return powerupName;
    }

    public virtual void SetName(string powerupName)
    {
        this.powerupName = powerupName;
    }
}