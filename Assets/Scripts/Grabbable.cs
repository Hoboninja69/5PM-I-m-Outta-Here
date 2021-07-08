using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Interactable))]
public class Grabbable : MonoBehaviour
{
    public bool snapToCursor;
    public bool hideCursor;

    [HideInInspector]
    public bool grabbed = false;

    private Interactable interactable;
    private Vector3 offset;

    private void Start ()
    {
        interactable = GetComponent<Interactable> ();
        interactable.OnInteract += Grab;
    }

    private void Update ()
    {
        if (!grabbed) return;

        transform.position = offset + Tools.GetRayPlaneIntersectionPoint (transform.position, -Camera.main.transform.forward, InputManager.cursorRay);
    }

    private void Grab ()
    {
        InputManager.Instance.OnMouseUpLeft += Drop;
        if (!snapToCursor)
            offset = transform.position - Tools.GetRayPlaneIntersectionPoint (transform.position, -Camera.main.transform.forward, InputManager.cursorRay);
        if (hideCursor)
            Cursor.visible = false;

        grabbed = true;
    }

    private void Drop ()
    {
        InputManager.Instance.OnMouseUpLeft -= Drop;
        if (hideCursor)
            Cursor.visible = true;

        grabbed = false;
    }

    private void OnDestroy ()
    {
        interactable.OnInteract -= Grab;
        if (grabbed)
            InputManager.Instance.OnMouseUpLeft -= Drop;
    }
}
