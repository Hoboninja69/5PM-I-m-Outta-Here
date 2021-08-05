using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSpawner : MonoBehaviour
{
    public int killAmountTarget;
    public GameObject[] Viruses;
    public Vector2 screenSize;
    public float delay;

    private int remaining;

    private void Start ()
    {
        remaining = killAmountTarget;
        StartCoroutine (SpawnPopup ());
    }

    //IEnumerator SpawnPopups ()
    //{
    //    while (!allClosed)
    //    {
    //        SpawnPopup ();

    //        yield return new WaitForSeconds (delay);

    //        if (delay < finalDelay)
    //            delay += delayIncreaseRate * delay;
    //    }
    //}

    private void OnPopupClose (PopupController popup)
    {
        popup.OnClose -= OnPopupClose;
        if (--remaining <= 0)
        {
            if (EventManager.Instance != null)
                EventManager.Instance.MicrogameEnd (MicrogameResult.Win);
            return;
        }
        StartCoroutine (SpawnPopup ());
        print (remaining);
    }

    private IEnumerator SpawnPopup ()
    {
        yield return new WaitForSeconds (delay);
        PopupController popup = Instantiate (Viruses[Random.Range (0, Viruses.Length)], transform).GetComponent<PopupController> ();
        popup.OnClose += OnPopupClose;
        popup.transform.localPosition = Tools.RandomPositionInArea (screenSize, popup.popupSize);
    }

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (transform.position, screenSize, transform.rotation.eulerAngles, Color.white);
    }
}
