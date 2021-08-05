using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailOnInput : MonoBehaviour
{
    private void Start ()
    {
        InputManager.Instance.OnMouseDownLeft += OnInput;
        InputManager.Instance.OnMouseDownRight += OnInput;
    }

    private void OnInput ()
    {
        EventManager.Instance.MicrogameEnd (MicrogameResult.Lose, 1f);
    }

    private void OnDestroy ()
    {
        InputManager.Instance.OnMouseDownLeft -= OnInput;
        InputManager.Instance.OnMouseDownRight -= OnInput;
    }
}
