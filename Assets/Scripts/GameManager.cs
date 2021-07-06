using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void StartGame ()
    {
        //usually the hallway scene would be loaded first
        //and then the microgame manager would load the microgame scene
        //before triggering this event
        //EventManager.Instance.MicrogameLoad ();

        //MicrogameManager.Instance.LoadCurrent ();
        MicrogameManager.Instance.LoadHallway ();
    }

    private void OnMicrogameEnd (MicrogameResult result)
    {
        //load transition scene
    }

    private void InitialiseScripts ()
    {
        FindObjectOfType<EventManager> ()?.Initialise ();
        FindObjectOfType<MicrogameManager> ()?.Initialise ();
        FindObjectOfType<UIManager> ()?.Initialise ();
        FindObjectOfType<AudioManager> ()?.Initialise ();
        FindObjectOfType<CountDownTimer> ()?.Initialise ();
    }

    private void SubscribeToEvents ()
    {
        EventManager.Instance.OnMicrogameEnd += OnMicrogameEnd;
    }
}
