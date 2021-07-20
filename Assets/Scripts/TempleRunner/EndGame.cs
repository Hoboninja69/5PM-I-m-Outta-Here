using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public float pushBack = 5;

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collison");

        if (collision.gameObject.tag == "Player")
        {
            EventManager.Instance.MicrogameEnd (MicrogameResult.Win);
            if (collision.gameObject.TryGetComponent(out TRMovement Move))
            {
                Move.enabled = false;
            }
        }
    }

}
