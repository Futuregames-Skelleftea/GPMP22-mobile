using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private float detachDuration; // The amount of time before the ball detaches from the pivot after launching
    [SerializeField] private GameObject ballPrefab; // The prefab of the ball to be instantiated
    [SerializeField] private Rigidbody2D pivot; // The pivot point around which the ball will rotate
    [SerializeField] private float respawnDelay; // The amount of time before a new ball is spawned after the previous ball is detached

    private Camera mainCamera; // The main camera used to convert touch position to world position
    private bool isDragging; // Whether or not the player is currently dragging the ball
    private Rigidbody2D currentBallRigidbody; // The Rigidbody2D component of the current ball being held
    private SpringJoint2D currentBallSpringJoint; // The SpringJoint2D component of the current ball being held

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main; // Get the main camera component in the scene

        SpawnNewBall(); // Spawn a new ball at the beginning of the game
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable(); // Enable Enhanced Touch support
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable(); // Disable Enhanced Touch support
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null) // If there is no current ball being held, return
        {
            return;
        }

        if (Touch.activeTouches.Count == 0) // If there are no active touches, launch the ball and return
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;

            return;
        }

        isDragging = true; // Set isDragging to true since there is an active touch

        currentBallRigidbody.isKinematic = true; // Set the current ball's Rigidbody2D to kinematic to prevent it from moving on its own

        Vector2 touchPosition = new Vector2(); // Initialize a Vector2 to hold the average position of all active touches

        foreach (Touch touch in Touch.activeTouches) // Loop through all active touches to get their positions
        {
            touchPosition += touch.screenPosition; // Add each touch's position to touchPosition
        }

        touchPosition /= Touch.activeTouches.Count; // Divide touchPosition by the number of active touches to get the average position

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition); // Convert touchPosition from screen space to world space

        currentBallRigidbody.position = worldPosition; // Set the current ball's position to the converted touch position
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity); // Instantiate a new ball prefab at the pivot position

        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the new ball
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>(); // Get the SpringJoint2D component of the new ball

        currentBallSpringJoint.connectedBody = pivot; // Set the connected body of the SpringJoint2D component to the pivot
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke(nameof(DetachBall), detachDuration);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
