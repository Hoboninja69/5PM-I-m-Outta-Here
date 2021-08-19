using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackLeaner : MonoBehaviour
{
    public float maxLeanIncrement;

    private Transform[] files;
    bool fallen = false;

    private void Start ()
    {
        Transform[] children = GetComponentsInChildren<Transform> ();
        List<Transform> fileList = new List<Transform> ();

        foreach (Transform child in children)
        {
            if (child.parent == transform)
                fileList.Add (child);
        }
        files = fileList.ToArray ();
    }

    public void SetLean (float amount)
    {
        for (int i = 0; i < files.Length; i++)
        {
            files[i].localRotation = Quaternion.Euler (0, 0, maxLeanIncrement * amount * i);
        }
    }

    public void Fall ()
    {
        if (fallen) return;
        fallen = true;

        foreach (Transform file in files)
        {
            if (file.GetChild (0).TryGetComponent (out Rigidbody rb))
                rb.isKinematic = false;
        }
    }
}
