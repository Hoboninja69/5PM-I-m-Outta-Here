using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject canvasObject;
    [SerializeField]
    private GameObject infoScreen, timer, resultScreen, menuScreen;
    [SerializeField]
    private Text timerText, timerShadowText, resultScreenTitle, resultScreenSubtitle;
    [SerializeField]
    private Image infoScreenImage;
    [SerializeField]
    private Texture2D cursorUp, cursorDown;

    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private Vector2 canvasPos, canvasSize;
    private readonly Vector2 cursorHotspot = new Vector2 (19, 10);

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
        canvasRectTransform = canvasObject.GetComponent<RectTransform> ();
        canvasPos = canvasRectTransform.anchoredPosition;
        canvasSize = canvasRectTransform.sizeDelta;

        EventManager.Instance.OnUIButtonPressed += OnUIButtonPressed;
        EventManager.Instance.OnGameLoad += OnGameLoad;
        EventManager.Instance.OnMicrogameLoad += OnMicrogameLoad;
        EventManager.Instance.OnMicrogameStart += OnMicrogameStart;
        EventManager.Instance.OnTimerTick += OnTimerTick;
        EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
        InputManager.Instance.OnMouseDownLeft += OnMouseDown;
        InputManager.Instance.OnMouseDownRight += OnMouseDown;
        InputManager.Instance.OnMouseUpLeft += OnMouseUp;
        InputManager.Instance.OnMouseUpRight += OnMouseUp;

        OnMouseUp ();
    }

    public void SetUseWorldSpace (bool useWorldSpace)
    {
        canvas.renderMode = useWorldSpace ? RenderMode.WorldSpace : RenderMode.ScreenSpaceOverlay;
        if (useWorldSpace)
            canvas.worldCamera = Camera.main;
        canvasRectTransform.anchoredPosition = canvasPos;
        canvasRectTransform.sizeDelta = canvasSize;
    }

    private void OnUIButtonPressed (string buttonName)
    {
        switch (buttonName)
        {
            case "StartGame":
                menuScreen.SetActive (false);
                SetUseWorldSpace (false);
                EventManager.Instance.GameStart ();
                break;
            case "MicrogameResultOK":
                resultScreen.SetActive (false);
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
        SetUseWorldSpace (true);
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


    private void OnDestroy ()
    {
        EventManager.Instance.OnUIButtonPressed -= OnUIButtonPressed;
        EventManager.Instance.OnMicrogameLoad -= OnMicrogameLoad;
        EventManager.Instance.OnTimerTick -= OnTimerTick;
    }
}
