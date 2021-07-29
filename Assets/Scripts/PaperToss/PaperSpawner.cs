using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperSpawner : MonoBehaviour
{
    public GameObject paper;
    public Transform movementPlane;
    public float delay;

    private PaperTossController currentPaper;

    private void Start ()
    {
        SpawnPaper ();
    }

    private void OnPaperLeaveDesk ()
    {
        Invoke ("SpawnPaper", delay);
    }

    private void SpawnPaper ()
    {
        if (currentPaper != null)
            currentPaper.OnLeaveDesk -= OnPaperLeaveDesk;

        currentPaper = Instantiate (paper, transform).GetComponent<PaperTossController> ();
        currentPaper.movementPlane = movementPlane;
        currentPaper.OnLeaveDesk += OnPaperLeaveDesk;
    }

    private void OnDestroy ()
    {
        if (currentPaper != null)
            currentPaper.OnLeaveDesk -= OnPaperLeaveDesk;
    }
}
