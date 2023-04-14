using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnituted;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private Camera mainCamera;

    private Vector3 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        mainCamera = Camera.main;
    }

    void Update()
    {
        ProcessInput();

        KeepPlayerOnScreen();

        RotateToFaceVelocity();
    }

    void FixedUpdate() 
    {
        if (movementDirection == Vector3.zero) { return; }

        rb.AddForce(movementDirection * forceMagnituted, ForceMode.Force);  // Adds a set amount of force to a certain direction.

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);  // Clamps the force value.
    
    }

    private void ProcessInput()
    {
        if(Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();  // Record touch position.

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition); // Transform touch to world position.

            movementDirection = transform.position - worldPosition; // Movement direction is set to the opposite side of where you are touching the screen.
            movementDirection.z = 0f;  // Remove Z axis movement.
            movementDirection.Normalize();  // Normalize speed.
        }
        else
        {
            movementDirection = Vector3.zero;  // If not touching the screen, direction is set to zero.
        }
    }

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if(viewportPosition.x > 1)  // If Ship moves past screen edge set newPosition to opposite side, X axis.
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if(viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }

        if(viewportPosition.y > 1)  // If Ship moves past screen edge set newPosition to opposite side, Y axis.
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if(viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }

        transform.position = newPosition;  // Change position to newly set position.
    }

    private void RotateToFaceVelocity()
    {
        if(rb.velocity == Vector3.zero) { return; } // If velocity is zero do not rotate.


        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back); // Rotate towards the velocity the object is heading.

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Smooths the rotation so it doesn't snap rotate.

    }

}
