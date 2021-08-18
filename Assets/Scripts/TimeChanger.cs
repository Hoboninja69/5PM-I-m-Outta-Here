using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeChanger : MonoBehaviour
{
    private void Start () => EventManager.Instance.OnGameStart += ChangeTime;
    private void ChangeTime () => GetComponent<TextMeshPro> ().text = "5:00 PM";
}
