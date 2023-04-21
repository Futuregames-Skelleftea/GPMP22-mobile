using UnityEngine.Audio;
using UnityEngine;

// This class represents a sound that can be played in the game
[System.Serializable]
public class Sound
{
    // The name of the sound
    public string name;

    // The audio clip associated with the sound
    public AudioClip clip;

    // The volume of the sound, with a range of 0 to 1
    [Range(0f, 1f)]
    public float volume;

    // The pitch of the sound, with a range of 0.1 to 3
    [Range(.1f, 3f)]
    public float pitch;

    // Whether or not the sound should loop
    public bool loop;

    // The audio source used to play the sound
    [HideInInspector]
    public AudioSource source;

    // The type of sound, for grouping and routing to different audio mixers
    public Type type;

    // Enum defining different types of sounds
    public enum Type
    {
        Master, Music, Soundeffect, Ambient, Narration, NotAssigned, audioMasterMixer
    }
}