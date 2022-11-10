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
    public KeyCode keyShoot = KeyCode.Mouse0; // Left-click

    public bool isCursorLocked;

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

        if (isCursorLocked)
        {
            // Locks the cursor to the center of the screen
            Cursor.lockState = CursorLockMode.Locked;

            // Makes the cursor invisible
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        wheelAnimator = pawn.GetComponent<TankWheelAnimator>();
        treadAnimator = pawn.GetComponent<TankTreadAnimator>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ProcessInputs();
        UpdateUI();
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
            pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
        }
        if (Input.GetKey(keyBackward))
        {
            pawn.MoveBackward();
            wheelAnimator.Backwards();
            treadAnimator.Backwards();
            pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
        }
        if (Input.GetKey(keyRotateLeft))
        {
            pawn.RotateBodyCounterclockwise();
            wheelAnimator.Counterclockwise();
            treadAnimator.Counterclockwise();
            pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
        }
        if (Input.GetKey(keyRotateRight))
        {
            pawn.RotateBodyClockwise();
            wheelAnimator.Clockwise();
            treadAnimator.Clockwise();
            pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
        }

        if (Input.GetKeyDown(keyShoot))
        {
            // Calls the Shoot function in the Pawn class
            pawn.Shoot();
            pawn.GetComponent<NoiseEmitter>().EmitShootNoise();
        }

        // Player will emit no noise that the AI can hear while not doing anything
        if (Input.anyKey == false)
        {
            pawn.GetComponent<NoiseEmitter>().EmitNoNoise();
        }

        pawn.MoveCamera();
    }

    public void UpdateUI()
    {
        if (UIManager.instance.cooldownText != null)
        {
            float cooldown = pawn.GetComponent<TankPawn>().GetTimeUntilNextEvent();
            UIManager.instance.cooldownText.text = "Shoot Cooldown: " + cooldown.ToString("n2");
        }
        if (UIManager.instance.healthText != null)
        {
            UIManager.instance.healthText.text = "Health: " + pawn.GetComponent<Health>().GetHealth();
        }
    }

    public override void Die()
    {
        // Calls coroutine in game manager to spawn in a new player
        GameManager.instance.RespawnPlayer();

        base.Die();
    }
}