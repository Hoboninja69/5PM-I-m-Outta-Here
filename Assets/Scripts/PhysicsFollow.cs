using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PhysicsFollow : MonoBehaviour
{
    public float gravityMultiplier;
    [Range (0, 20)]
    public float attractionForce;
    [Range (0.5f, 50)]
    public float dampening;    //To prevent wiggling back and forth
    [Range (0, 99)]
    public float velocityLimiting;  //To prevent clipping
    public float objectRadius;  //A rough estimate of the object's radius again for clipping resistance
    public LayerMask obstacle; //Layers that the object should not clip through

    [HideInInspector]
    public bool following = true;
    [HideInInspector]
    public Vector3 target;

    private Rigidbody rb;

    private void Start ()
    {
        rb = GetComponent<Rigidbody> ();
        rb.useGravity = false;
        rb.drag = dampening;
    }

    private void FixedUpdate ()
    {
        rb.AddForce (gravityMultiplier * Physics.gravity, ForceMode.Acceleration);
        if (!following || transform.position == target) return;

        Vector3 displacement = transform.position - target;

        if (Physics.Raycast (transform.position, -displacement, out RaycastHit hit, displacement.magnitude, obstacle))
        {
            //displacement = transform.position - (hit.point + hit.normal * objectRadius);
            displacement = transform.position - hit.point + Vector3.ProjectOnPlane (hit.point - target, hit.normal) - hit.normal * objectRadius;
        }
        Vector3 direction = displacement.normalized;
        float magnitude = displacement.magnitude;

        float forceMagnitude = magnitude * attractionForce * 100f;
        forceMagnitude = Mathf.Clamp ((100 - velocityLimiting) * Mathf.Sqrt (forceMagnitude) - attractionForce * 0.5f * (100 - velocityLimiting), 0, Mathf.Infinity);
        Vector3 force = -direction * forceMagnitude;
        rb.AddForce (force);
    }
}
