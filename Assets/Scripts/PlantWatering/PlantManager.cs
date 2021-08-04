using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public PlantController[] plants;
    public Transform view;
    public PlantController currentPlant { get { return currentPlantActive ? plants[currentIndex] : null; } }

    private int currentIndex;
    private bool currentPlantActive = true;
    private bool gameOver = false;

    private void Start ()
    {
        foreach (PlantController plant in plants)
            plant.OnPlantFilled += OnPlantFilled;

        StartCoroutine (FocusCurrentPlant ());
    }

    private void OnPlantFilled (bool correctAmount)
    {
        if (gameOver) return;

        if (correctAmount)
        {
            AudioManager.Instance.PlayAtLocation ("Sparkle", plants[currentIndex].transform, 0.75f, Random.Range (0.75f, 0.9f), Random.Range (0.95f, 1.05f));
            if (++currentIndex >= plants.Length)
            {
                currentPlantActive = false;
                EventManager.Instance.MicrogameEnd (MicrogameResult.Win);
                gameOver = true;
            }
            else
                StartCoroutine (FocusCurrentPlant ());
        }
        else
        {
            gameOver = true;
            EventManager.Instance.MicrogameEnd (MicrogameResult.Lose);
        }
    }

    private IEnumerator FocusCurrentPlant ()
    {
        Vector3 origin = view.position;
        Vector3 destination = currentPlant.transform.position;
        float time = 1f;

        currentPlantActive = false;

        for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
        {
            float ratio = elapsed / time;
            ratio = ratio * ratio * (3 - 2 * ratio);
            view.position = Vector3.Lerp (origin, destination, ratio);
            yield return null;
        }
    
        currentPlantActive = true;
        currentPlant.ShowGauge ();
    }

    private void OnDestroy ()
    {
        foreach (PlantController plant in plants)
            plant.OnPlantFilled -= OnPlantFilled;
    }
}