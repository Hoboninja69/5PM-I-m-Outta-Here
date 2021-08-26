using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform screenPlane;
    public GameObject bulletHole;
    public Animator gunAnim;
    public float delay;

    private float lastShoot;

    private void Start ()
    {
        InputManager.Instance.OnMouseDownLeft += Shoot;
        //InputManager.Instance.OnMouseMoved += OnMouseMoved;
    }

    private void Update ()
    {
        if (Time.timeScale > 0)
            transform.forward = Tools.GetRayPlaneIntersectionPoint (screenPlane.position, screenPlane.up, InputManager.cursorRay) - transform.position;
    }

    private void Shoot ()
    {
        if (Time.time - lastShoot < delay) return;
        lastShoot = Time.time;

        gunAnim.SetTrigger ("Shoot");
        if (Physics.Raycast (InputManager.cursorRay, out RaycastHit hit, 20))
        {
            if (hit.collider.TryGetComponent (out VirusController virus))
                virus.Shoot ();

            Transform hole = Instantiate (bulletHole, hit.point + hit.normal * 0.01f, Quaternion.identity).transform;
            hole.forward = hit.normal;
            hole.rotation = hole.rotation.SetEulerZ (Random.Range (0, 360));
            hole.localScale = Vector3.one * Random.Range (0.9f, 1.1f);
        }
    }

    private void OnDestroy ()
    {
        InputManager.Instance.OnMouseDownLeft -= Shoot;
        //InputManager.Instance.OnMouseMoved -= OnMouseMoved;
    }
}
