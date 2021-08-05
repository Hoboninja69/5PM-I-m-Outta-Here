using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "MicrogameName", menuName = "Microgame")]
public class Microgame : ScriptableObject
{
    public Sprite Infomercial;
    [Multiline ()]
    public string WinMessage, LoseMessage, OutOfTimeMessage;
    public string SceneName;
    public int TimerLength;
    public bool timerIsWin = false;
    public Sound[] Sounds;
    public Sound Music;
    public bool showCursor;
}
