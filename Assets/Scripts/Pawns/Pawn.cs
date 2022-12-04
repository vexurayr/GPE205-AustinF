using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Abstract means an object of this class will never be made or a function will never be called within that class
public abstract class Pawn : MonoBehaviour
{
    /// <summary>
    /// Variables that will determine how fast an inhereting object will move
    /// </summary>
    public float moveSpeed;
    public float turnSpeed;

    protected Mover mover;
    protected Shooter shooter;
    //protected Controller controller;

    // The game object that will be a pawn
    public GameObject tankBody;

    // For shooting
    public GameObject shellPrefab;
    public float fireForce;
    public float shellLifeSpan;
    public float shotsPerSecond;
    protected float secondsPerShot;
    protected float timeUntilNextEvent;

    // For disabling controls if the pawn is stunned
    protected bool isStunned = false;
    protected float stunTime = 0f;

    // For disabling controls if the game is paused
    protected bool isGamePaused = false;

    // Start is called before the first frame update
    // Virtual means child classes can override this method
    // Protected keyword is necessary because no access keyword defaults to private
    // And we don't want outside scripts calling these functions
    public virtual void Start()
    {
        // Gives pawn objects a reference to the mover class
        mover = GetComponent<Mover>();

        // Takes a reference of the Shooter class or its children to call its functions
        shooter = GetComponent<Shooter>();

        secondsPerShot = 1 / shotsPerSecond;

        timeUntilNextEvent = 0;
    }

    public virtual void Update()
    {
        Stun();

        if (timeUntilNextEvent >= 0)
        {
            timeUntilNextEvent -= Time.deltaTime;
            timeUntilNextEvent = Mathf.Clamp(timeUntilNextEvent, 0, secondsPerShot);
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

    public virtual void MoveForward()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        // Calls the Move function in the mover class, but the function in TankMover is run because that is the script attached to the pawn
        mover.Move(tankBody.transform.forward, moveSpeed);
    }

    public virtual void MoveBackward()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        mover.Move(tankBody.transform.forward, -moveSpeed);
    }
    
    public virtual void RotateClockwise()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        mover.Rotate(turnSpeed);
    }

    public virtual void RotateCounterclockwise()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        mover.Rotate(-turnSpeed);
    }

    public virtual void Shoot()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

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

    // A countdown method timer
    public virtual void ShootCooldown()
    {
        if (timeUntilNextEvent <= 0)
        {
            timeUntilNextEvent = secondsPerShot;
        }
    }

    public virtual float GetTimeUntilNextEvent()
    {
        return timeUntilNextEvent;
    }

    public virtual float GetSecondsPerShot()
    {
        return secondsPerShot;
    }

    public virtual void ToggleIsGamePaused()
    {
        isGamePaused = !isGamePaused;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void OnDestroy()
    {
        AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle");
        AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle High");
        AudioManager.instance.StopSoundIfItsPlaying("AI Tank Idle");
        AudioManager.instance.StopSoundIfItsPlaying("AI Tank Idle High");
    }
}