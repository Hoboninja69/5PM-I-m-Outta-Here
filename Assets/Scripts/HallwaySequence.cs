using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwaySequence : MonoBehaviour
{
    public Animator camAnimator;
    private HallwayGenerator generator;

    private static List<HallChunkConfig> remainingConfigs;

    private void Start()
    {
        generator = GetComponent<HallwayGenerator> ();
        StartCoroutine (Sequence ()); 
    }

    private IEnumerator Sequence ()
    {
        GameObject endChunk;
        if (remainingConfigs == null)
            remainingConfigs = new List<HallChunkConfig> (generator.Generate (MicrogameManager.Instance.remainingMicrogames + 1, out endChunk));
        else
        {
            remainingConfigs.RemoveAt (0);
            generator.Generate (remainingConfigs.ToArray (), out endChunk);
        }
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

            camAnimator.SetTrigger ("WalkAgain");
        }
    }
}
