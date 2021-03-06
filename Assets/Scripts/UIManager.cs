using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject canvasObject, worldCanvasObject;
    [SerializeField]
    private GameObject infoScreen, timer, resultScreen, menuScreen;
    [SerializeField]
    private PayslipFiller payslip;
    [SerializeField]
    private Text timerText, timerShadowText, resultScreenTitle, resultScreenSubtitle;
    [SerializeField]
    private Image infoScreenImage;
    [SerializeField]
    private Texture2D cursorUp, cursorDown;

    private Canvas canvas;
    private readonly Vector2 cursorHotspot = new Vector2 (19, 10);

    public void Initialise ()
    {
        if (Instance != null)
        {
            Destroy (canvasObject);
            Destroy (worldCanvasObject);
            Destroy (this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad (canvasObject);
            DontDestroyOnLoad (worldCanvasObject);
            DontDestroyOnLoad (gameObject);
        }

        //canvas = canvasObject.GetComponent<Canvas> ();

        EventManager.Instance.OnUIButtonPressed += OnUIButtonPressed;
        EventManager.Instance.OnGameLoad += OnGameLoad;
        EventManager.Instance.OnMicrogameLoad += OnMicrogameLoad;
        EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
        EventManager.Instance.OnTimerTick += OnTimerTick;
        EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
        EventManager.Instance.OnGameEnd += OnGameEnd;
        InputManager.Instance.OnMouseDownLeftPersistent += OnMouseDown;
        InputManager.Instance.OnMouseDownRight += OnMouseDown;
        InputManager.Instance.OnMouseUpLeftPersistent += OnMouseUp;
        InputManager.Instance.OnMouseUpRight += OnMouseUp;

        OnMouseUp ();
    }

    //public void SetUseWorldSpace (bool useWorldSpace)
    //{
    //    canvas.renderMode = useWorldSpace ? RenderMode.WorldSpace : RenderMode.ScreenSpaceOverlay;
    //    if (useWorldSpace)
    //        canvas.worldCamera = Camera.main;
    //}

    private void SetActiveAll (bool active)
    {
        infoScreen.SetActive (active);
        timer.SetActive (active);
        resultScreen.SetActive (active);
        menuScreen.SetActive (active);
        payslip.SetActive (active);
    }

    private void OnUIButtonPressed (string buttonName)
    {
        switch (buttonName)
        {
            case "StartGame":
                menuScreen.SetActive (false);
                //SetUseWorldSpace (false);
                EventManager.Instance.GameStart ();
                break;
            case "MicrogameResultOK":
                resultScreen.SetActive (false);
                break;
            case "MainMenu":
                EventManager.Instance.GameReset ();
                break;
        }
    }

    private void OnMouseDown ()
    {
        Cursor.SetCursor (cursorDown, cursorHotspot, CursorMode.ForceSoftware);
    }

    private void OnMouseUp ()
    {
        Cursor.SetCursor (cursorUp, cursorHotspot, CursorMode.ForceSoftware);
    }

    private void OnGameLoad ()
    {
        SetActiveAll (false);
        //SetUseWorldSpace (true);
        menuScreen.SetActive (true);
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

    private void OnGameEnd (float winRatio)
    {
        payslip.Fill (winRatio);
        payslip.SetActive (true);
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnUIButtonPressed -= OnUIButtonPressed;
        EventManager.Instance.OnGameLoad -= OnGameLoad;
        EventManager.Instance.OnMicrogameLoad -= OnMicrogameLoad;
        EventManager.Instance.OnMicrogameStart -= OnMicrogameStart;
        EventManager.Instance.OnTimerTick -= OnTimerTick;
        EventManager.Instance.OnMicrogameEnd -= OnMicrogameEnd;
        EventManager.Instance.OnGameEnd -= OnGameEnd;
        InputManager.Instance.OnMouseDownLeftPersistent -= OnMouseDown;
        InputManager.Instance.OnMouseDownRight -= OnMouseDown;
        InputManager.Instance.OnMouseUpLeftPersistent -= OnMouseUp;
        InputManager.Instance.OnMouseUpRight -= OnMouseUp;
    }
}
