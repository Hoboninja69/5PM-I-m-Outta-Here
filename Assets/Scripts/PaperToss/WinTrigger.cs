using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject confetti;

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("PaperBall"))
        {
            confetti.SetActive (true);
            EventManager.Instance.MicrogameEnd (MicrogameResult.Win, 1.5f);
        }
    }
}
