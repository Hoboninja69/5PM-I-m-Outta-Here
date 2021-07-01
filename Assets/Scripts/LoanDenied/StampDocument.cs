using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampDocument : MonoBehaviour
{
    public event Action OnDocumentStamped;
    public Transform stampPoint;

    private Vector3 startPos;
    private bool stamped = false;
    private bool animating = false;

    private void Start ()
    {
        startPos = transform.localPosition;
    }

    public void Stamp (Vector3 stampAttemptPoint)
    {
        if (stamped || animating) return;

        print ((stampAttemptPoint - stampPoint.position).sqrMagnitude / 0.01f);
        if ((stampAttemptPoint - stampPoint.position).sqrMagnitude < 0.01f)
        {
            print ("got it");
            stamped = true;
            OnDocumentStamped?.Invoke ();
        }
    }

    public void FlyIn ()
    {
        StartCoroutine (AnimateInOut (Vector3.zero));
    }

    public void FlyOut ()
    {
        print (-startPos);
        StartCoroutine (AnimateInOut (-startPos));
    }

    IEnumerator AnimateInOut (Vector3 destination)
    {
        animating = true;

        Vector3 origin = transform.localPosition;
        float time = 1;
        for (float elapsed = 0f; elapsed < time; elapsed += Time.deltaTime)
        {
            float ratio = elapsed / time;
            ratio = ratio * ratio * (3f - 2f * ratio);

            transform.localPosition = Vector3.Lerp (origin, destination, ratio);
            yield return null;
        }

        transform.localPosition = destination;
        animating = false;
    }
}
