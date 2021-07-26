using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Interactable))]
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (PhysicsFollow))]
public class PaperTossController : MonoBehaviour
{
    public bool hideCursor;
    public Transform movementPlane;
    public event Action OnLeaveDesk;

    private Interactable interactable;
    private PhysicsFollow follow;
    private Rigidbody rb;
    private bool leftDesk = false;

    private void Start ()
    {
        interactable = GetComponent<Interactable> ();
        follow = GetComponent<PhysicsFollow> ();
        interactable.OnInteract += Grab;

        rb = GetComponent<Rigidbody> ();
    }

    private void FixedUpdate ()
    {
        if (!leftDesk)
            follow.target = Tools.GetRayPlaneIntersectionPoint (movementPlane.position, movementPlane.up, InputManager.cursorRay);
    }

    private void Grab ()
    {
        InputManager.Instance.OnMouseUpLeft += Drop;
        if (hideCursor)
            Cursor.visible = false;

        follow.following = true;
    }

    private void Drop ()
    {
        InputManager.Instance.OnMouseUpLeft -= Drop;
        if (hideCursor)
            Cursor.visible = true;

        follow.following = false;
        if (rb.velocity.y > 0)
            rb.AddForce (new Vector3 (InputManager.cursorRay.direction.x, -0.1f, InputManager.cursorRay.direction.z).normalized * rb.velocity.y, ForceMode.Impulse);
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Win"))
            EventManager.Instance.MicrogameEnd (MicrogameResult.Win);
        else if (!leftDesk && other.CompareTag ("DeskBoundary"))
        {
            leftDesk = true;
            OnLeaveDesk?.Invoke ();

            interactable.OnInteract -= Grab;
            if (follow.following)
                InputManager.Instance.OnMouseUpLeft -= Drop;
            GetComponent<Interactable> ().active = false;
        }
    }

    private void OnDestroy ()
    {
        interactable.OnInteract -= Grab;
        if (follow.following)
            InputManager.Instance.OnMouseUpLeft -= Drop;
    }
}
