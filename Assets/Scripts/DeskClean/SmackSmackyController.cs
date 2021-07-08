using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmackSmackyController : MonoBehaviour
{
    public Transform planeOrigin;
    public float height;

    private PhysicsFollow follow;

    private void Start()
    {
        follow = GetComponent<PhysicsFollow>();
    }

    private void Update()
    {
        Vector3 targetPosition = Tools.GetRayPlaneIntersectionPoint(planeOrigin.position, Vector3.up, InputManager.cursorRay);
        follow.target = targetPosition + Vector3.up * height;
    }
}
