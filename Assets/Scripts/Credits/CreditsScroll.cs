using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public float startHeight, endHeight;
    public float scrollSpeed;

    private float height;
    private bool finished;

    private void Start ()
    {
        height = startHeight;
    }

    void Update()
    {
        if (finished) return;

        height += scrollSpeed * Time.deltaTime;
        if (height > Mathf.Max (startHeight, endHeight) || height < Mathf.Min (startHeight, endHeight))
            finished = true;

        transform.localPosition = new Vector3 (transform.localPosition.x, height, transform.localPosition.z);
    }
}
