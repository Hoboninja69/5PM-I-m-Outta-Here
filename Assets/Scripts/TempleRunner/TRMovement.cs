using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 30f;
    public float jumpHeight = 30f;
    public float minRotation = -30f;
    public float maxRotation = 30f;
    private float rotation = 0f;
    private Rigidbody rb;
    private float initialRotation;
    private bool leftMouse;
    private bool rightMouse;
    private bool stun;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation.eulerAngles.y;
        InputManager.Instance.OnMouseDownLeft += LeftDown;
        InputManager.Instance.OnMouseUpLeft += LeftUp;
        InputManager.Instance.OnMouseDownRight += RightDown;
        InputManager.Instance.OnMouseUpRight += RightUp;
    }

    void LeftDown()
    {
        leftMouse = true;
    }

    void LeftUp()
    {
        leftMouse = false;
    }

    void RightDown()
    {
        rightMouse = true;
    }

    void RightUp()
    {
        rightMouse = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement of the player happens at the start of the game
        // Rotate player via A/D keys
        // Identify this position, set the vertical axis as the axis to rotate around the set the rotation speed.
        // Then limit the rotation so the character can turn but still keep moving ahead.
        if (leftMouse)
        {
            rotation -= turnSpeed * Time.deltaTime;
            rotation = Mathf.Clamp(rotation, minRotation, maxRotation);

            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y = rotation + initialRotation;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
        if (rightMouse)
        {
            rotation += turnSpeed * Time.deltaTime;
            rotation = Mathf.Clamp(rotation, minRotation, maxRotation);

            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y = rotation + initialRotation;
            transform.rotation = Quaternion.Euler(currentRotation);
        }

        if (stun) return;

        rb.velocity = transform.forward * moveSpeed;
    }



    public void Pushback(Vector3 Direction)
    {
        stun = true;
        rb.AddForce(Direction * 40);
        Invoke("disablestun", 1f);
    }

    private void disablestun()
    {
        stun = false;
    }

  

}
