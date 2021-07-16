using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    public GameObject ball;
    public OscillatingGauge angleGauge, powerGauge;
    public Transform angleIndicator, spawn;
    public Vector2 angleRange;

    private State state;

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
        //thrownPlane = Instantiate (ball, spawn.position, spawn.rotation).GetComponent<AirplaneController> ();
        //thrownPlane.throwPower = powerGauge.value;
    }

    private void Update ()
    {
        if (state == State.AngleChoose)
            angleIndicator.localRotation = Quaternion.Euler (Vector3.zero + Vector3.up * Mathf.Lerp (angleRange.x, angleRange.y, angleGauge.value));
    }

    enum State
    {
        AngleChoose,
        PowerChoose,
        Throwing
    }
}
