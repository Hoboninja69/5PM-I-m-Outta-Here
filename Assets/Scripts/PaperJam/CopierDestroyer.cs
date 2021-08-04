using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopierDestroyer : MonoBehaviour
{
    public Transform cam;
    public CameraShake camShake;
    public Vector2 camRotXRange;
    public Mesh[] states;
    public int clicksToWin;
    public float posShake, scaleShake, rotShake;
    public MeshFilter mesh;

    private int clickCount;
    private int clicksPerState;
    private Vector3 basePos, baseScale, baseRot;
    private Vector3 targetPos, targetScale, targetRot;
    private float camTargetRotX;
    private bool destroyed = false;

    private void Start ()
    {
        basePos = transform.position;
        targetPos = transform.position;
        baseScale = transform.localScale;
        targetScale = transform.localScale;
        baseRot = transform.rotation.eulerAngles;
        targetRot = transform.rotation.eulerAngles;
        camTargetRotX = camRotXRange.x;

        clicksPerState = Mathf.Clamp (clicksToWin / (states.Length - 1), 1, clicksToWin);
        clicksToWin = clicksPerState * (states.Length - 1);
        print ($"ClicksPerState: {clicksPerState}, ClicksToWin: {clicksToWin}");

        GetComponent<Interactable> ().OnInteract += OnInteract;
    }

    private void OnInteract ()
    {
        AudioManager.Instance.PlayAtLocation ("CopierHit", transform.position, 0.9f, pitchMult: Random.Range (0.8f, 1.2f));
        AudioManager.Instance.PlayAtLocation ("Punch", transform.position, 0.9f, pitchMult: Random.Range (0.8f, 1.2f));

        camShake.smoothSpeed = Mathf.Clamp (camShake.smoothSpeed + 0.75f, 0, 5);
        cam.rotation = Quaternion.Euler (Mathf.Lerp (camRotXRange.x, camRotXRange.y, (float)clickCount / clicksToWin), cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z);

        if (!destroyed)
            mesh.mesh = states [Mathf.FloorToInt (++clickCount / clicksPerState)];
        RandomiseTransform ();
        transform.localScale = new Vector3 (transform.localScale.x, targetScale.y, transform.localScale.z);

        if (!destroyed && clickCount >= clicksToWin)
        {
            print ("WIN"); 
            destroyed = true;
            EventManager.Instance.MicrogameEnd (MicrogameResult.Win);
        }
    }

    private void Update ()
    {
        transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * 40);
        transform.localScale = Vector3.Lerp (transform.localScale, targetScale, Time.deltaTime * 40);
        transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (targetRot), Time.deltaTime * 40);
        camShake.smoothSpeed = Mathf.Lerp (camShake.smoothSpeed, 0, Time.deltaTime * 4);
    }

    private void RandomiseTransform ()
    {
        targetPos = basePos + new Vector3 (Random.Range (-posShake, posShake), 0, Random.Range (-posShake, posShake));
        targetScale = baseScale + new Vector3 (Random.Range (-scaleShake, scaleShake), -((float)(clickCount % clicksPerState) / clicksPerState) * ((float)1 / states.Length), Random.Range (-scaleShake, scaleShake));
        targetRot = baseRot + new Vector3 (Random.Range (-rotShake, rotShake), Random.Range (-rotShake, rotShake), Random.Range (-rotShake, rotShake));
    }

    private void OnDestroy ()
    {
        GetComponent<Interactable> ().OnInteract -= OnInteract;
    }
}
