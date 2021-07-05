using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayGenerator : MonoBehaviour
{
    public GameObject chunk;

    HallChunkConfig configToSpawn;

    private void Start ()
    {
        HallChunkConfig[] configs = Generate (10);
        configToSpawn = configs[0];
        Invoke ("SpawnCoworker", 2f);
    }

    private void SpawnCoworker ()
    {
        configToSpawn.associatedChunk.SpawnCoworker ();
    }

    /*private HallChunkConfig[][] configArray = new HallChunkConfig[10][];

    private void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Alpha1))
            GenerateArray (0);
        else if (Input.GetKeyDown (KeyCode.Alpha2))
            GenerateArray (1);
        else if (Input.GetKeyDown (KeyCode.Alpha3))
            GenerateArray (2);
        else if (Input.GetKeyDown (KeyCode.Alpha4))
            GenerateArray (3);
        else if (Input.GetKeyDown (KeyCode.Alpha5))
            GenerateArray (4);
        else if (Input.GetKeyDown (KeyCode.Alpha6))
            GenerateArray (5);
        else if (Input.GetKeyDown (KeyCode.Alpha7))
            GenerateArray (6);
        else if (Input.GetKeyDown (KeyCode.Alpha8))
            GenerateArray (7);
        else if (Input.GetKeyDown (KeyCode.Alpha9))
            GenerateArray (8);
        else if (Input.GetKeyDown (KeyCode.Alpha0))
            GenerateArray (9);
    }

    private void GenerateArray (int index)
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in GetComponentsInChildren<Transform> ())
            {
                if (child != null && child != transform)
                    Destroy (child.gameObject);
            }
        }

        if (configArray[index] == null)
            configArray[index] = Generate (10);
        else
            Generate (configArray[index]);
    }*/

    public HallChunkConfig[] Generate (int chunkCount)
    {
        HallChunkConfig[] configs = new HallChunkConfig[chunkCount];
        for (int i = 0; i < chunkCount; i++)
            configs[i] = Instantiate (chunk, Vector3.zero + Vector3.forward * 10 * i, Quaternion.identity, transform).GetComponent<HallChunk> ().Generate ();
        //generate end chunk
        return configs;
    }

    public void Generate (HallChunkConfig[] configs)
    {
        for (int i = 0; i < configs.Length; i++)
            Instantiate (chunk, Vector3.zero + Vector3.forward * 10 * i, Quaternion.identity, transform).GetComponent<HallChunk> ().Generate (configs[i]);
    }
}
