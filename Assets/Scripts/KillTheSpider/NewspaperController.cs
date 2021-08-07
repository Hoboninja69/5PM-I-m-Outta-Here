using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewspaperController : MonoBehaviour
{
    public Transform movementPlane, sphereCenter;
    public Animator animator;
    public float verticalOffset, sphereRadius;
    public LayerMask mask;

    private PhysicsFollow follow;
    private bool spiderHit = false;

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
        Collider[] hits = Physics.OverlapSphere (sphereCenter.position, sphereRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag ("Spider"))
            {
                spiderHit = true;
                print ("WON!");
                AudioManager.Instance.PlayAtLocation ("Punch", hit.transform, 0.9f, pitchMult: Random.Range (0.9f, 1.1f));
                if (hit.transform.parent.TryGetComponent (out SpiderMovement spider))
                    spider.Kill ();
                break;
            }
        }
    }

    private void OnDrawGizmos ()
    {
        Gizmos.DrawWireSphere (sphereCenter.position, sphereRadius);
    }
}
