using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class ImpactSound : MonoBehaviour
{
    public string soundName;
    public bool randomisePitch, useAttachedSource;
    public float minimumVolume, spatialBlend, startDelay;
    public Vector2 velocityRange, randomPitchRange;

    private Rigidbody rb;
    private AudioSource source;
    private Vector3 lastVelocity;
    private float startTime;

    private void Start ()
    {
        rb = GetComponent<Rigidbody> ();
        if (useAttachedSource)
        {
            source = GetComponent<AudioSource> ();
            source.spatialBlend = spatialBlend;
        }
        startTime = Time.time;
    }

    private void FixedUpdate ()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (Time.time - startTime <= startDelay) return;

        float volumeMult = Mathf.InverseLerp (velocityRange.x, velocityRange.y, lastVelocity.magnitude - rb.velocity.magnitude);
        volumeMult = Mathf.Clamp (volumeMult, minimumVolume, volumeMult);
        float pitchMult = randomisePitch ? Random.Range (randomPitchRange.x, randomPitchRange.y) : 1;

        if (useAttachedSource)
            AudioManager.Instance.Play (soundName, source, volumeMult, pitchMult);
        else
            AudioManager.Instance.PlayAtLocation (soundName, transform.position, spatialBlend, volumeMult, pitchMult);

    }
}
