using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform screenPlane;
    public GameObject bulletHole;

    private void Start ()
    {
        InputManager.Instance.OnMouseDownLeft += Shoot;
    }

    private void Update ()
    {
        transform.forward = Tools.GetRayPlaneIntersectionPoint (screenPlane.position, screenPlane.up, InputManager.cursorRay) - transform.position;
    }

    private void Shoot ()
    {
        if (Physics.Raycast (InputManager.cursorRay, out RaycastHit hit, 20))
            Instantiate (bulletHole, hit.point, Quaternion.LookRotation (Vector3.up, hit.normal));
    }
}
