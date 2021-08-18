using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Interactable))]
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (PhysicsFollow))]
public class PhysicsGrabbable : MonoBehaviour
{
    public bool snapToCursor, hideCursor, useObjectPosition, sticky;
    public Transform movementPlane;
    public float maxReleaseVelocity;
    public event Action OnGrabbed;
    public event Action OnDropped;

    private Interactable interactable;
    private PhysicsFollow follow;
    private Rigidbody rb;
    private Vector3 offset;

    private void Start ()
    {
        interactable = GetComponent<Interactable> ();
        follow = GetComponent<PhysicsFollow> ();
        interactable.OnInteract += Grab;

        rb = GetComponent<Rigidbody> ();
    }

    private void FixedUpdate ()
    {
        follow.target = offset + Tools.GetRayPlaneIntersectionPoint (
            useObjectPosition ? transform.position : movementPlane.position, movementPlane.up, InputManager.cursorRay);
    }

    private void Grab ()
    {
        OnGrabbed?.Invoke ();
        if (!sticky)
            InputManager.Instance.OnMouseUpLeft += Drop;

        if (!snapToCursor)
            offset = Tools.GetRayPlaneIntersectionPoint (movementPlane.position, movementPlane.up,
                new Ray (Camera.main.transform.position, transform.position - Camera.main.transform.position)) -
                Tools.GetRayPlaneIntersectionPoint (movementPlane.position, movementPlane.up, InputManager.cursorRay);
        if (hideCursor)
            Cursor.visible = false;

        follow.following = true;
    }

    private void Drop ()
    {
        OnDropped?.Invoke ();
        InputManager.Instance.OnMouseUpLeft -= Drop;
        if (hideCursor)
            Cursor.visible = true;

        follow.following = false;
        rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxReleaseVelocity);
    }

    private void OnDestroy ()
    {
        interactable.OnInteract -= Grab;
        if (follow.following && !sticky)
            InputManager.Instance.OnMouseUpLeft -= Drop;
    }
}
