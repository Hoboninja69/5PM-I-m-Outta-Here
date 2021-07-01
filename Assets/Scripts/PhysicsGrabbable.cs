using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Interactable))]
[RequireComponent (typeof (Rigidbody))]
public class PhysicsGrabbable : MonoBehaviour
{
    public bool snapToCursor;
    public bool hideCursor;

    public float gravityMultiplier;
    [Range (0, 20)]
    public float attractionForce;
    [Range (0.5f, 50)]
    public float dampening;    //To prevent wiggling back and forth
    [Range (0, 99)]
    public float velocityLimiting;  //To prevent clipping
    public float maxReleaseVelocity;    //Also to prevent clipping
    public float objectRadius;  //A rough estimate of the object's radius again for clipping resistance
    public LayerMask grabbableObstacle; //Layers that the object should not clip through

    [HideInInspector]
    public bool grabbed = false;

    private Interactable interactable;
    private Rigidbody rb;
    private Vector3 offset;
    private float originalDrag;

    private void Start ()
    {
        interactable = GetComponent<Interactable> ();
        interactable.OnInteract += Grab;

        rb = GetComponent<Rigidbody> ();
        if (gravityMultiplier != 1)
            rb.useGravity = false;
        originalDrag = rb.drag;
    }

    private void FixedUpdate ()
    {
        rb.AddForce (gravityMultiplier * Physics.gravity, ForceMode.Acceleration);
        if (!grabbed) return;

        Vector3 targetPosition = offset + Tools.GetRayPlaneIntersectionPoint (transform.position, -Camera.main.transform.forward, InputManager.cursorRay);
        Vector3 displacement = transform.position - targetPosition;

        if (Physics.Raycast (transform.position, -displacement, out RaycastHit hit, displacement.magnitude, grabbableObstacle, QueryTriggerInteraction.Ignore))
        {
            displacement = transform.position - (hit.point + hit.normal * objectRadius);
        }
        Vector3 direction = displacement.normalized;
        float magnitude = displacement.magnitude;

        float forceMagnitude = magnitude * attractionForce * 100f;
        forceMagnitude = Mathf.Clamp ((100 - velocityLimiting) * Mathf.Sqrt (forceMagnitude) - attractionForce * (100 - velocityLimiting), 0, Mathf.Infinity);
        Vector3 force = -direction * forceMagnitude;
        rb.AddForce (force);
    }

    private void Grab ()
    {
        InputManager.Instance.OnMouseUpLeft += Drop;
        if (!snapToCursor)
            offset = transform.position - Tools.GetRayPlaneIntersectionPoint (transform.position, -Camera.main.transform.forward, InputManager.cursorRay); 
        if (hideCursor)
            Cursor.visible = false;

        grabbed = true;
        rb.drag = dampening;
    }

    private void Drop ()
    {
        InputManager.Instance.OnMouseUpLeft -= Drop;
        if (hideCursor)
            Cursor.visible = true;

        grabbed = false;
        rb.drag = originalDrag;
        rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxReleaseVelocity);
    }

    private void OnDestroy ()
    {
        interactable.OnInteract -= Grab;
        if (grabbed)
            InputManager.Instance.OnMouseUpLeft -= Drop;
    }
}
