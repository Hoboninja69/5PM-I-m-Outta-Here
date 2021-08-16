using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanController : MonoBehaviour
{
    public StackLeaner stack;
    public Transform hipJoint;
    public float fallSpeed, leanControl, leanControlAcceleration;

    private float lean, targetLeanInfluence, leanInfluence, hipLean;

    private void Start ()
    {
        InputManager.Instance.OnMouseStayLeft += MouseStayLeft;
        InputManager.Instance.OnMouseStayRight += MouseStayRight;
    }

    private void Update ()
    {
        leanInfluence = Mathf.Lerp (leanInfluence, targetLeanInfluence, leanControlAcceleration * Time.deltaTime);
        hipLean = Mathf.Lerp (hipLean, targetLeanInfluence / leanControl, Time.deltaTime);

        lean += (fallSpeed * (lean * 1.25f + 0.01f) + leanInfluence) * Time.deltaTime;
        lean = Mathf.Clamp (lean,-1, 1);

        Vector3 hipRot = hipJoint.rotation.eulerAngles;
        hipRot.x = Mathf.Lerp (-30, 30, (hipLean + 1) / 2);
        print (hipLean);
        hipJoint.rotation = Quaternion.Euler (hipRot);

        if (Mathf.Abs (lean) == 1)
            stack.Fall ();
        stack.SetLean (lean);

        targetLeanInfluence = 0;
    }

    private void MouseStayLeft ()
    {
        targetLeanInfluence = leanControl;
    }

    private void MouseStayRight ()
    {
        targetLeanInfluence = -leanControl;
    }
}
