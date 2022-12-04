using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    [Range(1, 3)] public int lives;

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

    public void DeincrementLives()
    {
        lives--;
    }

    public void RespawnPlayer()
    {
        DeincrementLives();

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
}