using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwaySequence : MonoBehaviour
{
    private HallwayGenerator generator;

    private static List<HallChunkConfig> remainingConfigs;


    private void Start()
    {
        generator = GetComponent<HallwayGenerator> ();
        StartCoroutine (Sequence ()); 
    }

    private IEnumerator Sequence ()
    {
        if (remainingConfigs == null)
            remainingConfigs = new List<HallChunkConfig> (generator.Generate (MicrogameManager.Instance.remainingMicrogames + 1));
        else
        {
            remainingConfigs.RemoveAt (0);
            generator.Generate (remainingConfigs.ToArray ());
        }
        yield return new WaitForSeconds (2f);

        generator.configs[1].associatedChunk.SpawnCoworker ();
        yield return new WaitForSeconds (0.5f);

        EventManager.Instance.TransitionSceneEnd ();
    }
}
