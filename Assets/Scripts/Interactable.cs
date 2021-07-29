using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool active = true;
    public event Action OnInteract;
    public void Interact () { if (active) OnInteract?.Invoke (); }
}
