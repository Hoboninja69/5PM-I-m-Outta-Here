using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtWiper : MonoBehaviour
{
    public float affectRadius;

    private Vector3 lastPos;

    private void Update ()
    {
        Collider[] dirtPatches = Physics.OverlapSphere (transform.position, affectRadius);
        if (dirtPatches.Length > 0)
        {
            float distance = Vector3.Distance (lastPos, transform.position);
            if (distance > 0)
            {
                foreach (Collider dirt in dirtPatches)
                {
                    if (dirt.TryGetComponent (out DirtDissolve dissolve))
                        dissolve.Wipe (distance);
                }
            }
        }

        lastPos = transform.position;
    }

    private void OnDrawGizmos ()
    {
        Gizmos.DrawWireSphere (transform.position, affectRadius);
    }
}
