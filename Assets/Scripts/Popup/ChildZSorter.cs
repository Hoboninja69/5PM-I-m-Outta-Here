using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildZSorter : MonoBehaviour
{
    public float zSpacing;

    private Transform[] lastChildren;

    void Update()
    {
        Transform[] children = GetComponentsInChildren<Transform> ();
        if (children == lastChildren) return;

        for (int i = 1; i < children.Length; i++)
        {
            Vector3 childPos = children[i].localPosition;
            childPos.z = (i - 1) * zSpacing;
            children[i].localPosition = childPos;
        }
    }
}
