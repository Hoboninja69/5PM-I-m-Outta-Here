using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwaySequence : MonoBehaviour
{
    private HallwayGenerator generator;

    private void Awake()
    {
        generator = GetComponent<HallwayGenerator> ();
        StartCoroutine (Sequence ()); 
    }

    private IEnumerator Sequence ()
    {
        generator.Generate (10);
        yield return new WaitForSeconds (2f);

        generator.configs[1].associatedChunk.SpawnCoworker ();
        yield return new WaitForSeconds (0.5f);

        EventManager.Instance.MicrogameLoadTrigger ();
    }
}
