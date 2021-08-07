using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTracker : MonoBehaviour
{
    public float TargetHeight;

    private Transform[] Objects;
    private bool gameWon = false;
    void Awake()
    {
        Objects = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (gameWon) return;

        bool AllObjectsFallen = true;
        foreach (Transform CurrentObject in Objects)
        {
            if (CurrentObject.position.y > TargetHeight)
            {
                AllObjectsFallen = false;
                break;
            }
        }

        if (AllObjectsFallen)
        {
            gameWon = true;
            print(gameWon);
            EventManager.Instance.MicrogameEnd(MicrogameResult.Win, 1);
        }
    }
}
