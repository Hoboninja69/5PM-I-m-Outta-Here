using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    private int totalTime;
    private int timeLeft;

    public void Initialise ()
    {
        EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
    }

    public void OnMicrogameStart ()
    {
        totalTime = MicrogameManager.Instance.currentMicrogame.TimerLength;
        timeLeft = totalTime;

        InvokeRepeating ("Tick", 0, 1);
    }

    private void Tick ()
    {
        EventManager.Instance.TimerTick (timeLeft--);

        if (timeLeft < 0)
        {
            CancelInvoke ();
            EventManager.Instance.MicrogameEnd (MicrogameResult.OutOfTime);
        }
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
    }
}
