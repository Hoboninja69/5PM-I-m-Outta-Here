using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilatingGauge : MonoBehaviour
{
    public float oscillationSpeed;
    [HideInInspector]
    public float value;

    [SerializeField]
    private Transform indicator;
    [SerializeField]
    private float indicatorMoveAmount;

    private void Update ()
    {
        value = Mathf.Sin (Time.time * oscillationSpeed);
        indicator.transform.localPosition = new Vector3 (0, value * indicatorMoveAmount, 0);
    }
}
