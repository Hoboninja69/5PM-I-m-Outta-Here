using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPushBack : MonoBehaviour
{
    public float pushback = 5;

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collison");

        if (collision.gameObject.tag == "Player");
        
        }

    }


