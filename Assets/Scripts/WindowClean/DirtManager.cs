using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtManager : MonoBehaviour
{
    public GameObject dirt;
    public Vector2 screenSize;
    public int spawnAmount;

    private float remaining;

    private void Start ()
    {
        remaining = spawnAmount;
        for (int i = 0; i < spawnAmount; i++)
        {
            DirtDissolve dirtDissolve = Instantiate (dirt, transform).GetComponent<DirtDissolve> ();
            dirtDissolve.transform.localPosition = Tools.RandomPositionInArea (screenSize, Vector3.one * dirtDissolve.size);
            dirtDissolve.transform.localRotation = Quaternion.Euler (0, 0, Random.Range (0, 360));
            dirtDissolve.OnCleaned += OnDirtCleaned;
        }
    }

    private void OnDirtCleaned (DirtDissolve dirt)
    {
        AudioManager.Instance.PlayAtLocation ("Sparkle", dirt.transform, 0.75f, Random.Range (0.75f, 0.9f), Random.Range (0.95f, 1.05f));
        dirt.OnCleaned -= OnDirtCleaned;
        if (--remaining <= 0)
            EventManager.Instance.MicrogameEnd (MicrogameResult.Win);
    }

    private void OnDrawGizmos ()
    {
        Tools.DrawBox (transform.position, screenSize, transform.rotation.eulerAngles, Color.white);
    }
}
