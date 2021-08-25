using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager: MonoBehaviour
{
    public int minOpenCount;

    private int openCount;
    private PackageControl[] boxes;
    private bool bombFound = false;
   
    void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
            EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
        }
        else AudioManager.Instance.Play ("BombTick");

        boxes = GetComponentsInChildren<PackageControl>();

        foreach (PackageControl box in boxes)
        {
            box.OnBoxOpen += BoxOpen;
        }
    }

    private void BoxOpen (PackageControl box)
    {
        box.OnBoxOpen -= BoxOpen;

        if (!bombFound && ++openCount >= minOpenCount && Random.Range(0, boxes.Length - openCount + 1) == 0)
        {
            AudioManager.Instance.Stop ("BombTick");
            box.SetBomb(true);
            bombFound = true;
            EventManager.Instance.MicrogameEnd(MicrogameResult.Win, 1.5f);
        }
    }

    private void OnMicrogameStart (Microgame microgame) => AudioManager.Instance.Play ("BombTick");
    private void OnMicrogameEnd (MicrogameResult result) 
    {
        AudioManager.Instance.Stop ("BombTick");
        if (result == MicrogameResult.OutOfTime) AudioManager.Instance.Play ("Explosion"); 
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnMicrogameStart -= OnMicrogameStart;
        EventManager.Instance.OnMicrogameEnd -= OnMicrogameEnd;
    }
}
