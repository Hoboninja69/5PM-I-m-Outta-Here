using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailOnInput : MonoBehaviour
{
    public Animator veronica;

    private void Start ()
    {
        InputManager.Instance.OnMouseDownLeft += OnInput;
        InputManager.Instance.OnMouseDownRight += OnInput;
        if (EventManager.Instance != null)
            EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
        else
            OnMicrogameStart ();
    }

    private void OnMicrogameStart (Microgame microgame = null) => AudioManager.Instance.Play ("Rant");

    private void OnInput ()
    {
        AudioManager.Instance.Stop ("Rant");
        AudioManager.Instance.Play ("Interrupted");
        veronica.SetTrigger ("Interrupt");
        EventManager.Instance.MicrogameEnd (MicrogameResult.Lose, 2f);
    }

    private void OnDestroy ()
    {
        InputManager.Instance.OnMouseDownLeft -= OnInput;
        InputManager.Instance.OnMouseDownRight -= OnInput;
        EventManager.Instance.OnMicrogameStart -= OnMicrogameStart;
    }
}
