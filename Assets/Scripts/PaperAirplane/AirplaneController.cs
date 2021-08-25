using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    public float launchSpeed, targetSpeed, speedLerpTime, gravityInfluence, rollReturnSpeed;
    public float pitchSensitivity, rollSensitivity, yawSensitivity;
    public UnityEngine.Animations.PositionConstraint camConstraint;

    private Rigidbody rb;
    private AudioSource source;
    private float pitch, roll, yaw, currentSpeed, startTime;
    private Vector3 lastVelocity;
    private bool fall = false, autoPilot = false;

    private void Start ()
    {
        currentSpeed = launchSpeed;
        rb = GetComponent<Rigidbody> ();
        source = GetComponent<AudioSource> ();
        InputManager.Instance.OnMouseMoved += MouseMoved;
        startTime = Time.time;
    }

    private void MouseMoved (Vector2 movement)
    {
        pitch = Mathf.Clamp (pitch - movement.y * pitchSensitivity, -75, 75);
        roll = Mathf.Clamp (roll - movement.x * rollSensitivity, -60, 60);
    }

    private void Update ()
    {
        if (currentSpeed < targetSpeed)
            currentSpeed = Mathf.Lerp (launchSpeed, targetSpeed, (Time.time - startTime) / speedLerpTime);

        source.volume = Mathf.Lerp (0f, 0.75f, (rb.velocity.magnitude - 6) / 10);
        if (fall || autoPilot) return;

        roll = Mathf.Lerp (roll, 0, Time.deltaTime * rollReturnSpeed);
        yaw -= roll * yawSensitivity * Time.deltaTime;
        Quaternion targetRotation = Quaternion.Euler (pitch, yaw, roll);
        transform.rotation = targetRotation;

        source.panStereo = Mathf.Lerp (-1f, 1f, (yaw + 30) / 60);
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
        //{
        //    EventManager.Instance?.MicrogameEnd (MicrogameResult.Lose, 1f);
        //    fall = true;
        //}

        rb.velocity = transform.forward * (currentSpeed);
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter (Collision collision)
    {
        AudioManager.Instance.Play ("PaperImpact", Mathf.InverseLerp (0, 12, lastVelocity.magnitude - rb.velocity.magnitude) + 0.05f, Random.Range (0.4f, 1.5f));

        if (fall) return;
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (Vector3.Angle (collision.GetContact (i).normal, lastVelocity) > 90)
            {
                print ("LOSE");
                fall = true;
                EventManager.Instance?.MicrogameEnd (MicrogameResult.Lose, 1f);
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

    private void OnDestroy ()
    {
        InputManager.Instance.OnMouseMoved -= MouseMoved;
    }
}
