using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailManager : MonoBehaviour
{
    public Transform movementPlane;
    public GameObject[] mailObjects;
    public int spawnAmount;

    private int remaining;
    public bool failed = false;

    private void Start ()
    {
        SpawnMail ();
    }

    public void MailSorted (TriggerEvent trigger, Collider other)
    {
        bool isLeft = trigger.gameObject.name.Contains ("Left");
        //if dropped and not already sorted
        if (other.TryGetComponent (out Mail mail) && !mail.sorted && !mail.follow.following)
        {
            //if correct category
            if (!failed && ((mail.category == Category.Category1 && !isLeft) || (mail.category == Category.Category2 && isLeft)))
            {
                failed = true;
                EventManager.Instance.MicrogameEnd (MicrogameResult.Lose, 0.5f);
            }

            mail.Deactivate ();
            if (--remaining <= 0)
                EventManager.Instance.MicrogameEnd (MicrogameResult.Win, 0.5f);
        }
    }

    private void SpawnMail ()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnOffset = new Vector3 (Random.Range (-0.5f, 0.5f), i * 0.25f, Random.Range (-0.5f, 0.5f));
            Instantiate (mailObjects[Random.Range (0, mailObjects.Length)], transform.position + spawnOffset, Quaternion.identity, transform).GetComponent<PhysicsGrabbable> ().movementPlane = movementPlane;
        }

        remaining = spawnAmount;
    }
}
