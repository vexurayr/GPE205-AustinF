using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankPawn : Pawn
{
    // For rotating the tank segments individually
    public GameObject tankHead;
    public GameObject tankBodyHeadConnection;
    public GameObject tankBodyPivotPoint;

    // For camera movement
    public new Camera camera;
    public GameObject cameraPivotPoint;
    [Range(0, 90)] public float maxPitchAngleUp;
    [Range(0, 90)] public float maxPitchAngleDown;
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    [Range(0.0f, 1.0f)] public float mouseSmoothTime;

    // To track camera data
    private Vector3 cameraZoom = new Vector3(1, 1, 1);
    private float cameraPitch = 0.0f;
    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    // For per-player score tracking and displaying
    private int score = 0;

    // To only play reload sound once per shot
    private bool isReloadSoundReadyToPlay = false;

    private TankWheelAnimator wheelAnimator;
    private TankTreadAnimator treadAnimator;

    // Start is called before the first frame update
    public override void Start()
    {
        // Use base."Name"(); if you want to call the parent's function
        base.Start();

        score = ScoreManager.instance.GetCurrentScore();

        GetComponent<UIManager>().UpdateScoreUI();

        wheelAnimator = GetComponent<TankWheelAnimator>();
        treadAnimator = GetComponent<TankTreadAnimator>();
    }

    public override void Update()
    {
        base.Update();

        if (GetComponentInParent<UIManager>() != null)
        {
            GetComponentInParent<UIManager>().UpdateShootCooldownUI();
        }

        if (timeUntilNextEvent <= 0 && isReloadSoundReadyToPlay)
        {
            isReloadSoundReadyToPlay = false;

            AudioManager.instance.PlaySound("Player Tank Reload", gameObject.transform);
        }
    }

    public override void MoveForward()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        try
        {
            // Calls the Move function in the mover class, but the function in TankMover is run because that is the script attached to the pawn
            mover.Move(tankBody.transform.forward, moveSpeed);

            wheelAnimator.Forward();
            treadAnimator.Forward();
        }
        catch(Exception)
        {}
    }

    public void MoveForward(float delta)
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        try
        {
            // Calls the Move function in the mover class, but the function in TankMover is run because that is the script attached to the pawn
            mover.Move(tankBody.transform.forward, delta * moveSpeed);

            wheelAnimator.Forward();
            treadAnimator.Forward();
        }
        catch (Exception)
        {}
    }

    public override void MoveBackward()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        try
        {
            mover.Move(tankBody.transform.forward, -moveSpeed);

            wheelAnimator.Backwards();
            treadAnimator.Backwards();
        }
        catch (Exception)
        {}
    }

    public void MoveBackward(float delta)
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        try
        {
            mover.Move(tankBody.transform.forward, delta * -moveSpeed);

            wheelAnimator.Backwards();
            treadAnimator.Backwards();
        }
        catch (Exception)
        {}
    }

    public void RotateBodyClockwise()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        try
        {
            mover.RotateBody(tankBodyPivotPoint, turnSpeed);
            // Makes sure tank head stays attached to the tank body in the right place
            tankHead.transform.position = tankBodyHeadConnection.transform.position;

            wheelAnimator.Clockwise();
            treadAnimator.Clockwise();
        }
        catch (Exception)
        {}
    }

    public void RotateBodyClockwise(float delta)
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        try
        {
            mover.RotateBody(tankBodyPivotPoint, delta * turnSpeed);
            // Makes sure tank head stays attached to the tank body in the right place
            tankHead.transform.position = tankBodyHeadConnection.transform.position;

            wheelAnimator.Clockwise();
            treadAnimator.Clockwise();
        }
        catch(Exception)
        {}
    }

    public void RotateBodyCounterclockwise()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        try
        {
            mover.RotateBody(tankBodyPivotPoint, -turnSpeed);
            tankHead.transform.position = tankBodyHeadConnection.transform.position;

            wheelAnimator.Counterclockwise();
            treadAnimator.Counterclockwise();
        }
        catch (Exception)
        {}
    }

    public void RotateBodyCounterclockwise(float delta)
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        try
        {
            mover.RotateBody(tankBodyPivotPoint, delta * -turnSpeed);
            tankHead.transform.position = tankBodyHeadConnection.transform.position;

            wheelAnimator.Counterclockwise();
            treadAnimator.Counterclockwise();
        }
        catch (Exception)
        {}
    }

    public override void Shoot()
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
    public override void ShootCooldown()
    {
        if (timeUntilNextEvent <= 0)
        {
            timeUntilNextEvent = secondsPerShot;

            isReloadSoundReadyToPlay = true;
        }
    }

    // Used by PlayerController
    public void MoveCamera()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        // Saves the x and y position of the mouse on the screen
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Smooths camera movement
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        // Saves the camera's vertical rotation as the inverse of mouse movement
        cameraPitch -= currentMouseDelta.y * mouseSensitivityY;

        // Guarentees the camera does not rotate up or down beyond a certain point
        cameraPitch = Mathf.Clamp(cameraPitch, maxPitchAngleUp, maxPitchAngleDown + maxPitchAngleUp);

        // Rotates the camera vertically
        cameraPivotPoint.transform.localEulerAngles = Vector3.right * cameraPitch;

        // Rotates the top of the tank left and right based on the mouse's x (horizontal) position
        tankHead.transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivityX);

        cameraZoom.y += (Input.mouseScrollDelta.y * -1);
        cameraZoom.z += (Input.mouseScrollDelta.y * -1);

        if (cameraZoom.y <= 1 || cameraZoom.z <= 1)
        {
            cameraZoom.y = 1;
            cameraZoom.z = 1;
        }
        else if (cameraZoom.y >= 4 || cameraZoom.z >= 4)
        {
            cameraZoom.y = 4;
            cameraZoom.z = 4;
        }

        cameraPivotPoint.transform.localScale = cameraZoom;
    }

    // Following camera move functions used by PlayerGamepadController
    public void MoveCameraVertically(float direction)
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        // Saves the camera's vertical rotation as the inverse of mouse movement
        cameraPitch -= direction * mouseSensitivityY;

        // Guarentees the camera does not rotate up or down beyond a certain point
        cameraPitch = Mathf.Clamp(cameraPitch, maxPitchAngleUp, maxPitchAngleDown + maxPitchAngleUp);

        // Rotates the camera vertically
        cameraPivotPoint.transform.localEulerAngles = Vector3.right * cameraPitch;
    }

    public void MoveCameraHorizontally(float direction)
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        // Rotates the top of the tank left and right based on the mouse's x (horizontal) position
        tankHead.transform.Rotate(Vector3.up * direction * mouseSensitivityX);
    }

    public void ZoomCameraIn()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        cameraZoom.y -= 1;
        cameraZoom.z -= 1;

        if (cameraZoom.y <= 1 || cameraZoom.z <= 1)
        {
            cameraZoom.y = 1;
            cameraZoom.z = 1;
        }

        cameraPivotPoint.transform.localScale = cameraZoom;
    }

    public void ZoomCameraOut()
    {
        if (isStunned || isGamePaused)
        {
            return;
        }

        cameraZoom.y += 1;
        cameraZoom.z += 1;

        if (cameraZoom.y >= 4 || cameraZoom.z >= 4)
        {
            cameraZoom.y = 4;
            cameraZoom.z = 4;
        }

        cameraPivotPoint.transform.localScale = cameraZoom;
    }

    public void AddPoints(int pointsToAdd)
    {
        score = score + pointsToAdd;
    }

    public int GetScore()
    {
        return score;
    }

    public override void Die()
    {
        base.Die();
    }
}