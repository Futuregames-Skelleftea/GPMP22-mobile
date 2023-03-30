using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    // Serialized fields for ball prefab, pivot, respawn and detach delay
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float detachDelay;

    // References to the current ball's Rigidbody2D and SpringJoint2D
    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;

    // Reference to the main camera
    private Camera mainCamera;
    // Boolean to track if the user is dragging
    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the main camera reference
        mainCamera = Camera.main;

        // Spawn a new ball
        SpawnNewBall();
    }

    // Enable EnhancedTouchSupport when the object is enabled
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    // Disable EnhancedTouchSupport when the object is disabled
    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // If there is no current ball, return
        if (currentBallRigidbody == null)
        {
            return;
        }

        // If there are no active touches, launch the ball if dragging
        if (Touch.activeTouches.Count == 0)
        {
            if(isDragging)
            {
                LaunchBall();
            }

            isDragging = false;
        
            return;
        }

        // Set dragging to true and make the ball kinematic
        isDragging = true;
        currentBallRigidbody.isKinematic = true;

        // Calculate the average touch position
        Vector2 touchPosition = new Vector2();

        foreach(Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition;
        }

        touchPosition /= Touch.activeTouches.Count;

        // Convert touch position to world position and set ball position
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition); 

        currentBallRigidbody.position = worldPosition;
    }

    // Function to spawn a new ball and set up its Rigidbody2D and SpringJoint2D
    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }

    // Function to launch the ball and reset its Rigidbody2D
    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        // Call DetachBall after a specified delay
        Invoke(nameof(DetachBall), detachDelay);
    }

    // Function to detach the ball from the spring joint and respawn a new ball after a delay
    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallRigidbody = null;

        // Call SpawnNewBall after a specified delay
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
