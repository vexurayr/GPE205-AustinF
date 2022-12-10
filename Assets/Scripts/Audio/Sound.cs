using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    // The audio file this script will play
    public AudioClip clip;

    public string audioName;

    // Audio controls
    public bool isAudioMuted;
    [Range(0f, 100f)] public float volume;
    [Range(.1f, 4f)] public float pitch;

    // 0 = 2D, 1 = 3D
    [Range(0, 1)] public int spacialBlend;

    public bool isLooping;

    public AudioMixerGroup mixerGroup;

    [HideInInspector] public AudioSource source;
}