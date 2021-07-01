using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonEventTrigger : MonoBehaviour
{
    public string buttonName;
    public void ButtonPressed () => EventManager.Instance.UIButtonPressed (buttonName);
}
