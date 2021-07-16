using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    public GameObject airplane;
    public OscillatingGauge angleGauge, powerGauge;
    public Transform throwArm, spawn;
    public Vector2 angleRange;

    private State state;
    private AirplaneController thrownPlane;

    private void Awake ()
    {
        InputManager.Instance.OnMouseDownLeft += BeginPowerChoose;
        InputManager.Instance.OnMouseUpLeft += Throw;
    }

    private void BeginAngleChoose ()
    {
        state = State.AngleChoose;
        angleGauge.SetActive (true);
    }

    private void BeginPowerChoose ()
    {
        state = State.PowerChoose;
        angleGauge.SetActive (false);
        powerGauge.SetActive (true);
    }

    private void Throw ()
    {
        state = State.Throwing;
        powerGauge.SetActive (false);
        thrownPlane = Instantiate (airplane, spawn.position, spawn.rotation).GetComponent<AirplaneController> ();
        thrownPlane.throwPower = powerGauge.value;
    }

    private void Update ()
    {
        if (state == State.AngleChoose)
            throwArm.localRotation = Quaternion.Euler (Vector3.zero + Vector3.up * Mathf.Lerp (angleRange.x, angleRange.y, angleGauge.value));
    }

    enum State
    {
        AngleChoose,
        PowerChoose,
        Throwing
    }
}
