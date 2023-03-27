using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    // Serialized fields that can be edited in Unity's inspector
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;

    // Private fields for internal use
    private Rigidbody2D currentBallRigedbody;
    private SpringJoint2D currentBallSprintJoint;
    private Camera mainCamera;
    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the main camera
        mainCamera = Camera.main;

        // Spawn the first ball
        SpawnNewBall();

    }

    // Enable Enhanced Touch support
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    // Disable Enhanced Touch support
    void OnDesable()
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // If there is no ball, return
        if (currentBallRigedbody == null) { return; }

        // If there are no active touches, launch the ball
        if (Touch.activeTouches.Count == 0) 
        {
            if(isDragging)
            {
                LaunchBall();
            }

            isDragging = false;
           
            return;
        }

        // If there are active touches, prepare to drag the ball
        isDragging = true;

        // Make the ball's rigidbody kinematic
        currentBallRigedbody.isKinematic = true;

        // Calculate the average position of all active touches
        Vector2 touchPosition = new Vector2();

        foreach(Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition; 
        }

        touchPosition /= Touch.activeTouches.Count;

        // Convert the touch position to world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        // Move the ball to the calculated position
        currentBallRigedbody.position = worldPosition;

    }

    // Spawns a new ball instance
    private void SpawnNewBall()
    {
        // Instantiate a new ball at the pivot's position
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        // Get the new ball's Rigidbody2D and SpringJoint2D components
        currentBallRigedbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSprintJoint = ballInstance.GetComponent<SpringJoint2D>();

        // Connect the new ball to the pivot using a SpringJoint2D
        currentBallSprintJoint.connectedBody = pivot;
    }

    // Launches the ball
    private void LaunchBall()
    {
        // Make the ball's rigidbody non-kinematic
        currentBallRigedbody.isKinematic = false;

        // Set the current ball's rigidbody to null
        currentBallRigedbody = null;

        // Detach the ball from the pivot after a delay
        Invoke("DetachBall", detachDelay);

    }

    // Detaches the ball from the pivot
    private void DetachBall()
    {
        // Disable the SpringJoint2D component
        currentBallSprintJoint.enabled = false;

        // Set the current ball's SpringJoint2D component to null
        currentBallSprintJoint = null;

        // Schedule the spawning of a new ball with a delay determined by the respawnDelay field.
        Invoke(nameof(SpawnNewBall), respawnDelay);

    }

}
