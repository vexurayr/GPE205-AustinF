using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankPawn : Pawn
{
    // For UI
    [SerializeField] public Text cooldownText;
    [SerializeField] public Text healthText;

    // For disabling controls if the pawn is stunned
    private bool isStunned = false;
    private float stunTime = 0f;

    // Start is called before the first frame update
    public override void Start()
    {
        // Use base."Name"(); if you want to call the parent's function
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        Stun();

        if (timeUntilNextEvent >= 0)
        {
            timeUntilNextEvent -= Time.deltaTime;
            timeUntilNextEvent = Mathf.Clamp(timeUntilNextEvent, 0, secondsPerShot);
        }
        if (cooldownText != null)
        {
            cooldownText.text = "Shoot Cooldown: " + timeUntilNextEvent.ToString("n2");
        }
        if (healthText != null)
        {
            healthText.text = "Health: " + GetComponent<Health>().GetHealth();
        }
    }

    public void SetStunnedTrue(float stunTime)
    {
        isStunned = true;
        this.stunTime = stunTime;
    }

    public void Stun()
    {
        if (isStunned)
        {
            // Counts down until tank is no longer stunned
            stunTime -= Time.deltaTime;
        }
        if (stunTime <= 0f)
        {
            isStunned = false;
        }
    }

    // Inherits mover object from Pawn class and gets the component from Pawn's Start()
    public override void MoveForward()
    {
        if (!isStunned)
        {
            // Calls the Move function in the mover class, but the function in TankMover is run because that is the script attached to the pawn
            mover.Move(transform.forward, moveSpeed);
        }
    }

    public override void MoveBackward()
    {
        if (!isStunned)
        {
            mover.Move(transform.forward, -moveSpeed);
        }
    }

    public override void RotateClockwise()
    {
        if (!isStunned)
        {
            mover.Rotate(turnSpeed);
        }
    }

    public override void RotateCounterclockwise()
    {
        if (!isStunned)
        {
            mover.Rotate(-turnSpeed);
        }
    }

    // Method more so for AI tanks
    public override void RotateTowards(Vector3 targetPosition)
    {
        if (!isStunned)
        {
            // Find the vector from current position to the target's position
            Vector3 vectorToTarget = targetPosition - transform.position;

            // Find the rotation necessary to look down the direction of the vector above
            Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);

            // Rotate accordingly, closer to the vector
            // turnSpeed * Time.deltaTime made this move far differently than rotate clockwise/counterclockwise
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed);
        }
    }

    public override void Shoot()
    {
        if (!isStunned)
        {
            if (timeUntilNextEvent <= 0)
            {
                // Calls a function in Shooter using the reference in Pawn and gives the data saved in Pawn
                shooter.Shoot(shellPrefab, fireForce, shellLifeSpan);

                // Starts the cooldown after the player has shot
                ShootCooldown();

                if (GetComponent<PowerupManager>() != null)
                {
                    GetComponent<PowerupManager>().RemoveStunPowerup();
                }
            }
        }
    }

    // A countdown method timer
    public override void ShootCooldown()
    {
        if (timeUntilNextEvent <= 0)
        {
            timeUntilNextEvent = secondsPerShot;
        }
    }
}