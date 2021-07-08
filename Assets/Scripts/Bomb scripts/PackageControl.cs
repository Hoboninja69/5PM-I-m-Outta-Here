using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PackageControl : MonoBehaviour
{
    public bool hasBomb;
    public GameObject Box_Closed;
    public GameObject Box_Open;
    public GameObject Bomb;

    private Interactable BoxInteractable;
    private bool OpenTrue;

    public void Initialise()
    {
        BoxInteractable = GetComponent<Interactable>();
        BoxInteractable.OnInteract += ClickOn;

        SetOpen(false);
        Bomb.SetActive(hasBomb);
    }

    void ClickOn()
    {
        if (!OpenTrue)
        {
            OpenTrue = true;
            SetOpen(true);
            Debug.Log("box opened");

           // if (hasBomb)
           //     EventManager.Instance.MicrogameEnd(MicrogameResult.win);
           
        }
    }

    void SetOpen(bool open)
    {
        Box_Closed.SetActive(!open);
        Box_Open.SetActive(open);
    }


    


}
