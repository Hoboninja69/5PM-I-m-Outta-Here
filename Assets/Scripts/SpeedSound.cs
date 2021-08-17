using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSound : MonoBehaviour
{
    public AudioSource source;
    public string soundName;
    public bool playOnAwake;
    public Vector2 volumeRange, pitchRange, speedRange;
    public bool debugSpeed;

    private float currentSpeed;
    private Vector3 lastPos;

    private void Start ()
    {
        if (!playOnAwake) return;

        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
            EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
        }
        else
            Play ();
    }

    private void OnMicrogameStart (Microgame microgame)
    {
        Play ();
    }

    private void OnMicrogameEnd (MicrogameResult result)
    {
        Stop ();
    }

    public void Play ()
    {
        AudioManager.Instance.Play (soundName, source);
    }

    public void Stop ()
    {
        AudioManager.Instance.Stop (soundName);
    }

    private void FixedUpdate ()
    {
        currentSpeed = Vector3.Distance (transform.position, lastPos) / Time.deltaTime;
        float ratio = Mathf.InverseLerp (speedRange.x, speedRange.y, currentSpeed);

        AudioManager.Instance.SetMult (soundName, Mathf.Lerp (volumeRange.x, volumeRange.y, ratio), Mathf.Lerp (pitchRange.x, pitchRange.y, ratio));
        lastPos = transform.position;

        if (debugSpeed) print (currentSpeed);
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnMicrogameStart -= OnMicrogameStart;
        EventManager.Instance.OnMicrogameEnd -= OnMicrogameEnd;
    }
}
