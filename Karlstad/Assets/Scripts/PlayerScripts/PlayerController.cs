using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed; // The speed at which the player moves forward
    public float movementSpeed; // The speed at which the player moves left or right
    private Vector2 move; // The current movement input from the player
    public Rigidbody rb; // The rigidbody component attached to the player object
    public float jumpAmount = 10; // The amount of force applied to the player when they jump
    private bool isJumping; // Whether or not the player is currently jumping

    // Called by the input system when the player moves
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    // Called once per frame
    void Update()
    {
        // Move the player based on the current input
        movePlayer();
    }

    // Move the player based on the current input
    public void movePlayer()
    {
        // Move the player forward at a constant speed
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);

        // Calculate the movement based on the current input
        Vector3 movement = Vector3.zero;
        if (move.x < 0) // Move left
        {
            movement = Vector3.left * movementSpeed;
        }
        else if (move.x > 0) // Move right
        {
            movement = Vector3.right * movementSpeed;
        }

        // Apply the movement to the player
        transform.Translate(movement * Time.deltaTime, Space.World);

        // Check if the player is about to exceed the level boundary and adjust their position if necessary
        if (transform.position.x < LevelBoundary.leftSide)
        {
            transform.position = new Vector3(LevelBoundary.leftSide, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > LevelBoundary.rightSide)
        {
            transform.position = new Vector3(LevelBoundary.rightSide, transform.position.y, transform.position.z);
        }
    }

    // Called by the input system when the player jumps
    public void OnJump(InputAction.CallbackContext context)
    {
        // Only allow the player to jump if they are currently on the ground
        if (!isJumping)
        {
            // Apply an upward force to the player and mark them as jumping
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpAmount, ForceMode.Impulse);
            isJumping = true;
        }
    }

    // Called when the player collides with another object
    void OnCollisionEnter(Collision other)
    {
        // If the player collides with the floor, mark them as not jumping
        if (other.gameObject.CompareTag("floor"))
        {
            isJumping = false;
        }
    }

}
