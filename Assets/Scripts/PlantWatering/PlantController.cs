using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public Mesh[] plants;
    public WaterGauge gauge;
    public Vector2 minCapacityRange, overfillMarginRange;
    public event System.Action<bool> OnPlantFilled;

    private float maxCapacity;
    private float minCapacity; 
    private float currentCapacity = 0;
    private bool full = false;

    private void Start ()
    {
        GetComponent<MeshFilter> ().mesh = plants[Random.Range (0, plants.Length)];
        minCapacity = Random.Range (minCapacityRange.x, minCapacityRange.y);
        maxCapacity = minCapacity + Random.Range (overfillMarginRange.x, overfillMarginRange.y);

        gauge.gameObject.SetActive (false);
        gauge.Initialise ();
        gauge.SetTargetBand (minCapacity, maxCapacity - minCapacity);
        gauge.SetGauge (currentCapacity);
    }

    public void ShowGauge ()
    {
        if (!gauge.gameObject.activeSelf)
            gauge.gameObject.SetActive (true);
    }

    public void Fill (float amount)
    {
        currentCapacity += amount * Time.deltaTime;
        gauge.SetGauge (currentCapacity);
        //if (!full && currentCapacity > maxCapacity)
        //{
        //    //OnPlantFilled?.Invoke (false);
        //    full = true;
        //}
    }

    public void CheckFull ()
    {
        //if (!full && currentCapacity >= minCapacity)
        //{
        //    OnPlantFilled?.Invoke (true);
        //    full = true;
        //}
        if (currentCapacity >= minCapacity)
            OnPlantFilled?.Invoke (currentCapacity <= maxCapacity);
    }
}
