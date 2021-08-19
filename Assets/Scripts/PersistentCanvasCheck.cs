using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvasCheck : MonoBehaviour
{
    private void Awake ()
    {
        if (UIManager.Instance != null && UIManager.Instance.canvasObject != gameObject)
            Destroy (gameObject);
    }
}
