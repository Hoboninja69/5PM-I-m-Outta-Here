using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwaySequence : MonoBehaviour
{
    public Animator camAnimator, menuDoorAnimator;
    public GameObject menuScene;
    private HallwayGenerator generator;

    private GameObject endChunk;

    private void Start()
    {
        generator = GetComponent<HallwayGenerator> ();

        //if first boot
        if (MicrogameManager.Instance.remainingConfigs == null)
        {
            menuScene.SetActive (true);
            MicrogameManager.Instance.remainingConfigs = new List<HallChunkConfig> (generator.Generate (MicrogameManager.Instance.remainingMicrogames + 1, out endChunk));
            EventManager.Instance.OnGameStart += OnGameStart;
        }
        else
        {
            generator.Generate (MicrogameManager.Instance.remainingConfigs.ToArray (), out endChunk);
            StartCoroutine (TransitionSequence ());
        }
    }

    private void OnGameStart ()
    {
        StartCoroutine (MenuSequence ());
    }

    private IEnumerator MenuSequence ()
    {
        AudioManager.Instance.Play ("WatchBeep");
        yield return new WaitForSeconds (0.75f);

        yield return new WaitForSeconds (AudioManager.Instance.Play ("IntroVoice").clip.length - 1.25f);

        camAnimator.SetTrigger ("MenuWalk");
        yield return new WaitForSeconds (4.5f);

        menuDoorAnimator.SetTrigger ("Open");
        print ("Open");
        yield return new WaitForSeconds (2.75f);

        StartCoroutine (TransitionSequence ());
    }

    private IEnumerator TransitionSequence ()
    {
        if (MicrogameManager.Instance.remainingMicrogames > 0)
        {
            EventManager.Instance.TransitionSceneStart ();

            camAnimator.SetTrigger ("HallwayWalk");
            yield return new WaitForSeconds (2f);

            generator.configs[1].associatedChunk.SpawnCoworker ();
            AudioSource source = AudioManager.Instance.PlayAtLocation ("Coworker", generator.configs[1].associatedChunk.coworker.transform.position, 1f);
            source.minDistance = 100f;
            source.maxDistance = 200f;
            yield return new WaitForSeconds (source.clip.length + 0.2f);

            yield return new WaitWhile (() => MicrogameManager.Instance.loadingScene.progress < 0.9f);

            EventManager.Instance.TransitionSceneEnd ();
        }
        else
        {
            camAnimator.SetTrigger ("HallwayWalk");
            yield return new WaitForSeconds (2f); 
            
            endChunk.GetComponentInChildren<Animator> ().SetTrigger ("Open");
            yield return new WaitForSeconds (1f);

            camAnimator.SetTrigger ("HallwayExit");
            yield return new WaitForSeconds (3.5f);

            EventManager.Instance.GameEnd (MicrogameManager.Instance.winRatio);
        }
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnGameStart -= OnGameStart;
    }
}
