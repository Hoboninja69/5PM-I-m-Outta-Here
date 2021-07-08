using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInteraction : MonoBehaviour
{
    [SerializeField]
    private LayerMask interactMask;
    [SerializeField]
    private float maxRaycastDistance;
    [SerializeField]
    private QueryTriggerInteraction interactWithTriggers;

    private void Start ()
    {
        InputManager.Instance.OnMouseDownLeft += OnMouseDownLeft;
    }

    private void OnMouseDownLeft ()
    {
        if (InputManager.CastFromCursor (out RaycastHit hit, interactMask, maxRaycastDistance, interactWithTriggers))
        {
            if (hit.collider.TryGetComponent (out Interactable interactable))
                interactable.OnInteract?.Invoke ();
        }
    }

    private void OnDestroy ()
    {
        InputManager.Instance.OnMouseDownLeft -= OnMouseDownLeft;
    }
}
