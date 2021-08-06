using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject infoScreen, timer, resultScreen, canvasObject;
    [SerializeField]
    private Text timerText, timerShadowText, resultScreenTitle, resultScreenSubtitle;
    [SerializeField]
    private Image infoScreenImage;

    private Canvas canvas;

    public void Initialise ()
    {
        if (Instance != null)
        {
            Destroy (canvasObject);
            Destroy (this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad (canvasObject);
            DontDestroyOnLoad (gameObject);
        }

        canvas = canvasObject.GetComponent<Canvas> ();

        EventManager.Instance.OnUIButtonPressed += OnUIButtonPressed;
        EventManager.Instance.OnMicrogameLoad += OnMicrogameLoad;
        EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
        EventManager.Instance.OnTimerTick += OnTimerTick;
        EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
    }

    public void SetUseWorldSpace (bool useWorldSpace)
    {
        canvas.renderMode = useWorldSpace ? RenderMode.WorldSpace : RenderMode.ScreenSpaceOverlay;
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
        infoScreenImage.sprite = microgame.Infomercial;
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

        Microgame currentMicrogame = MicrogameManager.Instance.currentMicrogame;
        switch (result)
        {
            case MicrogameResult.Win:
                resultScreenTitle.text = "YOU DID IT!";
                resultScreenSubtitle.text = currentMicrogame.WinMessage != "" ? currentMicrogame.WinMessage :
                    "Great job, intern! Now get out of my office.";
                break;
            case MicrogameResult.Lose:
                resultScreenTitle.text = "UH OH...";
                resultScreenSubtitle.text = currentMicrogame.LoseMessage != "" ? currentMicrogame.LoseMessage :
                    "Yeah... I'm going to have to tell your boss about this one.";
                break;
            case MicrogameResult.OutOfTime:
                resultScreenTitle.text = "TOO SLOW!";
                resultScreenSubtitle.text = currentMicrogame.OutOfTimeMessage != "" ? currentMicrogame.OutOfTimeMessage :
                    "Yeah... I'm going to have to tell your boss about this one.";
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
