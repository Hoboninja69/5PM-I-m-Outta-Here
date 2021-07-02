using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPushback : MonoBehaviour
{
    public float pushBack = 5;

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collison");

        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.TryGetComponent (out TRMovement Move))
            {
                ContactPoint contact = collision.GetContact(0);
                Move.Pushback(-contact.normal * pushBack);
            }
        }
    }

}
