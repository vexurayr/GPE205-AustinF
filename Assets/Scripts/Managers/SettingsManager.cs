using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour, IDataPersistence
{
    public static SettingsManager instance { get; private set; }

    public bool isGameOnePlayer;
    public bool isDailyMapSelected;
    public float masterVolumeLevel;
    public float musicVolumeLevel;
    public float sFXVolumeLevel;

    public float masterSliderValue;
    public float musicSliderValue;
    public float sFXSliderValue;

    public AudioMixer mixer;

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

    public void ToggleIsDailyMapSelected()
    {
        isDailyMapSelected = !isDailyMapSelected;
    }

    public bool GetIsDailyMapSelected()
    {
        return isDailyMapSelected;
    }

    public void ToggleIsGameOnePlayer()
    {
        isGameOnePlayer = !isGameOnePlayer;
    }

    public bool GetIsGameOnePlayer()
    {
        return isGameOnePlayer;
    }

    // Allows these functions to work with both versions of the sliders
    public void SetMasterVolumeLevel(float sliderValue)
    {
        // Save reference to slider's value so a value from 0-2 can be returned to the slider
        masterSliderValue = sliderValue;

        // Slider is 0-2, this will get it to work with logarithmic decibels
        if (sliderValue <= 0)
        {
            // Set the volume to the lowest value
            sliderValue = -80;
        }
        else
        {
            // Find log10 value
            // Slider at .1 means db of -1, slider at 2 means db of .3
            sliderValue = Mathf.Log10(sliderValue);

            // Multiply db to get larger range, with a max of +6 db
            sliderValue = sliderValue * 20;
        }

        // Set the volume in the audio mixer group to the new volume
        mixer.SetFloat("MasterVolume", sliderValue);
        mixer.GetFloat("MasterVolume", out masterVolumeLevel);
    }

    public float GetMasterVolumeSliderValue()
    {
        return masterSliderValue;
    }

    public void SetMusicVolumeLevel(float sliderValue)
    {
        musicSliderValue = sliderValue;

        if (sliderValue <= 0)
        {
            sliderValue = -80;
        }
        else
        {
            sliderValue = Mathf.Log10(sliderValue);
            sliderValue = sliderValue * 20;
        }

        mixer.SetFloat("MusicVolume", sliderValue);
        mixer.GetFloat("MusicVolume", out musicVolumeLevel);
    }

    public float GetMusicVolumeSliderValue()
    {
        return musicSliderValue;
    }

    public void SetSFXVolumeLevel(float sliderValue)
    {
        sFXSliderValue = sliderValue;

        if (sliderValue <= 0)
        {
            sliderValue = -80;
        }
        else
        {
            sliderValue = Mathf.Log10(sliderValue);
            sliderValue = sliderValue * 20;
        }

        mixer.SetFloat("SFXVolume", sliderValue);
        mixer.GetFloat("SFXVolume", out sFXVolumeLevel);
    }

    public float GetSFXVolumeSliderValue()
    {
        return sFXSliderValue;
    }

    public void LoadData(GameData data)
    {
        this.isGameOnePlayer = data.isGameOnePlayer;
        this.isDailyMapSelected = data.isDailyMapSelected;
        this.masterVolumeLevel = data.masterVolumeLevel;
        this.musicVolumeLevel = data.musicVolumeLevel;
        this.sFXVolumeLevel = data.sFXVolumeLevel;
        this.masterSliderValue = data.masterSliderValue;
        this.musicSliderValue = data.musicSliderValue;
        this.sFXSliderValue = data.sFXSliderValue;
    }

    public void SaveData(GameData data)
    {
        data.isGameOnePlayer = this.isGameOnePlayer;
        data.isDailyMapSelected = this.isDailyMapSelected;
        data.masterVolumeLevel = this.masterVolumeLevel;
        data.musicVolumeLevel = this.musicVolumeLevel;
        data.sFXVolumeLevel = this.sFXVolumeLevel;
        data.masterSliderValue = this.masterSliderValue;
        data.musicSliderValue = this.musicSliderValue;
        data.sFXSliderValue = this.sFXSliderValue;
    }
}