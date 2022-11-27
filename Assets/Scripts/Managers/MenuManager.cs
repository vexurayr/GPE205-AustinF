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
    public GameObject deathScreen;
    public Text highScoreText;

    // For start screen menus
    public GameObject startScreenBackground;
    public GameObject redTankImage;
    public GameObject gameNameText;
    public GameObject blueTankImage;
    public GameObject mainMenu;

    private bool isSettingsMenuActive = false;

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
            UpdateSettingsMenu();
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

    public void UpdateSettingsMenu()
    {
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

        highScoreText.text = "Highscore: " + ScoreManager.instance.highScore;

        deathScreen.SetActive(true);
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
}