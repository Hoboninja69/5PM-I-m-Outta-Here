using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    public GameObject[] PopUps;
    public Vector2 screenSize;
    public float initialDelay, finalDelay, delayIncreaseRate;

    private int remaining;
    private float delay;
    private bool allClosed = false;

    private void Start ()
    {
        delay = initialDelay;
        StartCoroutine (SpawnPopups ());
    }

    IEnumerator SpawnPopups ()
    {
        while (!allClosed)
        {
            yield return new WaitForSeconds (delay);

            SpawnPopup ();

            if (delay < finalDelay)
                delay += delayIncreaseRate * delay;
        }
    }

    private void OnPopupClose (PopupController popup)
    {
        if (--remaining <= 0)
        {
            allClosed = true;
            StopAllCoroutines ();
            if (EventManager.Instance != null)
                EventManager.Instance.MicrogameEnd (MicrogameResult.Win);
        }
        popup.OnClose -= OnPopupClose;
    }

    private void SpawnPopup ()
    {
        remaining++;
        PopupController popup = Instantiate (PopUps[Random.Range (0, PopUps.Length)], transform).GetComponent<PopupController> ();
        popup.OnClose += OnPopupClose;
        popup.transform.localPosition = Tools.RandomPositionInArea (screenSize, popup.popupSize);
    }

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (transform.position, screenSize, transform.rotation.eulerAngles, Color.white);
    }
}
