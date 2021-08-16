using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mail : MonoBehaviour
{
    public Category category;
    public bool sorted = false;

    [HideInInspector]
    public PhysicsFollow follow;

    private void Start ()
    {
        follow = GetComponent<PhysicsFollow> ();
    }

    public void Deactivate ()
    {
        GetComponent<Interactable> ().active = false;
        sorted = true;
    }
}

public enum Category { Category1, Category2 }
