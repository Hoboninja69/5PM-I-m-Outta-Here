using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject infoScreen, timer, resultScreen;
    [SerializeField]
    private Text infoScreenTitle, infoScreenDescription, timerText, resultScreenResult;

    public void Initialise ()
    {
        EventManager.Instance.OnUIButtonPressed += OnUIButtonPressed;
        EventManager.Instance.OnMicrogameLoad += OnMicrogameLoad;
        EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
        EventManager.Instance.OnTimerTick += OnTimerTick;
        EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
    }

    private void OnUIButtonPressed (string buttonName)
    {
        switch (buttonName)
        {
            case "":
                return;
        }
    }

    private void OnMicrogameLoad ()
    {
        infoScreen.SetActive (true);
        infoScreenTitle.text = MicrogameManager.Instance.currentMicrogame.Title;
        infoScreenDescription.text = MicrogameManager.Instance.currentMicrogame.Description;
    }

    private void OnMicrogameStart ()
    {
        infoScreen.SetActive (false);
        timer.SetActive (true);
    }

    private void OnTimerTick (int timeLeft)
    {
        timerText.text = timeLeft.ToString ();
    }

    private void OnMicrogameEnd (MicrogameResult result)
    {
        timer.SetActive (false);
        resultScreen.SetActive (true);
        resultScreenResult.text = "Result: " + result;
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnUIButtonPressed -= OnUIButtonPressed;
        EventManager.Instance.OnMicrogameLoad -= OnMicrogameLoad;
        EventManager.Instance.OnTimerTick -= OnTimerTick;
    }
}
