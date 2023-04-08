using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] bool isSteering;
    private Rigidbody carRigidbody;

    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float turnSpeedIncrements; //how much the the turn speed will change depending on how far from the centre the finger/mouse is 
    [SerializeField] float turnSpeed;


    [SerializeField] float tiltDampingFactor = 0.8f; //how much the angular velocity of the hoverboard should be dampened
    [SerializeField] float tiltAdjustFactor = 0.5f; //how hard it should try and reorient itself towards the upwards position
    

    // Start is called before the first frame update
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSteering) Steer();
    }

    private void Steer()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();   //gets the position of the touch
            float turnValue = Mathf.Lerp(-turnSpeedIncrements, turnSpeedIncrements, touchPosition.x / Screen.width);
            carRigidbody.AddTorque(carRigidbody.transform.up * turnValue * turnSpeed * Time.deltaTime);
        }
        else if(Touchscreen.current.touches.Count == 0)
        {
            isSteering = false;
        }
    }

    //called by the input event system to start the steering function
    public void StartSteering()
    {
        isSteering = true;
    }

    public void StopSteering()
    {
        isSteering = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            GameManager.instance.LoadMainMenu();
        }
    }

    private void FixedUpdate()
    {
        float forwardVelcity = transform.InverseTransformDirection(carRigidbody.velocity).x;

        if (forwardVelcity < maxSpeed)
        {
            carRigidbody.AddForce(carRigidbody.transform.forward * acceleration, ForceMode.Force);
        }


        //Below code i found online and only slightly modified
        //It keeps the car upwright
        Quaternion deltaQuat = Quaternion.FromToRotation(carRigidbody.transform.up, Vector3.up);    //gets the rotation from the car to the up position

        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);

        //damping the angular velocity
        carRigidbody.AddTorque(-carRigidbody.angularVelocity * tiltDampingFactor, ForceMode.Acceleration);

        //reorienting itself
        carRigidbody.AddTorque(axis.normalized * angle * tiltAdjustFactor, ForceMode.Acceleration);
    }
}
