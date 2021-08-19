using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSpawner : MonoBehaviour
{
    public int killAmountTarget;
    public GameObject[] Viruses;
    public Vector2 screenSize;
    public Vector2 delayRange;

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

    private void OnPopupClose (VirusController popup)
    {
        popup.OnClose -= OnPopupClose;
        if (--remaining <= 0)
        {
            if (EventManager.Instance != null)
                EventManager.Instance.MicrogameEnd (MicrogameResult.Win, 0.5f);
            return;
        }
        StartCoroutine (SpawnPopup ());
        print (remaining);
    }

    private IEnumerator SpawnPopup ()
    {
        yield return new WaitForSeconds (Random.Range (delayRange.x, delayRange.y));
        VirusController popup = Instantiate (Viruses[Random.Range (0, Viruses.Length)], transform).GetComponent<VirusController> ();
        popup.OnClose += OnPopupClose;
        popup.transform.localPosition = Tools.RandomPositionInArea (screenSize, popup.popupSize);
    }

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (transform.position, screenSize, transform.rotation.eulerAngles, Color.white);
    }
}
