using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlinker : MonoBehaviour
{
    public float onIntensity, offIntensity;
    public Vector2 blinkDelayRange;

    private Light attachedLight;
    private bool on = true;

    private void Start ()
    {
        attachedLight = GetComponent<Light> ();
        StartCoroutine (Blink ());
    }

    private IEnumerator Blink ()
    {
        while (true)
        {
            ToggleLight ();
            yield return new WaitForSeconds (Random.Range (blinkDelayRange.x, blinkDelayRange.y));
        }
    }

    private void ToggleLight ()
    {
        on = !on;
        attachedLight.intensity = on ? onIntensity : offIntensity;
    }
}
