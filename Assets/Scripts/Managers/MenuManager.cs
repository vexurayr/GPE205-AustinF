using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject settingsMenu;

    public GameObject deathScreen;

    public Text highScoreText;

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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateSettingsMenu();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

            settingsMenu.SetActive(false);

            HideCursor();
        }
        // The settings menu is currently not visible
        else
        {
            isSettingsMenuActive = true;

            settingsMenu.SetActive(true);

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ShowDeathScreen()
    {
        ShowCursor();

        highScoreText.text = "Highscore: " + ScoreManager.instance.highScore;

        deathScreen.SetActive(true);
    }
}