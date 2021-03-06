using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewspaperController : MonoBehaviour
{
    public Transform movementPlane, sphereCenter;
    public Transform[] spheres;
    public Animator animator;
    public float verticalOffset, sphereRadius;

    private PhysicsFollow follow;

    private void Start ()
    {
        follow = GetComponent<PhysicsFollow> ();

        InputManager.Instance.OnMouseDownLeft += TriggerThwack;
    }

    void Update()
    {
        follow.target = Tools.GetRayPlaneIntersectionPoint (movementPlane.position, movementPlane.up, InputManager.cursorRay) + Vector3.up * verticalOffset;
    }

    private void TriggerThwack ()
    {
        animator.SetTrigger ("Thwack");
    }

    public void ThwackConnect ()
    {
        AudioManager.Instance.Play ("Thwack", pitchMult: Random.Range (0.9f, 1.1f));
        foreach (Transform sphere in spheres)
        {
            Collider[] hits = Physics.OverlapSphere (sphere.position, sphereRadius);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag ("Spider"))
                {
                    AudioManager.Instance.PlayAtLocation ("Punch", hit.transform, 0.9f, pitchMult: Random.Range (0.9f, 1.1f));
                    if (hit.transform.parent.TryGetComponent (out SpiderMovement spider))
                        spider.Kill ();
                    if (EventManager.Instance != null)
                        EventManager.Instance.MicrogameEnd (MicrogameResult.Win, 0.5f);
                    return;
                }
            }
        }
        //Collider[] hits = Physics.OverlapSphere (sphereCenter.position, sphereRadius);
        //foreach (Collider hit in hits)
        //{
        //    if (hit.CompareTag ("Spider"))
        //    {
        //        AudioManager.Instance.PlayAtLocation ("Punch", hit.transform, 0.9f, pitchMult: Random.Range (0.9f, 1.1f));
        //        if (hit.transform.parent.TryGetComponent (out SpiderMovement spider))
        //            spider.Kill ();
        //        if (EventManager.Instance != null)
        //            EventManager.Instance.MicrogameEnd (MicrogameResult.Win, 0.5f);
        //        break;
        //    }
        //}
    }

    private void OnDestroy ()
    {
        InputManager.Instance.OnMouseDownLeft -= TriggerThwack;
    }

    private void OnDrawGizmos ()
    {
        foreach (Transform sphere in spheres)
        {
            Gizmos.DrawWireSphere (sphere.position, sphereRadius);
        }
    }
}
