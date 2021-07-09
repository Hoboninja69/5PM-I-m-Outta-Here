using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObject : MonoBehaviour
{
    public bool pickOnAwake;
    [SerializeField]
    private GameObject[] spawnObjects;

    private GameObject[] childObjects;

    private void Awake ()
    {
        List<GameObject> firstLevelChildren = new List<GameObject> ();
        foreach (Transform child in GetComponentsInChildren<Transform> (true))
        {
            if (child.parent == transform)
                firstLevelChildren.Add (child.gameObject);
        }
        childObjects = firstLevelChildren.ToArray ();

        if (pickOnAwake)
            Generate ();
    }

    public int Generate ()
    {
        foreach (GameObject child in childObjects)
            child.SetActive (false);

        int index = Random.Range (0, spawnObjects.Length + childObjects.Length);

        if (index < spawnObjects.Length)
            Instantiate (spawnObjects[index], transform);
        else
            childObjects[index - spawnObjects.Length].SetActive (true);

        return index;
    }

    public void Generate (int index)
    {
        foreach (GameObject child in childObjects)
            child.SetActive (false);

        if (index < spawnObjects.Length)
            Instantiate (spawnObjects[index], transform);
        else
            childObjects[index - spawnObjects.Length].SetActive (true);
    }

    public void Ungenerate ()
    {
        foreach (GameObject child in childObjects)
            child.SetActive (false);
    }
}
