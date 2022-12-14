using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour, IDataPersistence
{
    public static DifficultyManager instance { get; private set; }

    // Multipliers for selectable difficulties
    [Range(0.1f, 2f)] public float easyMultiplier;
    [Range(0.1f, 2f)] public float normalMultiplier;
    [Range(0.1f, 2f)] public float hardMultiplier;
    [SerializeField] private int currentDifficulty;

    // Values that will be used when each AI tank is instantiated
    [SerializeField] private float currentAIMaxHealth;
    [SerializeField] private float currentAIShotsPerSecond;
    [SerializeField] private int currentPointsGiven;

    // Waves track the number of times new AI spawn, every 2 waves is a bump in AI stats
    [Range(0.1f, 10f)] public float waveAIMaxHealthInc;
    [Range(0.1f, 1f)] public float waveAIShotsPerSecondInc;
    [Range(1, 5)] public int wavePointInc;
    [SerializeField] private float waveAIMaxHealth;
    [SerializeField] private float waveAIShotsPerSecond;
    [SerializeField] private int wavePoint;
    [SerializeField] private int wave;

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

    // Start is called before the first frame update
    void Start()
    {
        waveAIMaxHealth = 0f;
        waveAIShotsPerSecond = 0f;
        wavePoint = 0;
        wave = 0;
        currentAIMaxHealth = 0f;
        currentAIShotsPerSecond = 0f;
        currentPointsGiven = 0;
    }

    public void SetAIToEasy()
    {
        currentDifficulty = 1;
    }

    public void SetAIToNormal()
    {
        currentDifficulty = 2;
    }

    public void SetAIToHard()
    {
        currentDifficulty = 3;
    }

    public void CheckForDifficultyBump()
    {
        wave++;

        // Wave is an even number: 2, 4, 6, 8
        if (wave % 2 == 0 && wave != 0)
        {
            IncreaseDifficulty();
        }
    }

    public void IncreaseDifficulty()
    {
        waveAIMaxHealth += waveAIMaxHealthInc;
        waveAIShotsPerSecond += waveAIShotsPerSecondInc;
        wavePoint += wavePointInc;
    }

    public void GenerateAITankStats(float maxHealth, float shotsPerSecond, int pointsGiven)
    {
        currentAIMaxHealth = maxHealth;
        currentAIShotsPerSecond = shotsPerSecond;
        currentPointsGiven = pointsGiven;

        switch (currentDifficulty)
        {
            case 1: // Easy
                currentAIMaxHealth += waveAIMaxHealth * easyMultiplier;
                currentAIShotsPerSecond += waveAIShotsPerSecond * easyMultiplier;
                currentPointsGiven += (int)(wavePoint * easyMultiplier);
                break;
            case 2: // Normal
                currentAIMaxHealth += waveAIMaxHealth * normalMultiplier;
                currentAIShotsPerSecond += waveAIShotsPerSecond * normalMultiplier;
                currentPointsGiven += (int)(wavePoint * normalMultiplier);
                break;
            case 3: // Hard
                currentAIMaxHealth += waveAIMaxHealth * hardMultiplier;
                currentAIShotsPerSecond += waveAIShotsPerSecond * hardMultiplier;
                currentPointsGiven += (int)(wavePoint * hardMultiplier);
                break;
            default:
                Debug.Log("The Difficulty Manager can not determine which difficulty is selected.");
                break;
        }
    }

    public float GetNewMaxHealth()
    {
        return currentAIMaxHealth;
    }

    public float GetNewShotsPerSecond()
    {
        return currentAIShotsPerSecond;
    }

    public int GetNewPointsGiven()
    {
        return currentPointsGiven;
    }

    public void ResetVariables()
    {
        waveAIMaxHealth = 0f;
        waveAIShotsPerSecond = 0f;
        wavePoint = 0;
        wave = 0;
        currentAIMaxHealth = 0f;
        currentAIShotsPerSecond = 0f;
        currentPointsGiven = 0;
    }

    public void LoadData(GameData data)
    {
        this.currentDifficulty = data.currentDifficulty;
    }

    public void SaveData(GameData data)
    {
        data.currentDifficulty = this.currentDifficulty;
    }
}