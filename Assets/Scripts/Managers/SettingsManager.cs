using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour, IDataPersistence
{
    public static SettingsManager instance { get; private set; }

    private bool isDailyMapSelected;
    private float masterVolumeLevel;
    private float musicVolumeLevel;
    private float sFXVolumeLevel;

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

    private void Update()
    {
        Debug.Log(masterVolumeLevel);
    }

    public void ToggleIsDailyMapSelected()
    {
        isDailyMapSelected = !isDailyMapSelected;
    }

    public bool GetIsDailyMapSelected()
    {
        return isDailyMapSelected;
    }

    public void SetMasterVolumeLevel()
    {
        Debug.Log("Level: " + MenuManager.instance.startScreenMasterVolumeLevel.value);
        this.masterVolumeLevel = MenuManager.instance.startScreenMasterVolumeLevel.value;
    }

    public float GetMasterVolumeLevel()
    {
        return this.masterVolumeLevel;
    }

    public void SetMusicVolumeLevel(float newLevel)
    {
        musicVolumeLevel = newLevel;
    }

    public float GetMusicVolumeLevel()
    {
        return musicVolumeLevel;
    }

    public void SetSFXVolumeLevel(float newLevel)
    {
        sFXVolumeLevel = newLevel;
    }

    public float GetSFXVolumeLevel()
    {
        return sFXVolumeLevel;
    }

    public void LoadData(GameData data)
    {
        this.isDailyMapSelected = data.isDailyMapSelected;
        this.masterVolumeLevel = data.masterVolumeLevel;
        this.musicVolumeLevel = data.musicVolumeLevel;
        this.sFXVolumeLevel = data.sFXVolumeLevel;
    }

    public void SaveData(GameData data)
    {
        data.isDailyMapSelected = this.isDailyMapSelected;
        data.masterVolumeLevel = this.masterVolumeLevel;
        data.musicVolumeLevel = this.musicVolumeLevel;
        data.sFXVolumeLevel = this.sFXVolumeLevel;
    }
}