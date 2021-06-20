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

    private Microgame[] microgameQueue;
    private int currentMicrogameIndex;

    public void Initialise ()
    {
        if (Instance != null)
            Destroy (this);
        else
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        EventManager.Instance.OnGameStart += RandomiseMicrogameQueue;
        EventManager.Instance.OnMicrogameEnd += NextMicrogame;
        EventManager.Instance.OnUIButtonPressed += OnUIButtonPressed;
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

    public void LoadCurrent ()
    {
        SceneManager.LoadScene (currentMicrogame.SceneName);
        Time.timeScale = 0;
        EventManager.Instance.MicrogameLoad ();
    }

    public void StartCurrent ()
    {
        Time.timeScale = 1;
        EventManager.Instance.MicrogameStart ();
    }

    private void NextMicrogame (MicrogameResult result)
    {
        print ("Microgame Ended. Result: " + result);
        currentMicrogameIndex++;
    }

    private void OnUIButtonPressed (string buttonName)
    {
        if (buttonName == "MicrogameStart")
            StartCurrent ();
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnGameStart -= RandomiseMicrogameQueue;
        EventManager.Instance.OnMicrogameEnd -= NextMicrogame;
        EventManager.Instance.OnUIButtonPressed -= OnUIButtonPressed;
    }
}

public enum MicrogameResult
{
    Win,
    Lose,
    OutOfTime
}
