using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    private Vector2 powerRange;

    [HideInInspector]
    public float throwPower;
    public event Action OnObstacleHit;

    private Animator animator;
    private Rigidbody rb;

    private void Start ()
    {
        animator = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody> ();
        rb.useGravity = false;

        if (throwPower < powerRange.x)
        {
            animator.SetTrigger ("underpoweredThrow");
            Invoke ("ObstacleHit", 1f);
        }
        else if (throwPower < powerRange.y)
        {
            animator.SetTrigger ("perfectThrow");
            rb.velocity = transform.forward;
        }
        else
        {
            animator.SetTrigger ("overpoweredThrow");
            Invoke ("ObstacleHit", 1f);
        }
    }

    private void OnCollisionEnter (Collision collision)
    {
        ObstacleHit ();
        rb.useGravity = true;
    }

    private void ObstacleHit ()
    {
        OnObstacleHit?.Invoke ();
    }
}
