using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PackageControl : MonoBehaviour
{
    public GameObject Box_Closed;
    public GameObject Box_Open;
    public GameObject Bomb;

    public event Action<PackageControl> OnBoxOpen;

    private Interactable BoxInteractable;
    private bool isOpen;

    private void Start()
    {
        SetOpen(false);
        SetBomb(false);
        BoxInteractable = GetComponent<Interactable>();
        BoxInteractable.OnInteract += OpenBox;
    }

    private void OpenBox ()
    {
        BoxInteractable.OnInteract -= OpenBox;

        if (isOpen)
            return;

        AudioManager.Instance.PlayAtLocation ("BoxOpen", transform, 0.9f, 1, UnityEngine.Random.Range (0.9f, 1.1f));
        SetOpen(true);
        OnBoxOpen?.Invoke(this);
    }

    public void SetBomb(bool enabled)
    {
        Bomb.SetActive(enabled);
    }

    void SetOpen(bool open)
    {
        isOpen = open;
        Box_Closed.SetActive(!open);
        Box_Open.SetActive(open);
    }
}
