using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    public Vector2 screenSize, waitRange;
    public Transform spider;
    public float moveSpeed;

    private bool alive = true;

    private void Start ()
    {
        StartCoroutine (MoveBaby ());
    }

    public void Kill ()
    {
        StopAllCoroutines ();
        alive = false;
    }

    private IEnumerator MoveBaby ()
    {
        while (alive)
        {
            Vector3 start = spider.localPosition;
            Vector3 target = RandomScreenPosition (); target.z = start.z;
            float time = Vector2.Distance (start, target) / moveSpeed;

            for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
            {
                float ratio = elapsed / time;
                spider.localPosition = Vector3.Lerp (start, target, ratio);
                yield return null;
            }
            spider.localPosition = target;

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
