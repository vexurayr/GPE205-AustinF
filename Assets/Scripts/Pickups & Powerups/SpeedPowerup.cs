using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeedPowerup : Powerup
{
    public float speedUpPercent;

    // For resetting speed bonus
    private float normalMoveSpeed;
    private float normalTurnSpeed;

    // This will give the target the speed boost to their movement and turn speed
    public override void Apply(PowerupManager target)
    {
        normalMoveSpeed = target.GetComponent<Pawn>().moveSpeed;
        normalTurnSpeed = target.GetComponent<Pawn>().turnSpeed;

        // Divide by 100 to turn speed up into actual percent
        float newMoveSpeed = normalMoveSpeed + normalMoveSpeed * (speedUpPercent / 100f);
        float newTurnSpeed = normalTurnSpeed + normalTurnSpeed * (speedUpPercent / 100f);

        target.GetComponent<Pawn>().moveSpeed = newMoveSpeed;
        target.GetComponent<Pawn>().turnSpeed = newTurnSpeed;
    }

    // This will remove the speed boost from the tank pawn
    public override void Remove(PowerupManager target)
    {
        target.GetComponent<Pawn>().moveSpeed = normalMoveSpeed;
        target.GetComponent<Pawn>().turnSpeed = normalTurnSpeed;
    }
}