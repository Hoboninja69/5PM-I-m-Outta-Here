using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGauge : MonoBehaviour
{
    public Transform fillBottom, fillBody, targetBand;
    public Vector2 bottomMinSize;
    public float maxBodySize;

    private float bottomPortion, totalHeight, targetBandBottom, targetBandTop;

    private void Start ()
    {
        bottomPortion = 0.5f / (maxBodySize + 0.5f);
        totalHeight = 0.5f + maxBodySize;
        targetBandBottom = fillBottom.localPosition.y;
        targetBandTop = targetBandBottom + totalHeight / 10;
    }

    //private void Update ()
    //{
    //    SetTargetBand (InputManager.cursorPosition.y / Screen.height, 0.1f);
    //    SetGauge (InputManager.cursorPosition.y / Screen.height);
    //}

    public void SetGauge (float amount)
    {
        float bottomRatio = Mathf.Clamp01 (amount / bottomPortion);
        float bodyRatio = (amount - bottomPortion) / (1 - bottomPortion);

        fillBottom.localScale = Vector3.Lerp (bottomMinSize, Vector3.one, bottomRatio);
        fillBody.localScale = new Vector3 (1, Mathf.Lerp (0, maxBodySize, bodyRatio), 1);
    }

    public void SetTargetBand (float min, float margin)
    {
        print (min + margin);
        targetBand.localPosition = new Vector3 (targetBand.localPosition.x, Mathf.Lerp (targetBandBottom, targetBandTop, min), targetBand.localPosition.z);
        targetBand.localScale = new Vector2 (1, Mathf.Lerp (0, totalHeight, margin));
    }
}
