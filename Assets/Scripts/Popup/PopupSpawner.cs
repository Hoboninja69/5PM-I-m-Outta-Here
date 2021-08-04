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
            SpawnPopup ();

            yield return new WaitForSeconds (delay);

            if (delay < finalDelay)
                delay += delayIncreaseRate * delay;
        }
    }

    private void OnPopupClose (PopupController popup)
    {
        if (--remaining <= 0)
            allClosed = true;
        popup.OnClose -= OnPopupClose;
        print (remaining);
    }

    private void SpawnPopup ()
    {
        remaining++;
        PopupController popup = Instantiate (PopUps[Random.Range (0, PopUps.Length)], transform).GetComponent<PopupController> ();
        popup.OnClose += OnPopupClose;
        popup.transform.localPosition = RandomScreenPosition (popup.popupSize);
    }

    private Vector2 RandomScreenPosition (Vector2 popupSize)
    {
        Vector2 maxOffset = new Vector2 (screenSize.x - popupSize.x, screenSize.y - popupSize.y) / 2;
        return new Vector2 (Random.Range (-maxOffset.x, maxOffset.x), Random.Range (-maxOffset.y, maxOffset.y));
    }

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (transform.position, screenSize, transform.rotation.eulerAngles, Color.white);
    }
}
