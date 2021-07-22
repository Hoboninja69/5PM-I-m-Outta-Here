using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject infoScreen, timer, resultScreen, canvas;
    [SerializeField]
    private Text infoScreenTitle, infoScreenDescription, timerText, timerShadowText, resultScreenTitle, resultScreenSubtitle;

    public void Initialise ()
    {
        if (Instance != null)
        {
            Destroy (canvas);
            Destroy (this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad (canvas);
            DontDestroyOnLoad (gameObject);
        }

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
            case "MicrogameResultOK":
                resultScreen.SetActive (false);
                return;
        }
    }

    private void OnMicrogameLoad (Microgame microgame)
    {
        Cursor.visible = true;
        infoScreen.SetActive (true);
        infoScreenTitle.text = microgame.Title;
        infoScreenDescription.text = microgame.Description;
    }

    private void OnMicrogameStart (Microgame microgame)
    {
        infoScreen.SetActive (false);
        timer.SetActive (true);
    }

    private void OnTimerTick (int timeLeft)
    {
        timerText.text = timeLeft.ToString ();
        timerShadowText.text = timeLeft.ToString ();
    }

    private void OnMicrogameEnd (MicrogameResult result)
    {
        Cursor.visible = true;
        timer.SetActive (false);
        resultScreen.SetActive (true);
        resultScreenTitle.text = "Result: " + result;
        switch (result)
        {
            case MicrogameResult.Win:
                resultScreenTitle.text = "YOU DID IT!";
                resultScreenSubtitle.text = "Great job, intern! Now get out of my office.";
                break;
            case MicrogameResult.Lose:
                resultScreenTitle.text = "UH OH...";
                resultScreenSubtitle.text = "Yeah... I'm going to have to tell your boss about this one.";
                break;
            case MicrogameResult.OutOfTime:
                resultScreenTitle.text = "TOO SLOW!";
                resultScreenSubtitle.text = "Yeah... I'm going to have to tell your boss about this one.";
                break;
        }
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnUIButtonPressed -= OnUIButtonPressed;
        EventManager.Instance.OnMicrogameLoad -= OnMicrogameLoad;
        EventManager.Instance.OnTimerTick -= OnTimerTick;
    }
}
