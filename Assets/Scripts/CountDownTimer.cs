using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public static CountDownTimer Instance;

    private int totalTime;
    private int timeLeft;

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

        EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
        EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
        EventManager.Instance.OnTimerPause += OnTimerPause;
    }

    private void OnMicrogameStart (Microgame microgame)
    {
        totalTime = microgame.TimerLength;
        timeLeft = totalTime;

        InvokeRepeating ("Tick", 0, 1);
    }

    private void OnMicrogameEnd (MicrogameResult result)
    {
        CancelInvoke ();
    }

    private void OnTimerPause ()
    {
        CancelInvoke ();
    }

    private void Tick ()
    {
        EventManager.Instance.TimerTick (timeLeft--);

        if (timeLeft < 0)
            EventManager.Instance.MicrogameEnd (MicrogameResult.OutOfTime);
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnMicrogameStart -= OnMicrogameStart;
        EventManager.Instance.OnMicrogameEnd -= OnMicrogameEnd;
    }
}
