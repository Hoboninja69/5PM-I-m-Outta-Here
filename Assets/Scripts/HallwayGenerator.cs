using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayGenerator : MonoBehaviour
{
    public GameObject chunk, endChunk;
    public HallChunkConfig[] configs { get; private set; }

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
        configs = new HallChunkConfig[chunkCount];
        for (int i = 0; i < chunkCount; i++)
            configs[i] = Instantiate (chunk, Vector3.zero + Vector3.forward * 10 * i, Quaternion.identity, transform).GetComponent<HallChunk> ().Generate ();
        Instantiate (endChunk, Vector3.zero + Vector3.forward * 10 * configs.Length, Quaternion.identity, transform);
        return configs;
    }
    
    public HallChunkConfig[] Generate (int chunkCount, out GameObject endChunk)
    {
        configs = new HallChunkConfig[chunkCount];
        for (int i = 0; i < chunkCount; i++)
            configs[i] = Instantiate (chunk, Vector3.zero + Vector3.forward * 10 * i, Quaternion.identity, transform).GetComponent<HallChunk> ().Generate ();
        endChunk = Instantiate (this.endChunk, Vector3.zero + Vector3.forward * 10 * configs.Length, Quaternion.identity, transform);
        return configs;
    }


    public void Generate (HallChunkConfig[] configs)
    {
        this.configs = configs;
        for (int i = 0; i < configs.Length; i++)
            Instantiate (chunk, Vector3.zero + Vector3.forward * 10 * i, Quaternion.identity, transform).GetComponent<HallChunk> ().Generate (configs[i]);
        Instantiate (endChunk, Vector3.zero + Vector3.forward * 10 * configs.Length, Quaternion.identity, transform);
    }

    public void Generate (HallChunkConfig[] configs, out GameObject endChunk)
    {
        this.configs = configs;
        for (int i = 0; i < configs.Length; i++)
            Instantiate (chunk, Vector3.zero + Vector3.forward * 10 * i, Quaternion.identity, transform).GetComponent<HallChunk> ().Generate (configs[i]);
        endChunk = Instantiate (this.endChunk, Vector3.zero + Vector3.forward * 10 * configs.Length, Quaternion.identity, transform);
    }
}
