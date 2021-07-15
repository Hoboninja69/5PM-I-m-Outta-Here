using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Interactable))]
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (PhysicsFollow))]
public class PhysicsGrabbable : MonoBehaviour
{
    public bool snapToCursor;
    public bool hideCursor;

    public Transform movementPlane;
    //public float gravityMultiplier;
    //[Range (0, 20)]
    //public float attractionForce;
    //[Range (0.5f, 50)]
    //public float dampening;    //To prevent wiggling back and forth
    //[Range (0, 99)]
    //public float velocityLimiting;  //To prevent clipping
    public float maxReleaseVelocity;    //Also to prevent clipping
    //public float objectRadius;  //A rough estimate of the object's radius again for clipping resistance
    //public LayerMask grabbableObstacle; //Layers that the object should not clip through

    private Interactable interactable;
    private PhysicsFollow follow;
    private Rigidbody rb;
    private Vector3 offset;
    private float originalDrag;

    private void Start ()
    {
        interactable = GetComponent<Interactable> ();
        follow = GetComponent<PhysicsFollow> ();
        interactable.OnInteract += Grab;

        rb = GetComponent<Rigidbody> ();
        //if (gravityMultiplier != 1)
        //    rb.useGravity = false;
        originalDrag = rb.drag;
    }

    private void FixedUpdate ()
    {
        //rb.AddForce (gravityMultiplier * Physics.gravity, ForceMode.Acceleration);

        Vector3 targetPosition = offset + Tools.GetRayPlaneIntersectionPoint (movementPlane.position, movementPlane.up, InputManager.cursorRay);
        follow.target = targetPosition;
        //Vector3 displacement = transform.position - targetPosition;

        //if (Physics.Raycast (transform.position, -displacement, out RaycastHit hit, displacement.magnitude, grabbableObstacle, QueryTriggerInteraction.Ignore))
        //{
        //    displacement = transform.position - (hit.point + hit.normal * objectRadius);
        //}
        //Vector3 direction = displacement.normalized;
        //float magnitude = displacement.magnitude;

        //float forceMagnitude = magnitude * attractionForce * 100f;
        //forceMagnitude = Mathf.Clamp ((100 - velocityLimiting) * Mathf.Sqrt (forceMagnitude) - attractionForce * (100 - velocityLimiting), 0, Mathf.Infinity);
        //Vector3 force = -direction * forceMagnitude;
        //rb.AddForce (force);
    }

    private void Grab ()
    {
        InputManager.Instance.OnMouseUpLeft += Drop;
        if (!snapToCursor)
            offset = transform.position - Tools.GetRayPlaneIntersectionPoint (transform.position, -Camera.main.transform.forward, InputManager.cursorRay); 
        if (hideCursor)
            Cursor.visible = false;

        follow.following = true;
        //rb.drag = dampening;
    }

    private void Drop ()
    {
        InputManager.Instance.OnMouseUpLeft -= Drop;
        if (hideCursor)
            Cursor.visible = true;

        follow.following = false;
        //rb.drag = originalDrag;
        rb.velocity = Vector3.ClampMagnitude (rb.velocity, maxReleaseVelocity);
    }

    private void OnDestroy ()
    {
        interactable.OnInteract -= Grab;
        if (follow.following)
            InputManager.Instance.OnMouseUpLeft -= Drop;
    }
}
