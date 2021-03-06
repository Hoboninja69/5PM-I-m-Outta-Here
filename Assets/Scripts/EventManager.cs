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

    public event Action OnGameLoad;
    public void GameLoad () => OnGameLoad?.Invoke ();

    public event Action OnGameStart;
    public void GameStart () => OnGameStart?.Invoke ();
    
    public event Action<float> OnGameEnd;
    public void GameEnd (float winRatio) => OnGameEnd?.Invoke (winRatio);

    public event Action OnGameReset;
    public void GameReset () => OnGameReset?.Invoke ();

    public event Action OnTransitionSceneStart;
    public void TransitionSceneStart () => OnTransitionSceneStart?.Invoke ();
    
    public event Action OnTransitionSceneEnd;
    public void TransitionSceneEnd () => OnTransitionSceneEnd?.Invoke ();

    public event Action<Microgame> OnMicrogameLoad;
    public void MicrogameLoad (Microgame microgame) => OnMicrogameLoad?.Invoke (microgame);

    public event Action<Microgame> OnMicrogameStart;
    public void MicrogameStart (Microgame microgame) => OnMicrogameStart?.Invoke (microgame);

    public event Action<MicrogameResult> OnMicrogameEnd;
    public void MicrogameEnd (MicrogameResult result) { if (!microgameEndBusy) OnMicrogameEnd?.Invoke (result); } 
    public void MicrogameEnd (MicrogameResult result, float delay) { if (!microgameEndBusy) StartCoroutine (IMicrogameEnd (result, delay)); }
    private IEnumerator IMicrogameEnd (MicrogameResult result, float delay) { microgameEndBusy = true; OnTimerPause?.Invoke (); yield return new WaitForSeconds (delay); OnMicrogameEnd?.Invoke (result); microgameEndBusy = false; }
    private bool microgameEndBusy = false;

    public event Action OnTimerPause;
    public void TimerPause () => OnTimerPause?.Invoke ();

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
