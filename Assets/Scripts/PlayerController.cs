using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    // Creates a new field representing a button that can be set in the editor
    public KeyCode keyForward = KeyCode.W;
    public KeyCode keyBackward = KeyCode.S;
    public KeyCode keyRotateLeft = KeyCode.A;
    public KeyCode keyRotateRight = KeyCode.D;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ProcessInputs();
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
        }
        if (Input.GetKey(keyBackward))
        {
            pawn.MoveBackward();
        }
        if (Input.GetKey(keyRotateLeft))
        {
            pawn.RotateCounterclockwise();
        }
        if (Input.GetKey(keyRotateRight))
        {
            pawn.RotateClockwise();
        }
    }
}
