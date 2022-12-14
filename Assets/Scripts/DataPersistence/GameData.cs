using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    /// <summary>
    /// Whatever needs to be saved, add it here, makes the most sense if the variables are named the same
    /// Then add IDataPersistence to the scipt that normally stores/uses the data
    /// Then add LoadData/SaveData methods to that script
    /// public void LoadData(GameData data)
    /// {}
    /// public void SaveData(GameData data)
    /// {}
    /// Use SerializableDictionary<string, bool> if a dictionary is needed
    /// </summary>
    
    // From Score Manager
    public int highScore;

    // From Settings Manager
    public bool isGameOnePlayer;
    public bool isDailyMapSelected;
    public float masterVolumeLevel;
    public float musicVolumeLevel;
    public float sFXVolumeLevel;
    public float masterSliderValue;
    public float musicSliderValue;
    public float sFXSliderValue;

    // From Difficulty Manager
    public int currentDifficulty;

    // The values in the constructor are the default values assigned on creating a new game with no previous data
    public GameData()
    {
        this.highScore = 0;
        this.isGameOnePlayer = true;
        this.isDailyMapSelected = false;
        this.masterVolumeLevel = 0f;
        this.musicVolumeLevel = 0f;
        this.sFXVolumeLevel = 0f;
        this.masterSliderValue = 1f;
        this.musicSliderValue = 1f;
        this.sFXSliderValue = 1f;
        this.currentDifficulty = 2;
    }
}