using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public Interactable closeButtonInteractable;
    public string openSoundName, closeSoundName;
    public Vector2 popupSize, openPitchRange, closePitchRange;
    public event System.Action<PopupController> OnClose;

    private void Start ()
    {
        closeButtonInteractable.OnInteractUp += OnCloseUp;
        AudioManager.Instance.PlayAtLocation (openSoundName, transform.position, 0.5f, pitchMult: Random.Range (openPitchRange.x, openPitchRange.y));
    }

    private void OnCloseUp ()
    {
        //play close animation
        OnClose?.Invoke (this);
        AudioManager.Instance.PlayAtLocation (closeSoundName, transform.position, 0.5f, Random.Range (closePitchRange.x, closePitchRange.y));
        Destroy (gameObject);
    }

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (transform.position, popupSize, transform.rotation.eulerAngles, Color.blue);
    }

    private void OnDestroy ()
    {
        closeButtonInteractable.OnInteract -= OnCloseUp;
    }
}
