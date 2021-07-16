using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager: MonoBehaviour
{

    public PackageControl[] boxes;
   
    void Start()
    {
        int selectedBox = Random.Range(0, boxes.Length);
        boxes[selectedBox].hasBomb = true;

        foreach (PackageControl box in boxes)
        {
            box.Initialise();
        }
    }


}
