using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "MicrogameName", menuName = "Microgame")]
public class Microgame : ScriptableObject
{
    public string Title;
    [Multiline ()]
    public string Description, WinMessage, LoseMessage, OutOfTimeMessage;
    public string SceneName;
    public int TimerLength;
    public Sound[] Sounds;
    public Sound Music;
    public bool showCursor;
}
