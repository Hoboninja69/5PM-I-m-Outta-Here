using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Sound[] sounds;
    private Sound[] microgameSounds;

    [SerializeField]
    private GameObject infoScreen, timer, resultScreen;
    [SerializeField]
    private Text infoScreenTitle, infoScreenDescription, timerText, resultScreenResult;

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

    private void OnMicrogameLoad (Microgame microgame)
    {
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
