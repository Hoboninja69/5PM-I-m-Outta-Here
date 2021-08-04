using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanController : MonoBehaviour
{
    public StackLeaner stack;
    public float fallSpeed, leanControl, leanControlAcceleration;

    private float lean, controlDirection, controlMult;

    private void Start ()
    {
        InputManager.Instance.OnMouseStayLeft += MouseStayLeft;
        InputManager.Instance.OnMouseStayRight += MouseStayRight;
    }

    private void Update ()
    {
        print ($"direction: {controlDirection}, mult: {controlMult}");
        lean += fallSpeed * Time.deltaTime * (lean * 1.5f + 0.05f) + controlDirection * controlMult;
        lean = Mathf.Clamp (lean,-1, 1);
        if (Mathf.Abs (lean) == 1)
            stack.Fall ();
        stack.SetLean (lean);
    }

    private void MouseStayLeft ()
    {
        if (controlDirection < 0)
            controlMult = 0;
        controlDirection = leanControl;
        controlMult = Mathf.Clamp01 (controlMult += leanControlAcceleration * Time.deltaTime);
    }

    private void MouseStayRight ()
    {
        if (controlDirection > 0)
            controlMult = 0;
        controlDirection = -leanControl;
        controlMult = Mathf.Clamp01 (controlMult += leanControlAcceleration * Time.deltaTime);
    }
}
