using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PhysicsFollow))]
public class StampController : MonoBehaviour
{
    public Transform deskPlaneOrigin;
    public GameObject deniedStamp;
    public float hoverHeight;
    public float stampDelay;

    private PhysicsFollow follow;
    private Vector3 targetPosition;
    private float height;
    private bool wasTouchingSurface;
    private bool mouseDown;
    private float lastStampTime;

    private void Start ()
    {
        InputManager.Instance.OnMouseDownLeft += OnMouseDownLeft;
        InputManager.Instance.OnMouseUpLeft += OnMouseUpLeft;
        follow = GetComponent<PhysicsFollow> ();
        height = hoverHeight;
    }

    private void Update ()
    {
        targetPosition = Tools.GetRayPlaneIntersectionPoint (deskPlaneOrigin.position, Vector3.up, InputManager.cursorRay);
        follow.target = targetPosition + Vector3.up * height;

        bool isTouchingSurface = Physics.Raycast (transform.position + Vector3.up * 0.02f, Vector3.down, out RaycastHit hit, 0.07f);
        if (mouseDown && isTouchingSurface && !wasTouchingSurface && Time.time - lastStampTime > stampDelay && hit.collider.CompareTag ("Stampable"))
        {
            Instantiate (deniedStamp, hit.point + Vector3.up * 0.001f, transform.rotation, hit.collider.transform);
            lastStampTime = Time.time;

            if (hit.collider.TryGetComponent<StampDocument> (out StampDocument document))
                document.Stamp (hit.point);
        }

        wasTouchingSurface = isTouchingSurface;
    }

    private void OnMouseDownLeft ()
    {
        mouseDown = true;
        height = 0.01f;
    }
    
    private void OnMouseUpLeft ()
    {
        mouseDown = false;
        height = hoverHeight;
    }
}
