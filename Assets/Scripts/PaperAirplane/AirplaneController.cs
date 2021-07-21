using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    public float launchSpeed, gravityInfluence, rollReturnSpeed;
    public float pitchSensitivity, rollSensitivity, yawSensitivity;
    public UnityEngine.Animations.PositionConstraint camConstraint;

    private Rigidbody rb;
    private float pitch, roll, yaw, currentSpeed;
    private bool fall = false, autoPilot = false;

    private void Start ()
    {
        currentSpeed = launchSpeed;
        rb = GetComponent<Rigidbody> ();
        InputManager.Instance.OnMouseMoved += MouseMoved;
    }

    private void MouseMoved (Vector2 movement)
    {
        pitch = Mathf.Clamp (pitch + movement.y * pitchSensitivity, -75, 75);
        roll = Mathf.Clamp (roll - movement.x * rollSensitivity, -60, 60);
    }

    private void Update ()
    {
        if (fall || autoPilot) return;

        roll = Mathf.Lerp (roll, 0, Time.deltaTime * rollReturnSpeed);
        yaw -= roll * yawSensitivity * Time.deltaTime;
        Quaternion targetRotation = Quaternion.Euler (pitch, yaw, roll);
        transform.rotation = targetRotation;
    }

    private void FixedUpdate ()
    {
        if (fall)
        {
            rb.AddForce (Physics.gravity, ForceMode.Acceleration);
            return;
        }

        currentSpeed += pitch * gravityInfluence * Time.fixedDeltaTime;
        //if (currentSpeed <= 0)
        //    fall = true;

        rb.velocity = transform.forward * (currentSpeed);
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.collider.CompareTag ("Obstacle"))
        {
            EventManager.Instance?.MicrogameEnd (MicrogameResult.Lose, 1f);
            fall = true;
            return;
        }

        for (int i = 0; i < collision.contactCount; i++)
        {
            Debug.DrawRay (collision.GetContact (i).point, collision.GetContact (i).normal, Color.blue, 10);
            print (Vector3.Angle (collision.GetContact (i).normal, transform.forward * currentSpeed));
            if (Vector3.Angle (collision.GetContact (i).normal, transform.forward * currentSpeed) > 90)
            {
                fall = true;
                break;
            }
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Win"))
        {
            EventManager.Instance?.MicrogameEnd (MicrogameResult.Win, 0.5f);
            camConstraint.constraintActive = false;
            autoPilot = true;
        }
    }
}
