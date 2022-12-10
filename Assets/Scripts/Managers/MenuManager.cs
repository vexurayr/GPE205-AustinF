using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    // For in game menus
    public GameObject inGameSettingsMenu;
    public Slider inGameMasterVolumeLevel;
    public Slider inGameMusicVolumeLevel;
    public Slider inGameSFXVolumeLevel;
    public GameObject deathScreen;
    public Text deathText;
    public Text finalScoreText;
    public Text finalScoreNumber;
    public Text highScoreText;
    public Text highScoreNumber;

    // For start screen menus
    public GameObject startScreenBackground;
    public GameObject redTankImage;
    public GameObject gameNameText;
    public GameObject blueTankImage;
    public GameObject mainMenu;
    public Slider startScreenMasterVolumeLevel;
    public Slider startScreenMusicVolumeLevel;
    public Slider startScreenSFXVolumeLevel;
    public Text toggleOneOrTwoPlayersButton;
    public Text useDailyMapButton;

    private bool isSettingsMenuActive;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (SettingsManager.instance != null)
        {
            UpdateAllAudioSliderValues();
        }

        if (instance.startScreenBackground.activeInHierarchy == true)
        {
            PlayMainMenuMusic();
        }
        else if (instance.startScreenBackground.activeInHierarchy == false)
        {
            PlayBackgroundMusic();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInGameSettingsMenu();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("RandomMapTesting");

        deathScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleInGameSettingsMenu()
    {
        // Pressing ESC won't bring this menu up in the main menu or if the player has just died
        if (startScreenBackground.activeInHierarchy || deathScreen.activeInHierarchy)
        {
            return;
        }

        ToggleGamePaused();

        UpdateAllAudioSliderValues();

        // The settings menu is currently visible
        if (isSettingsMenuActive)
        {
            isSettingsMenuActive = false;

            inGameSettingsMenu.SetActive(false);

            HideCursor();
        }
        // The settings menu is currently not visible
        else
        {
            isSettingsMenuActive = true;

            inGameSettingsMenu.SetActive(true);

            ShowCursor();
        }
    }

    public void UpdateStartScreenSettings()
    {
        UpdateAllAudioSliderValues();

        if (SettingsManager.instance.GetIsDailyMapSelected())
        {
            useDailyMapButton.text = "Yes";
        }
        else if (!SettingsManager.instance.GetIsDailyMapSelected())
        {
            useDailyMapButton.text = "No";
        }

        if (SettingsManager.instance.GetIsGameOnePlayer())
        {
            toggleOneOrTwoPlayersButton.text = "One";
        }
        else if (!SettingsManager.instance.GetIsGameOnePlayer())
        {
            toggleOneOrTwoPlayersButton.text = "Two";
        }
    }

    public void UpdateAllAudioSliderValues()
    {
        startScreenMasterVolumeLevel.value = SettingsManager.instance.GetMasterVolumeSliderValue();
        startScreenMusicVolumeLevel.value = SettingsManager.instance.GetMusicVolumeSliderValue();
        startScreenSFXVolumeLevel.value = SettingsManager.instance.GetSFXVolumeSliderValue();

        inGameMasterVolumeLevel.value = SettingsManager.instance.GetMasterVolumeSliderValue();
        inGameMusicVolumeLevel.value = SettingsManager.instance.GetMusicVolumeSliderValue();
        inGameSFXVolumeLevel.value = SettingsManager.instance.GetSFXVolumeSliderValue();
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }

    public void HideCursor()
    {
        // Locks the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Makes the cursor invisible
        Cursor.visible = false;
    }

    public void BackToMainMenu()
    {
        StartCoroutine(WaitToGoBackToMainMenu());
    }

    private void NowGoBackToMainMenu()
    {
        DestroyGameManager();

        SceneManager.LoadScene("StartScreen");

        startScreenBackground.SetActive(true);
        redTankImage.SetActive(true);
        gameNameText.SetActive(true);
        blueTankImage.SetActive(true);
        mainMenu.SetActive(true);
    }

    public void ShowDeathScreen()
    {
        ShowCursor();

        ToggleGamePaused();

        if (SettingsManager.instance.GetIsGameOnePlayer())
        {
            deathText.text = "You Died";
            finalScoreText.text = "Final Score:";
            highScoreText.text = "High Score:";
            finalScoreNumber.text = "" + ScoreManager.instance.GetCurrentScore();
            highScoreNumber.text = "" + ScoreManager.instance.GetHighScore();
        }
        // Show different death screen if there are two players
        else
        {
            deathText.text = "Game Over";
            finalScoreText.text = "Player One:";
            highScoreText.text = "Player Two:";

            // Player 1 died, player 2 won
            if (GameManager.instance.players.Count <= 0)
            {
                finalScoreNumber.text = "Loss";
                highScoreNumber.text = "Victory";
            }
            else
            {
                finalScoreNumber.text = "Victory";
                highScoreNumber.text = "Loss";
            }
        }

        deathScreen.SetActive(true);
        ScoreManager.instance.ResetCurrentScore();
    }

    public void HideDeathScreen()
    {
        deathScreen.SetActive(false);
    }

    public void PlayButtonPressedSound()
    {
        AudioManager.instance.PlaySound("All Button Pressed", gameObject.transform);

        StartCoroutine(WaitToReleaseButton());
    }

    public void PlayButtonReleasedSound()
    {
        AudioManager.instance.PlaySound("All Button Released", gameObject.transform);
    }

    private IEnumerator WaitToReleaseButton()
    {
        yield return new WaitForSeconds(0.2f);

        PlayButtonReleasedSound();
    }

    private IEnumerator WaitToGoBackToMainMenu()
    {
        yield return new WaitForEndOfFrame();

        NowGoBackToMainMenu();
    }

    public void PlayMainMenuMusic()
    {
        if (AudioManager.instance.IsSoundAlreadyPlaying("All Background Music"))
        {
            AudioManager.instance.StopSound("All Background Music");
        }
        if (!AudioManager.instance.IsSoundAlreadyPlaying("All Main Menu Music"))
        {
            AudioManager.instance.PlayLoopingSound("All Main Menu Music", gameObject.transform);
        }
    }

    public void PlayBackgroundMusic()
    {
        if (AudioManager.instance.IsSoundAlreadyPlaying("All Main Menu Music"))
        {
            AudioManager.instance.StopSound("All Main Menu Music");
        }
        if (!AudioManager.instance.IsSoundAlreadyPlaying("All Background Music"))
        {
            AudioManager.instance.PlayLoopingSound("All Background Music", gameObject.transform);
        }
    }

    public void ToggleGamePaused()
    {
        List<PlayerController> playerControllers = GameManager.instance.players;
        List<PlayerGamepadController> playerGamepadControllers = GameManager.instance.gamepadPlayers;
        List<AIController> aIControllers = GameManager.instance.aIPlayers;

        foreach (PlayerController playerController in playerControllers)
        {
            playerController.pawn.ToggleIsGamePaused();
        }

        foreach (PlayerGamepadController gamepadController in playerGamepadControllers)
        {
            gamepadController.pawn.ToggleIsGamePaused();
        }

        foreach (AIController aIController in aIControllers)
        {
            aIController.pawn.ToggleIsGamePaused();
        }
    }

    public void ToggleOneOrTwoPlayersText()
    {
        // One Player is not selected, make it selected
        if (!SettingsManager.instance.GetIsGameOnePlayer())
        {
            SettingsManager.instance.ToggleIsGameOnePlayer();
            toggleOneOrTwoPlayersButton.text = "One";
        }
        else if (SettingsManager.instance.GetIsGameOnePlayer())
        {
            SettingsManager.instance.ToggleIsGameOnePlayer();
            toggleOneOrTwoPlayersButton.text = "Two";
        }
    }

    public void ToggleUseDailyMapText()
    {
        // Daily Map not selected, make it selected
        if (!SettingsManager.instance.GetIsDailyMapSelected())
        {
            SettingsManager.instance.ToggleIsDailyMapSelected();
            useDailyMapButton.text = "Yes";
        }
        else if (SettingsManager.instance.GetIsDailyMapSelected())
        {
            SettingsManager.instance.ToggleIsDailyMapSelected();
            useDailyMapButton.text = "No";
        }
    }

    public void DestroyGameManager()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.DestoryGameManager();
        }
    }
}