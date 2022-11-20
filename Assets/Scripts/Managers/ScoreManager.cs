using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
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
        foreach (PlayerController controller in players)
        {
            int score = controller.pawn.GetScore();

            if (highScore < score)
            {
                highScore = score;
            }
        }
    }
}