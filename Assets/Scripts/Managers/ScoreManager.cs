using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IDataPersistence
{
    public static ScoreManager instance;

    public List<PlayerController> players;

    public int highScore;

    private int currentScore;

    private void Awake()
    {
        players = new List<PlayerController>();

        // Only allows for one score manager, one singleton
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
        if (highScore < currentScore)
        {
            highScore = currentScore;
        }
    }

    public void UpdateCurrentScore()
    {
        foreach (PlayerController controller in players)
        {
            currentScore = controller.pawn.GetScore();
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;
    }

    public void LoadData(GameData data)
    {
        this.highScore = data.highScore;
    }

    public void SaveData(GameData data)
    {
        data.highScore = this.highScore;
    }
}