using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
    // Tracks how far away the noise can be heard from
    public float movementVolumeDistance;
    public float shootVolumeDistance;

    // Tells how many seconds the volume of shooting lingers
    public float timeShootVolumeLingers;

    // Used to store either 0 or the values from the other variables
    private float currentNoiseDistance = 0f;

    // Used to make sure louder sounds aren't cut off
    private float timeShootVolumeCountdown;
    private bool makingLingeringNoise = false;

    public void EmitMovementNoise()
    {
        currentNoiseDistance = movementVolumeDistance;
    }

    // Noise from tank shot lingers
    public void EmitShootNoise()
    {
        makingLingeringNoise = true;

        currentNoiseDistance = shootVolumeDistance;

        timeShootVolumeCountdown = timeShootVolumeLingers;

        while (timeShootVolumeCountdown > 0)
        {
            timeShootVolumeCountdown -= Time.deltaTime;
        }

        makingLingeringNoise = false;
    }

    // Won't emit no noise if there is lingering sound
    public void EmitNoNoise()
    {
        if (!makingLingeringNoise)
        {
            currentNoiseDistance = 0f;
        }
    }

    // Returns value used by AI tanks
    public float GetCurrentNoiseDistance()
    {
        return currentNoiseDistance;
    }
}