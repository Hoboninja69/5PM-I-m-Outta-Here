using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampController : MonoBehaviour
{
    public Transform deskPlaneOrigin;
    public LayerMask deskSurface;
    public GameObject deniedStamp;

    private Vector3 targetPosition;
    private Animator animator;

    private void Start ()
    {
        //InputManager.Instance.OnMouseMoved += OnMouseMoved;
        InputManager.Instance.OnMouseDownLeft += OnMouseDownLeft;
        InputManager.Instance.OnMouseUpLeft += OnMouseUpLeft;
    }

    private void Update ()
    {
        if (Physics.Raycast (InputManager.cursorRay, out RaycastHit hit, 10f, deskSurface))
            targetPosition = hit.point;

        transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * 5);
    }

    private void OnMouseDownLeft ()
    {
        if (Physics.Raycast (targetPosition + Vector3.up, Vector3.down * 2, out RaycastHit hit))
        {
            if (hit.collider.CompareTag ("Document"))
            Instantiate (deniedStamp, hit.point + Vector3.up * 0.001f, transform.rotation);
        }
    }
    
    private void OnMouseUpLeft ()
    {

    }
}
