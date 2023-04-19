using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] bool isSteering;   //if steering is taking place
    private int currentTouchId;     //the touch id of the touch that is currently controlling the car
    private Rigidbody carRigidbody;
    
    //Movement Variables
    [SerializeField] float acceleration;    //the acceleration to be applied
    [SerializeField] float maxSpeed;    //the maximum speed after which no acceleration will be applied
    [SerializeField] float turnSpeedIncrements; //how much the the turn speed will change depending on how far from the centre the finger/mouse is 
    [SerializeField] float turnSpeed;       //how fast to turn

    // Tilt correction-related variables
    [SerializeField] float tiltDampingFactor = 0.8f; //how much the angular velocity of the hoverboard should be dampened
    [SerializeField] float tiltAdjustFactor = 0.5f; //how hard it should try and reorient itself towards the upwards position
    
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();   //gets the car rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        if (isSteering) Steer();
    }

    //the main steering function being called every frame
    private void Steer()
    {
        //if the screen is being pressed then steer
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Input.GetTouch(currentTouchId).position;   //gets the position of the touch
            float turnValue = Mathf.Lerp(-turnSpeedIncrements, turnSpeedIncrements, touchPosition.x / Screen.width);
            carRigidbody.AddTorque(carRigidbody.transform.up * turnValue * turnSpeed * Time.deltaTime);
        }
        else if(Touchscreen.current.touches.Count == 0) //stop steering when the screen isnt being pressed
        {
            isSteering = false;
        }
    }

    //called by the input event system to start the steering function
    public void StartSteering()
    {
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            //finds the id of the touch which started the steering input
            if(Touchscreen.current.touches[i].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                currentTouchId = i; //sets the current touch id to the id of the touch which began the steering action
            }
        }
        isSteering = true;
    }

    //function that gets called when the finger leaves the input zone
    public void StopSteering()
    {
        currentTouchId = 0;
        isSteering = false;
    }

    //Collision Detection
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            GameManager.instance.LoadMainMenu();    //if the player collides with something labeled as obstacle then load the main menu
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
        //It keeps the car upright
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
