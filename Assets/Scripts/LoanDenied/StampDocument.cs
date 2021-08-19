using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampDocument : MonoBehaviour
{
    public event Action OnDocumentStamped;
    public Transform stampPoint;
    public Vector2 documentArea, stampArea;

    private Vector3 startPos;
    private bool stamped = false;
    private bool animating = false;
    private AudioSource source;

    public void Initialise ()
    {
        startPos = transform.localPosition;
        source = GetComponent<AudioSource> ();
        stampPoint.localPosition = Tools.RandomPositionInArea (documentArea, stampArea);
    }

    public void Stamp (Vector3 stampAttemptPoint)
    {
        if (stamped || animating) return;

        print ((stampAttemptPoint - stampPoint.position).sqrMagnitude / 0.01f);
        if ((stampAttemptPoint - stampPoint.position).sqrMagnitude < 0.05f)
        {
            print ("got it");
            stamped = true;
            OnDocumentStamped?.Invoke ();
        }
    }

    public void FlyIn ()
    {
        Invoke ("PlaySound", 0.4f);
        StartCoroutine (AnimateInOut (Vector3.zero));
    }

    public void FlyOut ()
    {
        PlaySound ();
        StartCoroutine (AnimateInOut (-startPos));
    }

    private void PlaySound ()
    {
        AudioManager.Instance.Play ("Swipe", source, UnityEngine.Random.Range (0.8f, 1.2f), UnityEngine.Random.Range (0.8f, 1.2f));
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

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (stampPoint.parent.position, documentArea, stampPoint.parent.rotation.eulerAngles, Color.white);
        Tools.DrawBox (stampPoint.position, stampArea, stampPoint.localRotation.eulerAngles, Color.blue);
    }
}
