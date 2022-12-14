using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows objects of this class to appear in the editor
[System.Serializable]
public class PlayerController : Controller
{
    // Reference to a pawn object
    public PlayerTankPawn pawn;

    // Creates a new field representing a button that can be set in the editor
    public KeyCode keyForward = KeyCode.W;
    public KeyCode keyBackward = KeyCode.S;
    public KeyCode keyRotateLeft = KeyCode.A;
    public KeyCode keyRotateRight = KeyCode.D;
    public KeyCode keyShoot = KeyCode.Mouse0; // Left-click

    public bool isCursorLocked;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        AudioManager.instance.PlaySound("Player Tank Creation", pawn.transform);

        // Adds itself to the game manager
        if (GameManager.instance != null && GameManager.instance.players != null)
        {
            GameManager.instance.players.Add(this);
        }

        // Adds itself to the score manager
        if (ScoreManager.instance != null && ScoreManager.instance.players != null)
        {
            ScoreManager.instance.players.Add(this);
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

        pawn.GetComponent<UIManager>().UpdateLivesUI();
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

        if (ScoreManager.instance != null && ScoreManager.instance.players != null)
        {
            ScoreManager.instance.players.Remove(this);
        }

        if (LivesManager.instance != null && GameManager.instance != null)
        {
            LivesManager.instance.RespawnPlayer();
        }
    }

    // Function that uses the keycodes to access the pawn referrence in the parent class
    // to run the functions in the TankPawn class
    public override void ProcessInputs()
    {
        // Get Key runs as long as it's held, Get Key Down runs once on being pressed, Get Key Up runs once on being released
        if (Input.GetKey(keyForward))
        {
            AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle");
            AudioManager.instance.PlayLoopingSound("Player Tank Idle High", pawn.transform);

            // Tells the given pawn to move forward, whatever pawn this script is attached to
            pawn.MoveForward();
            
            pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
        }
        if (Input.GetKey(keyBackward))
        {
            AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle");
            AudioManager.instance.PlayLoopingSound("Player Tank Idle High", pawn.transform);

            pawn.MoveBackward();
            
            pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
        }
        if (Input.GetKey(keyRotateLeft))
        {
            AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle");
            AudioManager.instance.PlayLoopingSound("Player Tank Idle High", pawn.transform);

            pawn.RotateBodyCounterclockwise();
            
            pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
        }
        if (Input.GetKey(keyRotateRight))
        {
            AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle");
            AudioManager.instance.PlayLoopingSound("Player Tank Idle High", pawn.transform);

            pawn.RotateBodyClockwise();
            
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
            AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle High");
            AudioManager.instance.PlayLoopingSound("Player Tank Idle", pawn.transform);

            pawn.GetComponent<NoiseEmitter>().EmitNoNoise();
        }

        pawn.MoveCamera();
    }

    // The UI will update when the player's health/max health changes or when they shoot
    public void UpdateHealthUI()
    {
        if (pawn.GetComponent<UIManager>() != null)
        {
            pawn.GetComponent<UIManager>().UpdateHealthUI();
        }
    }

    public void UpdateShootCooldownUI()
    {
        if (pawn.GetComponent<UIManager>() != null)
        {
            pawn.GetComponent<UIManager>().UpdateShootCooldownUI();
        }
    }

    public override void Die()
    {
        base.Die();
    }
}