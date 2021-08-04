using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public bool continuallyCall = false;
    public string tagCheck;
    public UnityEvent<TriggerEvent, Collider> Event;

    private void OnTriggerEnter (Collider other)
    {
        if (continuallyCall) return;
        Check (other);
    }

    private void OnTriggerStay (Collider other)
    {
        if (!continuallyCall) return;
        Check (other);
    }

    private void Check (Collider other)
    {
        if ((tagCheck == "" || other.CompareTag (tagCheck)))
            Event?.Invoke (this, other);
    }
}
