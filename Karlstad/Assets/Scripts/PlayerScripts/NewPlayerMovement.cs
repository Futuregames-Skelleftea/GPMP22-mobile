using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedGainPerSecond = 0.2f;
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private float boundaryPadding = 0.1f;

    private int steerValue;

    // Declare the animator as a private field
    private Animator animator;

    private void Start()
    {
        // Find the animator component on the child object with the model
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        speed += speedGainPerSecond * Time.deltaTime;

        // Calculate the movement direction based on player input
        Vector3 movementDirection = Vector3.forward * speed * Time.deltaTime;
        movementDirection += Vector3.right * steerValue * turnSpeed * Time.deltaTime;

        // Calculate the boundaries with padding
        float leftBoundary = LevelBoundary.leftSide + boundaryPadding;
        float rightBoundary = LevelBoundary.rightSide - boundaryPadding;

        // Check if the desired movement would cause the player to exceed the level boundary
        if (transform.position.x + movementDirection.x < leftBoundary)
        {
            // Clamp the movement direction so the player stops at the boundary
            movementDirection.x = leftBoundary - transform.position.x;
        }
        else if (transform.position.x + movementDirection.x > rightBoundary)
        {
            // Clamp the movement direction so the player stops at the boundary
            movementDirection.x = rightBoundary - transform.position.x;
        }

        transform.Translate(movementDirection);

        // Check if the player has hit the boundary and adjust their position if necessary
        if (transform.position.x < LevelBoundary.leftSide)
        {
            transform.position = new Vector3(LevelBoundary.leftSide, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > LevelBoundary.rightSide)
        {
            transform.position = new Vector3(LevelBoundary.rightSide, transform.position.y, transform.position.z);
        }

        // Set the "TurnLeft" and "TurnRight" bool parameters on the animator based on player input
        if (steerValue < 0)
        {
            animator.SetBool("isTurningLeft", true);
            animator.SetBool("isTurningRight", false);
        }
        else if (steerValue > 0)
        {
            animator.SetBool("isTurningLeft", false);
            animator.SetBool("isTurningRight", true);
        }
        else
        {
            animator.SetBool("isTurningLeft", false);
            animator.SetBool("isTurningRight", false);
        }
    }

    public void Steer(int value)
    {
        steerValue = value;
    }
}