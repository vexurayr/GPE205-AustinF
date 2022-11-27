using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Audio is at times buggy, cutting in and out, refusing to play even when it's 2D,
    // practically inaudible no matter how much the volume is raised, and I can't figure out why
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
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.mute = sound.isAudioMuted;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.spatialBlend = sound.spacialBlend;
            sound.source.loop = sound.isLooping;
        }
    }

    public void PlaySound(string audioName, Transform soundTransform)
    {
        Sound sound = Array.Find(sounds, sound => sound.audioName == audioName);

        try
        {
            // Without updating the transform, every source in 3D space would be (0, 0, 0)
            sound.source.transform.position = soundTransform.position;
            sound.source.Play();
        }
        catch (Exception e)
        {
            Debug.LogError("Couldn't find audio with name (" + audioName + ")\n" + e);
        }
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
        catch (Exception e)
        {
            Debug.LogError("Couldn't find audio with name (" + audioName + ")\n" + e);
        }
    }

    public void StopSound(string audioName)
    {
        Sound sound = Array.Find(sounds, sound => sound.audioName == audioName);

        try
        {
            sound.source.Stop();
        }
        catch (Exception e)
        {
            Debug.LogError("Couldn't find audio with name (" + audioName + ")\n" + e);
        }
    }

    // To handle looping sounds
    public bool IsSoundAlreadyPlaying(string audioName)
    {
        Sound sound = Array.Find(sounds, sound => sound.audioName == audioName);

        try
        {
            return sound.source.isPlaying;
        }
        catch (Exception e)
        {
            Debug.LogError("Couldn't find audio with name (" + audioName + ")\n" + e);
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
        catch (Exception e)
        {
            Debug.LogError("Couldn't find audio with name (" + audioName + ")\n" + e);
        }
    }
}