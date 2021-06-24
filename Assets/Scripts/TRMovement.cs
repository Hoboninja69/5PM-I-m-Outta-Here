using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 10f;
    public float jumpHeight = 30f;
    public float minRotation = -30f;
    public float maxRotation = 30f;
    private float rotation = 0f;
    private Rigidbody rb;
    private float initialRotation;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation.eulerAngles.y;

    }

    // Update is called once per frame
    void Update()
    {
        //Movement of the player happens at the start of the game
        this.transform.position += this.transform.forward * moveSpeed * Time.deltaTime;
        // Rotate player via A/D keys
        // Identify this position, set the vertical axis as the axis to rotate around the set the rotation speed.
        // Then limit the rotation so the character can turn but still keep moving ahead.
        if (Input.GetKey(KeyCode.A) == true)
        {

            rotation -= turnSpeed * Time.deltaTime;
            rotation = Mathf.Clamp(rotation, minRotation, maxRotation);

            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y = rotation + initialRotation;
            transform.rotation = Quaternion.Euler(currentRotation);


        }
        if (Input.GetKey(KeyCode.D) == true)
        {

            rotation += turnSpeed * Time.deltaTime;
            rotation = Mathf.Clamp(rotation, minRotation, maxRotation);

            Vector3 currentRotation = transform.rotation.eulerAngles;
            currentRotation.y = rotation + initialRotation;
            transform.rotation = Quaternion.Euler(currentRotation);


        }

        if (Input.GetKey(KeyCode.Space) == true && Mathf.Abs(this.GetComponent<Rigidbody>().velocity.y) < 0.01f)
        {
            this.GetComponent<Rigidbody>().velocity += Vector3.up * this.jumpHeight;
        }



    }
    // Add a gravititational force to the character as it jumps to make it fall faster. 
    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * -30f);
    }
}
