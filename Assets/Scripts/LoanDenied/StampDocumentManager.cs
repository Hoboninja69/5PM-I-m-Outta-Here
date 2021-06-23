using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampDocumentManager : MonoBehaviour
{
    public int docCount;
    public GameObject[] documentVariations;

    private StampDocument currentDocument;
    private int currentDocIndex;

    private void Start ()
    {
        NextDocument ();
    }

    private void NextDocument ()
    {
        if (currentDocument != null)
        {
            currentDocument.FlyOut ();
            currentDocument.GetComponent<StampDocument> ().OnDocumentStamped -= NextDocument;
        }

        if (++currentDocIndex < docCount)
        {
            currentDocument = Instantiate (documentVariations[Random.Range (0, documentVariations.Length)],
                transform.position + transform.right * -4, Quaternion.identity, transform).GetComponent<StampDocument> ();
            currentDocument.FlyIn ();
            currentDocument.GetComponent<StampDocument> ().OnDocumentStamped += NextDocument;
        }
        else print ("complete!");
    }
}
