using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Allows objects of this class to appear in the editor
[System.Serializable]
public class PlayerGamepadController : Controller
{
    // Using Unity's Input System
    private TankGamepadInput gamepadInput;

    // Reference to a pawn object
    public PlayerTankPawn pawn;

    // Variables to get values from the gamepad buttons
    private float forward = 0;
    private float backward = 0;
    private float right = 0;
    private float left = 0;
    private float directionX = 0;
    private float directionY = 0;

    public void Awake()
    {
        gamepadInput = new TankGamepadInput();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        AudioManager.instance.PlaySound("Player Tank Creation", pawn.transform);

        // Adds itself to the game manager
        if (GameManager.instance != null && GameManager.instance.gamepadPlayers != null)
        {
            GameManager.instance.gamepadPlayers.Add(this);
        }

        pawn.GetComponent<UIManager>().UpdateLivesUI();

        gamepadInput.GamepadControls.MoveForward.performed += context => forward = context.ReadValue<float>();
        gamepadInput.GamepadControls.MoveForward.canceled += context => forward = 0;

        gamepadInput.GamepadControls.MoveBackward.performed += context => backward = context.ReadValue<float>();
        gamepadInput.GamepadControls.MoveBackward.canceled += context => backward = 0;

        gamepadInput.GamepadControls.RotateRight.performed += context => right = context.ReadValue<float>();
        gamepadInput.GamepadControls.RotateRight.canceled += context => right = 0;

        gamepadInput.GamepadControls.RotateLeft.performed += context => left = context.ReadValue<float>();
        gamepadInput.GamepadControls.RotateLeft.canceled += context => left = 0;

        gamepadInput.GamepadControls.CameraLeftRight.performed += context => directionX = context.ReadValue<float>();
        gamepadInput.GamepadControls.CameraLeftRight.canceled += context => directionX = 0;

        gamepadInput.GamepadControls.CameraUpDown.performed += context => directionY = context.ReadValue<float>();
        gamepadInput.GamepadControls.CameraUpDown.canceled += context => directionY = 0;

        gamepadInput.GamepadControls.Shoot.performed += context => Shoot();
        gamepadInput.GamepadControls.ZoomCameraIn.performed += context => ZoomCameraIn();
        gamepadInput.GamepadControls.ZoomCameraOut.performed += context => ZoomCameraOut();
        gamepadInput.GamepadControls.ToggleMenuEnabled.performed += context => ToggleMenu();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ProcessInputs();
    }

    public void OnDestroy()
    {
        // Instance of player controller in list already exists
        if (GameManager.instance != null && GameManager.instance.players != null)
        {
            GameManager.instance.gamepadPlayers.Remove(this);
        }

        if (LivesManager.instance != null)
        {
            LivesManager.instance.RespawnPlayer();
        }
    }

    // Player will emit no noise that the AI can hear while not doing anything
    public override void ProcessInputs()
    {
        MoveForward();
        MoveBackward();
        RotateLeft();
        RotateRight();
        MoveCameraAxisX();
        MoveCameraAxisY();

        // Yes, this is the only way I see to check if nothing is being pressed using Unity's Input System
        if (gamepadInput.GamepadControls.MoveForward.IsPressed() == false &&
            gamepadInput.GamepadControls.MoveBackward.IsPressed() == false &&
            gamepadInput.GamepadControls.RotateLeft.IsPressed() == false &&
            gamepadInput.GamepadControls.RotateRight.IsPressed() == false)
        {
            AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle High");
            AudioManager.instance.PlayLoopingSound("Player Tank Idle", pawn.transform);

            pawn.GetComponent<NoiseEmitter>().EmitNoNoise();
        }
    }

    public void MoveForward()
    {
        AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle");
        AudioManager.instance.PlayLoopingSound("Player Tank Idle High", pawn.transform);

        // Tells the given pawn to move forward, whatever pawn this script is attached to
        pawn.MoveForward(forward);

        pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
    }

    public void MoveBackward()
    {
        AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle");
        AudioManager.instance.PlayLoopingSound("Player Tank Idle High", pawn.transform);

        pawn.MoveBackward(backward);

        pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
    }

    public void RotateLeft()
    {
        AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle");
        AudioManager.instance.PlayLoopingSound("Player Tank Idle High", pawn.transform);

        pawn.RotateBodyCounterclockwise(left);

        pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
    }

    public void RotateRight()
    {
        AudioManager.instance.StopSoundIfItsPlaying("Player Tank Idle");
        AudioManager.instance.PlayLoopingSound("Player Tank Idle High", pawn.transform);

        pawn.RotateBodyClockwise(right);

        pawn.GetComponent<NoiseEmitter>().EmitMovementNoise();
    }

    public void Shoot()
    {
        // Calls the Shoot function in the Pawn class
        pawn.Shoot();
        pawn.GetComponent<NoiseEmitter>().EmitShootNoise();
    }

    public void MoveCameraAxisX()
    {
        pawn.MoveCameraHorizontally(directionX * Time.deltaTime);
    }

    public void MoveCameraAxisY()
    {
        pawn.MoveCameraVertically(directionY * Time.deltaTime);
    }

    public void ZoomCameraIn()
    {
        pawn.ZoomCameraIn();
    }

    public void ZoomCameraOut()
    {
        pawn.ZoomCameraOut();
    }

    public void ToggleMenu()
    {
        MenuManager.instance.ToggleInGameSettingsMenu();
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

    public void OnEnable()
    {
        gamepadInput.GamepadControls.Enable();
    }

    public void OnDisable()
    {
        gamepadInput.GamepadControls.Disable();
    }

    public override void Die()
    {
        base.Die();
    }
}