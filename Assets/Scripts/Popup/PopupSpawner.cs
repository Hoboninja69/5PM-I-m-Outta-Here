using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    public GameObject PopUp;
    public Vector2 screenSize;
    public float initialDelay, finalDelay, delayIncreaseRate;

    private int remaining;
    private float delay;

    private void Start ()
    {
        delay = initialDelay;
    }

    IEnumerator SpawnPopups ()
    {
        while (remaining <= 0)
        {
            remaining++;
            Instantiate (PopUp, RandomScreenPosition (), Quaternion.identity, transform).GetComponent<PopupController> ().OnClose += OnPopupClose;
            yield return new WaitForSeconds (delay);

            if (delay < finalDelay)
                delay += delayIncreaseRate * delay;
        }
    }

    private void OnPopupClose (PopupController popup)
    {
        popup.OnClose -= OnPopupClose;
        
    }

    private Vector3 RandomScreenPosition ()
    {
        Vector2 centerOffset = new Vector2 (Random.Range (-screenSize.x / 2, screenSize.x / 2), Random.Range (-screenSize.y / 2, screenSize.y / 2));
        return transform.position + transform.right * centerOffset.x + transform.up * centerOffset.y;
    }
}
