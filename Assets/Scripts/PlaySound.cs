using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public string soundName;
    public bool useAttachedSource;
    public Vector2 randomPitchRange, randomVolumeRange;

    private AudioSource source;

    private void Start ()
    {
        if (useAttachedSource)
            source = GetComponent<AudioSource> ();
    }

    public void Play ()
    {
        float volumeMult = Random.Range (randomVolumeRange.x, randomVolumeRange.y);
        float pitchMult = Random.Range (randomPitchRange.x, randomPitchRange.y);

        if (useAttachedSource)
            AudioManager.Instance.Play (soundName, source, volumeMult, pitchMult);
        else
            AudioManager.Instance.Play (soundName, volumeMult, pitchMult);
    }

    public void Stop ()
    {
        source.Stop ();
    }
}
