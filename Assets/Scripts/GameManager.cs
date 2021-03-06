using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake ()
    {
        if (Instance != null)
        {
            Destroy (gameObject);
            Destroy (this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }

        InitialiseScripts ();
        SubscribeToEvents ();

        StartGame ();
    }

    public void PauseGame ()
    {
        print ("PAUSED");
        Time.timeScale = 0;
        EventManager.Instance.Freeze ();
    }

    public void ResumeGame ()
    {
        print ("UNPAUSED");
        Time.timeScale = 1;
        EventManager.Instance.Unfreeze ();
    }

    private void OnUIButtonPressed (string buttonName)
    {
        switch (buttonName)
        {
            case "QuitGame":
                Application.Quit ();
                break;
            case "Credits":
                SceneManager.LoadScene ("CreditsScene");
                break;
        }
    }

    private void StartGame ()
    {
        //usually the hallway scene would be loaded first
        //and then the microgame manager would load the microgame scene
        //before triggering this event
        //EventManager.Instance.MicrogameLoad ();

        //MicrogameManager.Instance.LoadCurrent ();
        if (EventManager.Instance != null)
            EventManager.Instance.GameLoad ();
    }

    private void InitialiseScripts ()
    {
        FindObjectOfType<EventManager> ()?.Initialise ();
        FindObjectOfType<MicrogameManager> ()?.Initialise ();
        FindObjectOfType<InputManager> ()?.Initialise ();
        FindObjectOfType<UIManager> ()?.Initialise ();
        FindObjectOfType<CursorInteraction> ()?.Initialise ();
        FindObjectOfType<AudioManager> ()?.Initialise ();
        FindObjectOfType<CountDownTimer> ()?.Initialise ();
    }

    private void SubscribeToEvents ()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.OnUIButtonPressed += OnUIButtonPressed;
    }
}
