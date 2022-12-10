using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    [Range(1, 3)] public int lives;

    private int startingLives;

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

        startingLives = lives;
    }

    public void DeincrementLives()
    {
        lives--;
    }

    public void RespawnPlayer()
    {
        DeincrementLives();

        if (GameManager.instance == null)
        {
            return;
        }

        if (lives > 0)
        {
            // Calls coroutine in game manager to spawn in a new player
            GameManager.instance.RespawnPlayer();
        }
        else
        {
            MenuManager.instance.ShowDeathScreen();
        }
    }

    public void ResetLives()
    {
        lives = startingLives;
    }

    public void SetStartingLivesTo1()
    {
        startingLives = 1;
        lives = startingLives;
    }

    public void SetStartingLivesTo3()
    {
        startingLives = 3;
        lives = startingLives;
    }
}