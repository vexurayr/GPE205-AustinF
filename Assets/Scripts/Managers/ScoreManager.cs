using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IDataPersistence
{
    public static ScoreManager instance;

    public List<PlayerController> players;

    public int highScore;

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
        if (players != null)
        {
            foreach (PlayerController controller in players)
            {
                if (controller != null && controller.pawn != null)
                {
                    int score = controller.pawn.GetScore();

                    if (highScore < score)
                    {
                        highScore = score;
                    }
                }
            }
        }
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