using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    public string openSoundName, closeSoundName;
    public Vector2 popupSize, openPitchRange, closePitchRange;
    public event System.Action<VirusController> OnClose;

    private void Start ()
    {
        AudioManager.Instance.PlayAtLocation (openSoundName, transform.position, 0.8f, pitchMult: Random.Range (openPitchRange.x, openPitchRange.y));
    }

    public void Shoot ()
    {
        OnClose?.Invoke (this);
        AudioManager.Instance.PlayAtLocation (closeSoundName, transform.position, 0.8f, pitchMult: Random.Range (closePitchRange.x, closePitchRange.y));
        Destroy (gameObject);
    }

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (transform.position, popupSize, transform.rotation.eulerAngles, Color.blue);
    }
}
