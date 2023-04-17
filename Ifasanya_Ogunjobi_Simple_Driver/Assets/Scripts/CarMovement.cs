using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


public class CarMovement : MonoBehaviour
{
    public float carSpeed;
    public float turnSpeed;
    private Rigidbody rb;

    public float gravityMultiplier;













    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    // W and S key for forward and backward keybord keys
    void FixedUpdate()
    {

        Move();
        Turn();
        Fall();

    }

    void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(new Vector3 (Vector3.forward.x, 0, Vector3.forward.z) * carSpeed * 10);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(-(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * carSpeed * 10 / 2));// Backward force should be less than the forward force
        }

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);

        localVelocity.x = 0;

        rb.velocity = transform.TransformDirection(localVelocity);
    }

    void Turn()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(Vector3.up * turnSpeed * 10);// Backward force should be less than the forward force
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(-Vector3.up * turnSpeed * 10);// Backward force should be less than the forward force
        }
    }

    void Fall()
    {
        rb.AddForce(Vector3.down * gravityMultiplier * 10);
    }
}
