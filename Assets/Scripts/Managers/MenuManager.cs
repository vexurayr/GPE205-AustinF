using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsMenu;

    private bool isSettingsMenuActive = false;

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

            DisablePauseMenu();
        }
        // The settings menu is currently not visible
        else
        {
            isSettingsMenuActive = true;

            settingsMenu.SetActive(true);

            EnablePauseMenu();
        }
    }

    public void EnablePauseMenu()
    {
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;

        //Time.timeScale = 0;
    }

    public void DisablePauseMenu()
    {
        // Locks the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Makes the cursor invisible
        Cursor.visible = false;

        Time.timeScale = 1;
    }
}