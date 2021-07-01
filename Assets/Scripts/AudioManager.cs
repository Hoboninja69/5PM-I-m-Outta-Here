using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] sounds;
    private Sound[] microgameSounds;

    public void Initialise ()
    {
        if (Instance != null)
            Destroy (this);
        else
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        EventManager.Instance.OnMicrogameLoad += GetMicrogameSounds;

        foreach (Sound sound in sounds)
            AddSoundSource (sound);
    }

    public void Play (string soundName, float volumeMult = 1, float pitchMult = 1)
    {
        if (!FindSound (soundName, out Sound sound))
        {
            Debug.LogError ("Could not Play sound \"" + soundName + "\" (not found)");
            return;
        }

        if (sound.source == null)
            AddSoundSource (sound);

        sound.source.volume = sound.volume * volumeMult;
        sound.source.pitch = sound.pitch * pitchMult;

        sound.source.Play ();
    }

    public AudioSource PlayAtLocation (string soundName, Vector3 location, float spatialBlend, float volumeMult = 1, float pitchMult = 1)
    {
        if (!FindSound (soundName, out Sound sound))
        {
            Debug.LogError ("Could not Play sound \"" + soundName + "\" at location (not found)");
            return null;
        }

        GameObject soundObject = new GameObject ();
        soundObject.transform.position = location;
        AudioSource source = soundObject.AddComponent<AudioSource> ();
        source.clip = sound.clip;
        source.volume = sound.volume * volumeMult;
        source.pitch = sound.pitch * pitchMult;
        source.spatialBlend = spatialBlend;
        source.loop = sound.loop;

        source.Play ();
        if (!source.loop && source.pitch != 0)
            Destroy (soundObject, sound.clip.length * Mathf.Abs (source.pitch));

        return source;
    }

    public void Stop (string soundName)
    {
        if (!FindSound (soundName, out Sound sound))
        {
            Debug.LogError ("Could not Stop sound \"" + soundName + "\" (not found)");
            return;
        }

        sound.source.Stop ();
    }

    private void GetMicrogameSounds (Microgame microgame)
    {
        if (microgameSounds != null)
        {
            foreach (Sound sound in microgameSounds)
                Destroy (sound.source);
        }    

        microgameSounds = microgame.MicrogameSounds;
        foreach (Sound sound in sounds)
            AddSoundSource (sound);
    }

    private bool FindSound (string soundName, out Sound foundSound)
    {
        Sound sound = System.Array.Find (sounds, s => s.name == soundName);
        if (sound == null && microgameSounds != null)
            sound = System.Array.Find (microgameSounds, s => s.name == soundName);

        foundSound = sound;
        return sound != null;
    }

    private void AddSoundSource (Sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource> ();
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;

    public string name;
    public float volume = 1, pitch = 1;
    public bool loop;
}
