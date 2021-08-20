using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MicrogameManager : MonoBehaviour
{
    public static MicrogameManager Instance;

    public Microgame currentMicrogame { get { return microgameQueue[currentMicrogameIndex]; } }
    public List<HallChunkConfig> remainingConfigs;

    [SerializeField]
    private Microgame[] allMicrogames;
    [SerializeField]
    private int microgamesPerGame;

    [HideInInspector]
    public Microgame[] microgameQueue { get; private set; }
    [HideInInspector]
    public int currentMicrogameIndex { get; private set; }
    public int remainingMicrogames { get { return microgameQueue.Length - currentMicrogameIndex; } }
    [HideInInspector]
    public int winCount;
    [HideInInspector]
    public float winRatio { get { return (float)winCount / (float)microgamesPerGame; } }
    public AsyncOperation loadingScene { get; private set; }

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

        EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
        EventManager.Instance.OnUIButtonPressed += OnUIButtonPressed;
        EventManager.Instance.OnGameReset += OnGameReset;
        EventManager.Instance.OnTransitionSceneStart += OnTransitionSceneStart;
        EventManager.Instance.OnTransitionSceneEnd += OnTransitionSceneEnd;

        RandomiseMicrogameQueue ();
    }

    public void RandomiseMicrogameQueue ()
    {
        List<Microgame> microgamesLeft = new List<Microgame> (allMicrogames);
        Microgame[] queue = new Microgame[Mathf.Min (microgamesPerGame, allMicrogames.Length)];
        for (int i = 0; i < queue.Length; i++)
        {
            int randIndex = Random.Range (0, microgamesLeft.Count);
            queue[i] = microgamesLeft[randIndex];
            microgamesLeft.RemoveAt (randIndex);
        }

        microgameQueue = queue;
        currentMicrogameIndex = 0;
    }

    public void LoadHallway ()
    {
        SceneManager.LoadScene ("TransitionHallway");
    }

    public void UnloadHallwayAsync (AsyncOperation loadOp)
    {
        SceneManager.UnloadSceneAsync ("TransitionHallway");
        loadingScene = null;
    }

    private void OnTransitionSceneStart ()
    {
        loadingScene = SceneManager.LoadSceneAsync (currentMicrogame.SceneName, LoadSceneMode.Additive);
        loadingScene.allowSceneActivation = false;
    }

    private void OnTransitionSceneEnd ()
    {
        loadingScene.completed += UnloadHallwayAsync;
        loadingScene.allowSceneActivation = true;
        GameManager.Instance.PauseGame ();
        EventManager.Instance.MicrogameLoad (currentMicrogame);
    }

    public void StartCurrent ()
    {
        GameManager.Instance.ResumeGame ();
        EventManager.Instance.MicrogameStart (currentMicrogame);
        Cursor.visible = currentMicrogame.showCursor;
    }

    private void OnGameReset ()
    {
        remainingConfigs = null;
        RandomiseMicrogameQueue ();
        EventManager.Instance.GameLoad ();
        LoadHallway ();
    }

    private void OnMicrogameEnd (MicrogameResult result)
    {
        if (result == MicrogameResult.Win)
            winCount++;

        remainingConfigs.RemoveAt (0);
        GameManager.Instance.PauseGame ();
    }

    private void OnUIButtonPressed (string buttonName)
    {
        switch (buttonName)
        {
            case "MicrogameInfoOK":
                print ("INFO OK PRESSED");
                StartCurrent ();
                break;
            case "MicrogameResultOK":
                GameManager.Instance.ResumeGame ();
                currentMicrogameIndex++;
                LoadHallway ();
                break;
        }
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnMicrogameEnd -= OnMicrogameEnd;
        EventManager.Instance.OnUIButtonPressed -= OnUIButtonPressed;
    }
}

public enum MicrogameResult
{
    Win,
    Lose,
    OutOfTime
}
