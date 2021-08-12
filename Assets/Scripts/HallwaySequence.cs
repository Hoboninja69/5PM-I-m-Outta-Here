using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwaySequence : MonoBehaviour
{
    public Animator camAnimator, menuDoorAnimator;
    public GameObject menuScene;
    private HallwayGenerator generator;

    private static List<HallChunkConfig> remainingConfigs;
    private GameObject endChunk;

    private void Start()
    {
        generator = GetComponent<HallwayGenerator> ();

        //if first boot
        if (remainingConfigs == null)
        {
            menuScene.SetActive (true);
            remainingConfigs = new List<HallChunkConfig> (generator.Generate (MicrogameManager.Instance.remainingMicrogames + 1, out endChunk));
            EventManager.Instance.OnGameStart += OnGameStart;
        }
        else
        {
            remainingConfigs.RemoveAt (0);
            generator.Generate (remainingConfigs.ToArray (), out endChunk);
            StartCoroutine (TransitionSequence ());
        }
    }

    private void OnGameStart ()
    {
        StartCoroutine (MenuSequence ());
    }

    private IEnumerator MenuSequence ()
    {
        camAnimator.SetTrigger ("MenuWalk");
        yield return new WaitForSeconds (4.5f);

        menuDoorAnimator.SetTrigger ("Open");
        print ("Open");
        yield return new WaitForSeconds (2.75f);

        StartCoroutine (TransitionSequence ());
    }

    private IEnumerator TransitionSequence ()
    {
        camAnimator.SetTrigger ("HallwayWalk");
        yield return new WaitForSeconds (2f);

        if (MicrogameManager.Instance.remainingMicrogames > 0)
        {
            generator.configs[1].associatedChunk.SpawnCoworker ();
            yield return new WaitForSeconds (0.5f);

            EventManager.Instance.TransitionSceneEnd ();
        }
        else
        {
            endChunk.GetComponentInChildren<Animator> ().SetTrigger ("Open");
            yield return new WaitForSeconds (1f);

            camAnimator.SetTrigger ("HallwayExit");
            yield return new WaitForSeconds (3.5f);

            Application.Quit ();
        }
    }

    private void OnDestroy ()
    {
        EventManager.Instance.OnGameStart -= OnGameStart;
    }
}
