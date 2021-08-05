using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    public Vector2 screenSize, waitRange;
    public Transform spider;
    public Animator spiderAnim;
    public float moveSpeed;

    private bool alive = true;

    private void Start ()
    {
        StartCoroutine (MoveBaby ());
    }

    public void Kill ()
    {
        spiderAnim.SetTrigger("Die");
        StopAllCoroutines ();
        alive = false;
    }

    private IEnumerator MoveBaby ()
    {
        while (alive)
        {
            spiderAnim.SetBool("Moving", true);

            Vector3 start = spider.localPosition;
            Vector3 target = RandomScreenPosition (); target.z = start.z;
            Vector3 displacement = target - start;
            float time = Vector2.Distance(start, target) / moveSpeed;

            float radAngle = Mathf.Atan2(displacement.x, displacement.y);
            Debug.DrawRay(spider.position, new Vector2 (Mathf.Sin (radAngle), Mathf.Cos (radAngle)), Color.red, time);
            //Vector3 rotation = new Vector3 (0, 0, radAngle * Mathf.Rad2Deg);

            spider.localRotation = Quaternion.AngleAxis (radAngle * Mathf.Rad2Deg, Vector3.back);
            //spider.forward = displacement;


            for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
            {
                float ratio = elapsed / time;
                spider.localPosition = Vector3.Lerp (start, target, ratio);
                yield return null;
            }
            spider.localPosition = target;

            spiderAnim.SetBool("Moving", false);
            yield return new WaitForSeconds (Random.Range (waitRange.x, waitRange.y));
        }
    }

    private Vector2 RandomScreenPosition ()
    {
        Vector2 maxOffset = new Vector2 (screenSize.x, screenSize.y) / 2;
        return new Vector2 (Random.Range (-maxOffset.x, maxOffset.x), Random.Range (-maxOffset.y, maxOffset.y));
    }

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (transform.position, screenSize, transform.rotation.eulerAngles, Color.blue);
    }
}
