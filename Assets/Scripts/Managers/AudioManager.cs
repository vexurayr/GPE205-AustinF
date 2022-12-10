using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Audio is at times buggy
    public Sound[] sounds;

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

        // Initialize each sound and its settings
        foreach (Sound sound in sounds)
        {
            GameObject audioSource = new GameObject();
            audioSource.name = sound.audioName;
            audioSource.transform.parent = transform;

            sound.source = audioSource.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.mute = sound.isAudioMuted;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.spatialBlend = sound.spacialBlend;
            sound.source.loop = sound.isLooping;
            sound.source.outputAudioMixerGroup = sound.mixerGroup;
        }
    }

    public void PlaySound(string audioName, Transform soundTransform)
    {
        Sound sound = Array.Find(sounds, sound => sound.audioName == audioName);
        
        try
        {
            if (sound.source.loop)
            {
                PlayLoopingSound(audioName, soundTransform);
            }
            else
            {
                // Without updating the transform, every source in 3D space would be (0, 0, 0)
                AudioSource.PlayClipAtPoint(sound.clip, soundTransform.position);
            }
        }
        catch (Exception)
        {}
    }

    public void PlayLoopingSound(string audioName, Transform soundTransform)
    {
        Sound sound = Array.Find(sounds, sound => sound.audioName == audioName);

        try
        {
            sound.source.transform.position = soundTransform.position;

            if (!IsSoundAlreadyPlaying(audioName))
            {
                sound.source.Play();
            }
        }
        catch (Exception)
        {}
    }

    public void StopSound(string audioName)
    {
        Sound sound = Array.Find(sounds, sound => sound.audioName == audioName);

        try
        {
            sound.source.Stop();
        }
        catch (Exception)
        {}
    }

    // To handle looping sounds
    public bool IsSoundAlreadyPlaying(string audioName)
    {
        Sound sound = Array.Find(sounds, sound => sound.audioName == audioName);

        try
        {
            return sound.source.isPlaying;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void StopSoundIfItsPlaying(string audioName)
    {
        Sound sound = Array.Find(sounds, sound => sound.audioName == audioName);

        try
        {
            if (IsSoundAlreadyPlaying(audioName))
            {
                sound.source.Stop();
            }
        }
        catch (Exception)
        {}
    }
}