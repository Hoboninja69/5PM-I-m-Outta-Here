using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "MicrogameName", menuName = "Microgame")]
public class Microgame : ScriptableObject
{
    public string Title;
    [Multiline ()]
    public string Description;
    public string SceneName;
    public int TimerLength;
    public Sound[] MicrogameSounds;
}
