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
    public Text highScoreNumber;
    public Text finalScoreNumber;

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

        inGameMasterVolumeLevel.value = SettingsManager.instance.GetMasterVolumeLevel();
        inGameMusicVolumeLevel.value = SettingsManager.instance.GetMusicVolumeLevel();
        inGameSFXVolumeLevel.value = SettingsManager.instance.GetSFXVolumeLevel();

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
        startScreenMasterVolumeLevel.value = SettingsManager.instance.GetMasterVolumeLevel();
        startScreenMusicVolumeLevel.value = SettingsManager.instance.GetMusicVolumeLevel();
        startScreenSFXVolumeLevel.value = SettingsManager.instance.GetSFXVolumeLevel();

        if (!SettingsManager.instance.GetIsDailyMapSelected())
        {
            useDailyMapButton.text = "No";
        }
        else if (SettingsManager.instance.GetIsDailyMapSelected())
        {
            useDailyMapButton.text = "Yes";
        }
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

        finalScoreNumber.text = "" + ScoreManager.instance.GetCurrentScore();
        highScoreNumber.text = "" + ScoreManager.instance.GetHighScore();

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
            AudioManager.instance.PlaySound("All Main Menu Music", gameObject.transform);
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
            AudioManager.instance.PlaySound("All Background Music", gameObject.transform);
        }
    }

    public void ToggleGamePaused()
    {
        List<PlayerController> playerControllers = GameManager.instance.players;
        List<AIController> aIControllers = GameManager.instance.aIPlayers;

        foreach (PlayerController playerController in playerControllers)
        {
            playerController.pawn.ToggleIsGamePaused();
        }

        foreach (AIController aIController in aIControllers)
        {
            aIController.pawn.ToggleIsGamePaused();
        }
    }

    public void ToggleOneOrTwoPlayersButtonText()
    {
        if (toggleOneOrTwoPlayersButton.text == "One")
        {
            toggleOneOrTwoPlayersButton.text = "Two";
        }
        else if (toggleOneOrTwoPlayersButton.text == "Two")
        {
            toggleOneOrTwoPlayersButton.text = "One";
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