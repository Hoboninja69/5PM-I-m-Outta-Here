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
        //AudioManager.Instance.PlayAtLocation ("PopupOpen", transform.position, 0.5f, pitchMult: Random.Range (0.9f, 1.1f));
        AudioManager.Instance.PlayAtLocation ("Pop", transform.position, 0.5f, pitchMult: Random.Range (1.75f, 2f));
    }

    private void OnCloseDown ()
    {
        //change button colour
    }

    private void OnCloseUp ()
    {
        //play close animation
        OnClose?.Invoke (this);
        AudioManager.Instance.PlayAtLocation ("Pop", transform.position, 0.5f, Random.Range (0.5f, 0.75f));//Random.Range (0.9f, 1.1f));
        Destroy (gameObject);
    }

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (transform.position, popupSize, transform.rotation.eulerAngles, Color.blue);
    }
}
