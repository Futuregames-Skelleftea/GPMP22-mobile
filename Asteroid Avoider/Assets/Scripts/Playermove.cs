using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Playermove : MonoBehaviour
{

    //Movement Variables
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    private Vector3 moveDirection;

    // gamecomponents/objects
    private Rigidbody rb;
    private Camera mainCamera;

    [SerializeField] private float rotationSpeed;

    void Start()
    {
        mainCamera = Camera.main;

        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        ProcessInput();

        KeepPlayerOnScreen();

        RotateToFaceVelocity();
    }

    private void FixedUpdate()
    {
        // If movedirection is 0 do nothing
        if (moveDirection == Vector3.zero) { return; }

        //addforce equal forcemagnitude in the direction of moveDirection.
        rb.AddForce(moveDirection * forceMagnitude, ForceMode.Force);

        //Set Max velocity so ths ship doesn't infinitly accelerate.
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }


    //method for processing Inputs
    private void ProcessInput()
    {
        //if the screen is being touched
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            //get the touchposition.
            Vector2 touchposition = Touchscreen.current.primaryTouch.position.ReadValue();

            //convert the touch position to the world position.
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchposition);

            //changes the players position and locks it's position on the Z axis.
            moveDirection = transform.position - worldPosition;
            moveDirection.z = 0;
            moveDirection.Normalize();
        }

        // If there is currently nothing touching the screen. Set movedirection to 0
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    //method for screen wrapping to keep player on screen
    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        //right side
        if (viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }

        //left Side
        if (viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }

        //top Side
        if (viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }

        //bottom Side
        if (viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }
        transform.position = newPosition;

    }

    //Method for rotating player in the direction they are moving
    private void RotateToFaceVelocity()
    {
        if(rb.velocity == Vector3.zero) { return; }

        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}