using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // An array of Sound objects that will be managed by this script.
    public Sound[] sounds;

    void Awake()
    {
        // For each Sound object, create an AudioSource and set its properties.
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        // Load audio volume from PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat("AudioVolume", 1f);

        // Set volume for each sound
        foreach (Sound s in sounds)
        {
            s.source.volume = savedVolume;
        }
    }

    // Play the sound with the given name.
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    // Stop the sound with the given name.
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    // Set the volume for the sound with the given name.
    public void SetVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = volume;
    }
}