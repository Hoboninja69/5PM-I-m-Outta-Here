using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourController : MonoBehaviour
{
    public PlantManager manager;
    public ParticleSystem waterParticleSystem;
    public float speedIncrementMult, pourSpeedMult;
    public Vector2 waterStartSpeedRange, waterSpawnRateRange;
    [HideInInspector]
    public float pourSpeed { get { return Mathf.Pow (unmultPourSpeed * pourSpeedMult, 2); } }

    private float unmultPourSpeed;
    private ParticleSystem.MainModule waterMain;
    private ParticleSystem.EmissionModule waterEmission;
    private AudioSource source;

    private void Start ()
    {
        InputManager.Instance.OnMouseWheel += OnMouseWheel;
        waterMain = waterParticleSystem.main;
        waterEmission = waterParticleSystem.emission;
        source = GetComponent<AudioSource> ();

        SetParticleProperties ();
    }

    private void OnMouseWheel (float amount)
    {
        unmultPourSpeed = Mathf.Clamp01 (unmultPourSpeed + amount * speedIncrementMult);
        source.volume = unmultPourSpeed;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = unmultPourSpeed * -70;
        transform.rotation = Quaternion.Euler (rotation);

        SetParticleProperties ();
    }

    private void Update ()
    {
        if (manager.currentPlant == null) return;

        if (unmultPourSpeed > 0)
            manager.currentPlant.Fill (pourSpeed);
        else
            manager.currentPlant.CheckFull ();
    }

    private void SetParticleProperties ()
    {
        waterMain.startSpeed = Mathf.Lerp (waterStartSpeedRange.x, waterStartSpeedRange.y, unmultPourSpeed);
        waterEmission.rateOverTime = Mathf.Lerp (waterSpawnRateRange.x, waterSpawnRateRange.y, unmultPourSpeed);
    }

    private void OnDestroy ()
    {
        InputManager.Instance.OnMouseWheel -= OnMouseWheel;
    }
}
