using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInteraction : MonoBehaviour
{
    public static CursorInteraction Instance;

    [SerializeField]
    private LayerMask interactMask;
    [SerializeField]
    private float maxRaycastDistance;
    [SerializeField]
    private QueryTriggerInteraction interactWithTriggers;

    public void Initialise ()
    {
        if (Instance != null)
        {
            Destroy (this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        Cursor.lockState = CursorLockMode.Confined;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMouseDownLeft += OnMouseDownLeft;
        }
    }

    private void OnMouseDownLeft ()
    {
        if (InputManager.CastFromCursor (out RaycastHit hit, interactMask, maxRaycastDistance, interactWithTriggers))
        {
            if (hit.collider.TryGetComponent (out Interactable interactable))
                interactable.Interact ();
        }
    }

    private void OnDestroy ()
    {
        InputManager.Instance.OnMouseDownLeft -= OnMouseDownLeft;
    }
}
