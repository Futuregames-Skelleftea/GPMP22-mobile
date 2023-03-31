using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;

    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;

    private Camera mainCamera;
    private bool isDragging;

    void Start()
    {
        mainCamera = Camera.main;  // Set camera variable.

        SpawnNewBall(); // Spawn a ball.
    }

    void Update()
    {
        if (currentBallRigidbody == null) { return; } // Don't run code if the ball rigidbody is null.


        if(!Touchscreen.current.primaryTouch.press.isPressed) // If not pressing screen then set isDragging to false and return.
        {
            if(isDragging) // If touching screen then upon release run Launchball code.
            {
                LaunchBall();
            }

            isDragging = false;

            return;
        }

        isDragging = true;
        currentBallRigidbody.isKinematic = true;  // Ball rigidbody kinematic is set to true.

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue(); // Log touch position value.

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition); // Set worldposition to where you are touching the screen.


        currentBallRigidbody.position = worldPosition; // Set ball rigidbody to worldposition.

        
    }

    private void SpawnNewBall () // Instantiates a ball prefab at pivots position and gets the rigidbody and springjoint from the prefab.
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;  // sets the instantiated ball springjoint to the pivot point.

    }

    private void LaunchBall() // Ball is launched with rigidbody kinematic set to false and rigidbody set to null.
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;
        
        Invoke(nameof(DetachBall), detachDelay); // After delay run DetachBall code.

    }

    private void DetachBall() // Detaches ball from springjoint and spawns a new ball after a delay.
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);

    }
}
