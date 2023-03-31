using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // The magnitude of the force applied to the player
    [SerializeField] private float forceMagnitude;

    // The maximum velocity the player can reach
    [SerializeField] private float maxVelocity;

    // The speed at which the player rotates
    [SerializeField] private float rotationSpeed;

    // The Rigidbody component of the player
    private Rigidbody rb;
    // The main Camera in the scene
    private Camera mainCamera;

    // The direction of the player's movement
    private Vector3 movementDirection;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Processes the input from the player
        ProcessInput();

        // Keeps the player within the bounds of the screen
        KeepPlayerOnScreen();

        // Rotates the player to face the direction of movement
        RotatToFaceVelocity();

    }

    private void FixedUpdate()
    {
        if(movementDirection == Vector3.zero) { return; }

        // Applies force to the player in the movement direction
        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime, ForceMode.Force);

        // Limits the player's velocity to the maximum velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

    }

    private void ProcessInput()
    {
        // If the player is touching the screen
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Gets the position of the touch
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // Converts the touch position to world space
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            // Calculates the direction of movement from the touch position
            movementDirection = transform.position - worldPosition;

            // Resets the Z component of the movement direction
            movementDirection.z = 0f;

            // Normalizes the movement direction
            movementDirection.Normalize();
        }
        // If the player is not touching the screen
        else
        {
            // Sets the movement direction to zero
            movementDirection = Vector3.zero;
        }
    }

    // KeepPlayerOnScreen method is used to keep the player within the bounds of the screen
    private void KeepPlayerOnScreen()
    {
        // Get current position of the player
        Vector3 newPosition = transform.position;

        // Convert player position to viewport position
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.transform.position);

        // If player is outside the right of the screen
        if (viewportPosition.x > 1)
        {
            // Move player to the left by a small amount
            newPosition.x = -newPosition.x + 0.1f;
        }
        // If player is outside the left of the screen
        else if (viewportPosition.x < 0)
        {
            // Move player to the right by a small amount
            newPosition.x = -newPosition.x - 0.1f;
        }

        // If player is outside the top of the screen
        if (viewportPosition.y > 1)
        {
            // Move player down by a small amount
            newPosition.y = -newPosition.y + 0.1f;
        }
        // If player is outside the bottom of the screen
        else if (viewportPosition.y < 0)
        {
            // Move player up by a small amount
            newPosition.y = -newPosition.y - 0.1f;
        }

        // Update player position
        transform.position = newPosition;
    }

    // RotatToFaceVelocity method is used to rotate the player to face the direction of its velocity
    private void RotatToFaceVelocity()
    {
        // If player is not moving, do not rotate
        if (rb.velocity == Vector3.zero) { return; }

        // Calculate target rotation based on player's velocity
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);

        // Smoothly rotate player towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


}
