using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public event Action OnGameStart;
    public void GameStart () => OnGameStart?.Invoke ();

    public event Action OnMicrogameLoad;
    public void MicrogameLoad () => OnMicrogameLoad?.Invoke ();

    public event Action OnMicrogameStart;
    public void MicrogameStart () => OnMicrogameStart?.Invoke ();

    public event Action<MicrogameResult> OnMicrogameEnd;
    public void MicrogameEnd (MicrogameResult result) => OnMicrogameEnd?.Invoke (result);

    public event Action<int> OnTimerTick;
    public void TimerTick (int timeLeft) => OnTimerTick?.Invoke (timeLeft);

    public event Action<string> OnUIButtonPressed;
    public void UIButtonPressed (string buttonName) => OnUIButtonPressed?.Invoke (buttonName);

    public void Initialise ()
    {
        if (Instance != null)
            Destroy (this);
        else
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }
    }

    private void Awake ()
    {
        if (Instance != this)
            Destroy (this);
    }
}
