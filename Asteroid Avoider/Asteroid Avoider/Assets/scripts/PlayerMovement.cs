using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    // The force applied to the player's movement
    [SerializeField] private float forceMagnitude;
    // The maximum velocity of the player
    [SerializeField] private float maxVelocity;
    // The speed at which the player rotates
    [SerializeField] private float rotationSpeed;

    // The main camera in the scene
    private Camera maineCamera;
    // The player's Rigidbody component
    private Rigidbody rb;

    // The direction the player is moving in
    private Vector3 movementDirection;

    void Start()
    {
        // Get the player's Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Get the main camera in the scene
        maineCamera = Camera.main;
    }

    void Update()
    {
        // Check for input and update player movement and rotation accordingly
        ProcessInput();

        // Ensure the player stays within the bounds of the screen
        KeepPlayerOnScreen();

        // Rotate the player to face the direction of their velocity
        RotateToFaceVelocity();
    }

    private void FixedUpdate()
    {
        // If the player isn't moving, do nothing
        if (movementDirection == Vector3.zero)
        {
            return;
        }

        // Add force to the player in the direction of movement
        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);

        // Limit the player's velocity to the maximum velocity set in the Inspector
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    // Check for player input and update movement direction
    private void ProcessInput()
    {
        // If the player is touching the screen
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Get the touch position and convert it to a world position
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 worldPosition = maineCamera.ScreenToWorldPoint(touchPosition);

            // Set the movement direction to the difference between the player's position and the touch position
            movementDirection = transform.position - worldPosition;
            // Set the z-axis of the movement direction to 0 to prevent movement along the z-axis
            movementDirection.z = 0f;
            // Normalize the movement direction to a length of 1
            movementDirection.Normalize();
        }
        // If the player isn't touching the screen
        else
        {
            // Set the movement direction to zero
            movementDirection = Vector3.zero;
        }
    }

    // Keep the player within the bounds of the screen
    private void KeepPlayerOnScreen()
    {
        // Get the player's current position
        Vector3 newPosition = transform.position;

        // Convert the player's position to a viewport position (a normalized coordinate in the screen space)
        Vector3 viewPortPosition = maineCamera.WorldToViewportPoint(transform.position);

        // If the player is off the right side of the screen, move them to the left
        if (viewPortPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        // If the player is off the left side of the screen, move them to the right
        else if (viewPortPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }
        // If the player is off the top side of the screen, move them to the bottom
        if (viewPortPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        // If the player is off the bottom side of the screen, move them to the top
        else if (viewPortPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }

        // Set the new position of the player
        transform.position = newPosition;
    }

    private void RotateToFaceVelocity()
    {
        // If the player is not moving, do not rotate
        if (rb.velocity == Vector3.zero)
        {
            return;
        }

        // Determine the target rotation to face the direction of the player's velocity
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);

        // Gradually rotate the player towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
