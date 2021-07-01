using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    private AudioSource loopHorn;

    private void Start ()
    {
        EventManager.Instance.OnUIButtonPressed += OnButton;
    }

    private void OnButton (string button)
    {
        switch (button)
        {
            case "PlayAw":
                AudioManager.Instance.Play ("Aw");
                break;
            case "PlayAwRand":
                AudioManager.Instance.Play ("Aw", Random.Range (0.5f, 1f), Random.Range (0.1f, 2f));
                break;
            case "StopAw":
                AudioManager.Instance.Stop ("Aw");
                break;
            case "PlayHornRandLoc":
                AudioManager.Instance.PlayAtLocation ("Horn", Camera.main.transform.position + Vector3.right * Random.Range (-5, 5), 1);
                break;
            case "PlayHornLoop":
                if (loopHorn != null)
                    loopHorn.Stop ();
                loopHorn = AudioManager.Instance.PlayAtLocation ("LoopHorn", Camera.main.transform.position + Vector3.right * Random.Range (-5, 5), 0.75f);
                break;
            case "StopHornLoop":
                if (loopHorn != null)
                    loopHorn.Stop ();
                break;
        }
    }
}
