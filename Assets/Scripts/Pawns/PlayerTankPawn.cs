using JetBrains.Annotations;
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

    // Start is called before the first frame update
    public override void Start()
    {
        // Use base."Name"(); if you want to call the parent's function
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (GetComponentInParent<UIManager>() != null)
        {
            GetComponentInParent<UIManager>().UpdateShootCooldownUI();
        }
    }

    // These two only work for the player for some reason
    public void RotateBodyClockwise()
    {
        if (!isStunned)
        {
            mover.RotateBody(tankBodyPivotPoint, turnSpeed);
            // Makes sure tank head stays attached to the tank body in the right place
            tankHead.transform.position = tankBodyHeadConnection.transform.position;
        }
    }

    public void RotateBodyCounterclockwise()
    {
        if (!isStunned)
        {
            mover.RotateBody(tankBodyPivotPoint, -turnSpeed);
            tankHead.transform.position = tankBodyHeadConnection.transform.position;
        }
    }

    public void MoveCamera()
    {
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
}