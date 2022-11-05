using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not quite like the game manager
// This won't be a singleton, it'll go on any object capable of using powerups
public class PowerupManager : MonoBehaviour
{
    // All active powerups in powerup manager
    public List<Powerup> powerups;
    public List<Powerup> removedPowerupQueue;

    void Start()
    {
        // Makes list empty, not null
        powerups = new List<Powerup>();
        removedPowerupQueue = new List<Powerup>();
    }

    void Update()
    {
        if (powerups.Count > 0)
        {
            DecrementPowerupTimers();
        }
    }

    void LateUpdate()
    {
        if (removedPowerupQueue.Count > 0)
        {
            ApplyRemovePowerupsQueue();
        }
    }

    // This method activates the powerup's effects and adds it to a list of active powerups
    public void Add(Powerup powerup)
    {
        bool canApply = true;
        int indexPowerupExists = 0;
        int index = 0;

        // See if the incoming powerup is already in the powerup manager's list
        foreach (Powerup power in powerups)
        {
            Debug.Log("Comparing " + powerup.GetName() + " to " + power.GetName() + ".");
            if (powerup.GetName() == power.GetName())
            {
                canApply = false;
                indexPowerupExists = index;
            }
            index++;
        }

        // Apply powerup if it's not already in this list
        if (canApply)
        {
            powerup.Apply(this);
            // Add powerup to list
            powerups.Add(powerup);
            Debug.Log("Applied " + powerup.GetName() + " powerup.");
        }
        // If it is in the list, reset the time it lasts for
        else
        {
            powerups[indexPowerupExists].powerupDuration = powerup.powerupDuration;
            Debug.Log("Reset " + powerup.GetName() + " timer.");
        }
    }

    // This method will remove a powerup's effects from the tank pawn
    public void Remove(Powerup powerup)
    {
        // Remove the powerup from the pawn
        powerup.Remove(this);

        // Add powerup to list of powerups that will be removed after the foreach loop is done
        removedPowerupQueue.Add(powerup);

        Debug.Log(powerup.GetName() + " powerup has expired.");
    }

    // Stun powerup is permanent until player shoots
    public void RemoveStunPowerup()
    {
        foreach (Powerup powerup in powerups)
        {
            if (powerup.GetName() == "Stun")
            {
                Remove(powerup);
            }
        }
    }

    public void DecrementPowerupTimers()
    {
        foreach (Powerup powerup in powerups)
        {
            // Can't decrease timer of permanent powerup
            if (!powerup.isPermanent)
            {
                // Decrease the time it has remaining
                powerup.powerupDuration -= Time.deltaTime;

                // Remove powerup if time is up
                if (powerup.powerupDuration <= 0)
                {
                    Remove(powerup);
                }
            }
        }
    }

    private void ApplyRemovePowerupsQueue()
    {
        foreach (Powerup powerup in removedPowerupQueue)
        {
            powerups.Remove(powerup);
        }
        // Clear list for next time powerups need to be removed
        removedPowerupQueue.Clear();
    }

    public bool HasSpeedPowerup()
    {
        foreach (Powerup powerup in powerups)
        {
            if (powerup != null && powerup.GetName() == "Speed")
            {
                return true;
            }
        }
        return false;
    }

    public bool HasStunPowerup()
    {
        foreach (Powerup powerup in powerups)
        {
            if (powerup != null && powerup.GetName() == "Stun")
            {
                return true;
            }
        }
        return false;
    }
}