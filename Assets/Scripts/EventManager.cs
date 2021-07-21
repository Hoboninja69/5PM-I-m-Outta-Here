using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public event Action OnFreeze;
    public void Freeze () => OnFreeze?.Invoke ();
    
    public event Action OnUnfreeze;
    public void Unfreeze () => OnUnfreeze?.Invoke ();

    public event Action OnTransitionSceneEnd;
    public void TransitionSceneEnd () => OnTransitionSceneEnd?.Invoke ();

    public event Action<Microgame> OnMicrogameLoad;
    public void MicrogameLoad (Microgame microgame) => OnMicrogameLoad?.Invoke (microgame);

    public event Action<Microgame> OnMicrogameStart;
    public void MicrogameStart (Microgame microgame) => OnMicrogameStart?.Invoke (microgame);

    public event Action<MicrogameResult> OnMicrogameEnd;
    public void MicrogameEnd (MicrogameResult result) => OnMicrogameEnd?.Invoke (result);
    public void MicrogameEnd (MicrogameResult result, float delay) => StartCoroutine (IMicrogameEnd (result, delay));
    private IEnumerator IMicrogameEnd (MicrogameResult result, float delay) { yield return new WaitForSeconds (delay); OnMicrogameEnd?.Invoke (result); }

    public event Action<int> OnTimerTick;
    public void TimerTick (int timeLeft) => OnTimerTick?.Invoke (timeLeft);

    public event Action<string> OnUIButtonPressed;
    public void UIButtonPressed (string buttonName) => OnUIButtonPressed?.Invoke (buttonName);

    public void Initialise ()
    {
        if (Instance != null)
        {
            Destroy (this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }
    }

    //private void Awake ()
    //{
    //    if (Instance != this)
    //        Destroy (this);
    //}
}
