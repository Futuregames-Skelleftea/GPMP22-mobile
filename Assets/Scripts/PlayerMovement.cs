using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude; //Force for players Rigidbody
    [SerializeField] private float maxVelocity; //Maximum velocity the player's rb can reach
    [SerializeField] private float rotationSpeed; 

    private Rigidbody rb;
    private Camera mainCamera;

    private Vector3 movementDirection; // Direction of player movement

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        ProcessInput(); //User input

        KeepPlayerOnScreen(); // Keeps player within the screen

        RotateToFaceVeloctiy(); //Rotates the player towards its velocity
    }

    void FixedUpdate()
    {
        if(movementDirection == Vector3.zero) { return; } //return if we are not doing anything

        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force); //Add force based on the player movement direction

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity); //Clamp the velocity to the max velocity
    }

    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed) //Checks if we are touching the screen
        {
            // Gets the world space postion from our touch input
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition); 

            //Calculates in which direction we should move in
            movementDirection = transform.position - worldPosition;
            movementDirection.z = 0;
            movementDirection.Normalize();
        }
        else
        {
            movementDirection = Vector3.zero; //Don't move if we are not touching the screen
        }
    }

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position; //Get current position of the player
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position); //Get viewport space position of the player

        // If we are past the RIGHT edge of the screen, move us to the LEFT
        if (viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        // If we are past the LEFT edge of the screen, move us to the RIGHT
        else if (viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }
        // If we are past the TOP edge of the screen, move us to the BOTTOM
        if (viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        // If we are past the BOTTOM edge of the screen, move us to the TOP
        else if (viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }


        transform.position = newPosition; //Set the player's position to the new position
    }

    private void RotateToFaceVeloctiy()
    {
        //Don't rotate if we are not moving
        if(rb.velocity == Vector3.zero)
        {
            return;
        }

        // Calculate our target rotation based on our velocity
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);

        // Interpolate our current rotation towards our target rotation over time
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
