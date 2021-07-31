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
        boxes = GetComponentsInChildren<PackageControl>();

        foreach (PackageControl box in boxes)
        {
            box.OnBoxOpen += BoxOpen;
        }
    }

    private void BoxOpen (PackageControl box)
    {
        if (!bombFound && ++openCount >= minOpenCount && Random.Range(0, boxes.Length - openCount + 1) == 0)
        {
            box.SetBomb(true);
            bombFound = true;
            EventManager.Instance.MicrogameEnd(MicrogameResult.Win);
        }
    }
}
