using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows objects of this class to appear in the editor
[System.Serializable]
public class PlayerController : Controller
{
    // Creates a new field representing a button that can be set in the editor
    public KeyCode keyForward = KeyCode.W;
    public KeyCode keyBackward = KeyCode.S;
    public KeyCode keyRotateLeft = KeyCode.A;
    public KeyCode keyRotateRight = KeyCode.D;
    public KeyCode swapCameraView = KeyCode.V;
    public KeyCode shootKey = KeyCode.Mouse0; // Left-click

    private TankWheelAnimator wheelAnimator;
    private TankTreadAnimator treadAnimator;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // Adds itself to the game manager
        if (GameManager.instance != null && GameManager.instance.players != null)
        {
            GameManager.instance.players.Add(this);
        }

        wheelAnimator = pawn.GetComponent<TankWheelAnimator>();
        treadAnimator = pawn.GetComponent<TankTreadAnimator>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ProcessInputs();
    }

    // Built in function runs when objects is destroyed

    public void OnDestroy()
    {
        // Instance of player controller in list already exists
        if (GameManager.instance != null && GameManager.instance.players != null)
        {
            GameManager.instance.players.Remove(this);
        }
    }

    // Function that uses the keycodes to access the pawn referrence in the parent class
    // to run the functions in the TankPawn class
    public override void ProcessInputs()
    {
        // Get Key runs as long as it's held, Get Key Down runs once on being pressed, Get Key Up runs once on being released
        if (Input.GetKey(keyForward))
        {
            // Tells the given pawn to move forward, whatever pawn this script is attached to
            pawn.MoveForward();
            wheelAnimator.Forward();
            treadAnimator.Forward();
        }
        if (Input.GetKey(keyBackward))
        {
            pawn.MoveBackward();
            wheelAnimator.Backwards();
            treadAnimator.Backwards();
        }
        if (Input.GetKey(keyRotateLeft))
        {
            pawn.RotateCounterclockwise();
            wheelAnimator.Counterclockwise();
            treadAnimator.Counterclockwise();
        }
        if (Input.GetKey(keyRotateRight))
        {
            pawn.RotateClockwise();
            wheelAnimator.Clockwise();
            treadAnimator.Clockwise();
        }

        if (Input.GetKeyDown(shootKey))
        {
            // Calls the Shoot function in the Pawn class
            pawn.Shoot();
        }

        if (Input.GetKeyDown(swapCameraView))
        {
            pawn.SwitchCamera();
        }
    }
}