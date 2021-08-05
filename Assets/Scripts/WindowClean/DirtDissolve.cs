using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtDissolve : MonoBehaviour
{
    public event System.Action<DirtDissolve> OnCleaned;
    public Vector2 requiredDistanceRange, sizeRange;
    [HideInInspector]
    public float size;

    private new MeshRenderer renderer;
    private float requiredDistance, totalDistance;
    private bool cleaned = false;

    private void Start ()
    {
        renderer = GetComponent<MeshRenderer> ();
        requiredDistance = Random.Range (requiredDistanceRange.x, requiredDistanceRange.y);
        size = Random.Range (sizeRange.x, sizeRange.y);
        transform.localScale = Vector3.one * size;
    }

    public void Wipe (float distance)
    {
        if (cleaned) return;

        totalDistance += distance;
        renderer.material.color = Transparent (Mathf.Clamp01(1 - (totalDistance / requiredDistance)));

        if (totalDistance >= requiredDistance)
        {
            OnCleaned?.Invoke (this);
            cleaned = true;
        }
    }

    private Color Transparent (float alpha) => new Color (1, 1, 1, alpha);
}
