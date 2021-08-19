using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvasCheck : MonoBehaviour
{
    public bool world;
    private void Awake ()
    {
        if (UIManager.Instance != null && (world ? UIManager.Instance.worldCanvasObject != gameObject : UIManager.Instance.canvasObject != gameObject))
            Destroy (gameObject);
    }
}
