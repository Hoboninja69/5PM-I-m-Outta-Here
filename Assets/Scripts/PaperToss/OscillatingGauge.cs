using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingGauge : MonoBehaviour
{
    public Transform indicator;
    [HideInInspector]
    public float value;
    public float indicatorMoveAmount;
    public float oscillationSpeed;
    public float disappearDelay;

    [HideInInspector]
    public bool active { get; private set; } = true;

    public void SetActive (bool active)
    {
        if (this.active == active) return;

        this.active = active;
        if (!active)
            Invoke ("Hide", disappearDelay);
        else
            gameObject.SetActive (true);
    }

    private void Update ()
    {
        if (!active) return;
        
        value = Mathf.Sin (Time.time * oscillationSpeed);
        indicator.localPosition = Vector3.up * value * indicatorMoveAmount;
    }

    private void Hide ()
    {
        gameObject.SetActive (false);
    }
}