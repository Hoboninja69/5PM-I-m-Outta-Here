using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MicrogameManager : MonoBehaviour
{
    public static MicrogameManager Instance;

    public Microgame currentMicrogame { get { return microgameQueue[currentMicrogameIndex]; } }

    [SerializeField]
    private Microgame[] allMicrogames;
    [SerializeField]
    private int microgamesPerGame;

    [HideInInspector]
    public Microgame[] microgameQueue { get; private set; }
    [HideInInspector]
    public int currentMicrogameIndex { get; private set; }
    public int remainingMicrogames { get { return microgameQueue.Length - currentMicrogameIndex; } }

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

        EventManager.Instance.OnTransitionSceneEnd += LoadCurrent;
        EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
        EventManager.Instance.OnUIButtonPressed += OnUIButtonPressed;

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

    public void LoadCurrent ()
    {
        SceneManager.LoadScene (currentMicrogame.SceneName);
        GameManager.Instance.PauseGame ();
        EventManager.Instance.MicrogameLoad (currentMicrogame);
    }

    public void StartCurrent ()
    {
        GameManager.Instance.ResumeGame ();
        EventManager.Instance.MicrogameStart (currentMicrogame);
    }


    private void OnMicrogameEnd (MicrogameResult result)
    {
        GameManager.Instance.PauseGame ();
    }

    private void OnUIButtonPressed (string buttonName)
    {
        switch (buttonName)
        {
            case "MicrogameInfoOK":
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
        EventManager.Instance.OnTransitionSceneEnd -= LoadCurrent;
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
