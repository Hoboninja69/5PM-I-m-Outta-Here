using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] sounds;

    private Sound[] microgameSounds;
    private Sound currentSong;

    public void Initialise ()
    {
        if (Instance != null)
            Destroy (this);
        else
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnMicrogameLoad += GetMicrogameSounds;
            EventManager.Instance.OnMicrogameLoad += PlayMicrogameMusic;
            EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
        }

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
        sound.source.clip = sound.GetClip ();

        sound.source.Play ();
    }

    public void Play (string soundName, AudioSource source, float volumeMult = 1, float pitchMult = 1)
    {
        if (!FindSound (soundName, out Sound sound))
        {
            Debug.LogError ("Could not Play sound \"" + soundName + "\" (not found)");
            return;
        }

        if (source == null)
        {
            Debug.LogError ("Audiosource is null.");
            return;
        }

        sound.source = source;
        source.volume = sound.volume * volumeMult;
        source.pitch = sound.pitch * pitchMult;
        source.loop = sound.loop;
        source.clip = sound.GetClip ();

        source.Play ();
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
        source.clip = sound.GetClip ();
        source.volume = sound.volume * volumeMult;
        source.pitch = sound.pitch * pitchMult;
        source.spatialBlend = spatialBlend;
        source.loop = sound.loop;

        source.Play ();
        if (!source.loop && source.pitch != 0)
            Destroy (soundObject, source.clip.length * Mathf.Abs (source.pitch));

        return source;
    }

    public AudioSource PlayAtLocation (string soundName, Transform target, float spatialBlend, float volumeMult = 1, float pitchMult = 1)
    {
        if (!FindSound (soundName, out Sound sound))
        {
            Debug.LogError ("Could not Play sound \"" + soundName + "\" at location (not found)");
            return null;
        }

        AudioSource source = target.gameObject.AddComponent<AudioSource> ();
        source.clip = sound.GetClip ();
        source.volume = sound.volume * volumeMult;
        source.pitch = sound.pitch * pitchMult;
        source.spatialBlend = spatialBlend;
        source.loop = sound.loop;

        source.Play ();
        if (!source.loop && source.pitch != 0)
            Destroy (source, source.clip.length * Mathf.Abs (source.pitch));

        return source;
    }

    public void SetMult (string soundName, float volumeMult = 1, float pitchMult = 1)
    {
        if (!FindSound (soundName, out Sound sound))
        {
            Debug.LogError ("Could not set multipliers for sound \"" + soundName + "\" (not found)");
            return;
        }

        sound.source.volume = sound.volume * volumeMult;
        sound.source.pitch = sound.pitch * pitchMult;
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

        microgameSounds = microgame.Sounds;
        foreach (Sound sound in microgameSounds)
            AddSoundSource (sound);
    }

    private void PlayMicrogameMusic (Microgame microgame)
    {
        if (microgame.Music == null || microgame.Music.clips.Length == 0) return;

        currentSong = microgame.Music;

        if (currentSong.source != null)
            SetSoundSource (currentSong.source, currentSong);
        else
            AddSoundSource (currentSong);

        currentSong.source.Play ();
    }

    private void OnMicrogameEnd (MicrogameResult result)
    {
        if (currentSong != null)
            currentSong.source.Stop ();

        if (result == MicrogameResult.Win)
            Play ("Win");
        else
            Play ("Lose");
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
        sound.source.clip = sound.GetClip ();
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
    }
    
    private void SetSoundSource (AudioSource source, Sound sound)
    {
        print ("Setting sound source for sound \"" + sound.name + "\"");
        sound.source = source;
        source.clip = sound.GetClip ();
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.loop = sound.loop;
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip[] clips;
    [HideInInspector]
    public AudioSource source;

    public string name;
    public float volume = 1, pitch = 1;
    public bool loop;

    public AudioClip GetClip ()
    {
        if (clips.Length == 0)
        {
            Debug.LogError ("No clips specified for sound \"" + name + "\"");
            return null;
        }
        return clips[Random.Range (0, clips.Length)];
    }
}
