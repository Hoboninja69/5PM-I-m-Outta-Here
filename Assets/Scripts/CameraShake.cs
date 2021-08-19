using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Range (0, 0.3f)]
    public float delay;
    public float smoothSpeed, magnitude;

    private Vector3 targetPos;
    private float lastTime;

    private void Start ()
    {
        targetPos = transform.localPosition;
    }

    private void Update ()
    {
        if (Time.time - lastTime > delay)
        {
            targetPos = new Vector3 (Random.Range (-magnitude, magnitude), Random.Range (-magnitude, magnitude), Random.Range (-magnitude, magnitude));
            lastTime = Time.time;
        }

        transform.localPosition = Vector3.Lerp (transform.localPosition, targetPos, Time.deltaTime * smoothSpeed);
    }
}
