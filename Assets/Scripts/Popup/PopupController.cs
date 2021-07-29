using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public Interactable closeButtonInteractable;
    public Vector2 popupSize;
    public event System.Action<PopupController> OnClose;

    private void Start ()
    {
        closeButtonInteractable.OnInteract += OnCloseDown;
        closeButtonInteractable.OnInteractUp += OnCloseUp;
    }

    private void OnCloseDown ()
    {
        //change button colour
    }

    private void OnCloseUp ()
    {
        //play close animation
        OnClose?.Invoke ();
        Destroy (gameObject);
    }
}
