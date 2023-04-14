using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private Camera mainCamera;
    

    private Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    
    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }

    // FixedUpdate is called every time the physics system update
    void FixedUpdate()
    {
        // Check if the player is not moving
        if (movementDirection == Vector3.zero) { return; }

        // Add force to the player's rigidbody
        rb.AddForce(movementDirection * forceMagnitude, ForceMode.Force);

        // Limit the player's velocity to maxVelocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void ProcessInput()
    {
        // Process touch input from the primary touch and set the movement direction accordingly
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Get the touch position in screen coordinates
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // Convert the touch position to a world position on the same plane as the player
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            // Calculate the movement direction as the vector from the player's current position to the touch position
            movementDirection = transform.position - worldPosition;

            // Set the z-component of the movement direction to zero (since we're moving in 2D)
            movementDirection.z = 0f;

            // Normalize the movement direction vector
            movementDirection.Normalize();
        }
        else
        {
            // If the primary touch is not pressed, set the movement direction to zero
            movementDirection = Vector3.zero;
        }
    }

    private void KeepPlayerOnScreen()
    {
        /**
        Get current position of player and its viewport position.
        If player is out of screen on the right, move it to the left.
        If player is out of screen on the left, move it to the right.
        If player is out of screen on the top, move it to the bottom.
        If player is out of screen on the bottom, move it to the top.
        */
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }

        else if (viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }

        if (viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }

        else if (viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }

        transform.position = newPosition;
    }

    private void RotateToFaceVelocity()
    {
        // Check if the velocity is zero to avoid division by zero errors
        if (rb.velocity == Vector3.zero) { return; }

        // Calculate the target rotation using the LookRotation function
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);

        // Use Quaternion.Lerp to smoothly interpolate between the current and target rotation
        // based on the rotationSpeed and Time.deltaTime
        transform.rotation = Quaternion.Lerp(
           transform.rotation , targetRotation, rotationSpeed * Time.deltaTime);
    }
}
