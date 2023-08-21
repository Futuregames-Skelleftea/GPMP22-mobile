using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    // Prefab of the ball to be launched
    [SerializeField] private GameObject ballPrefab;

    // Pivot point to which the ball is attached
    [SerializeField] private Rigidbody2D pivot;

    // Duration of delay before detaching the ball after launching
    [SerializeField] private float delayDuration;

    // Delay before respawning a new ball after detaching
    [SerializeField] private float respawnDelay;

    private Rigidbody2D currentBallRigidBody;
    private SpringJoint2D currentBallSpringJoint;

    private Camera mainCamera;
    private bool isDragging;

    // Start is called before the first frame update
    void Start()
    {
        // Get the main camera for screen-to-world conversion
        mainCamera = Camera.main;

        // Spawn the first ball
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if a ball exists
        if (currentBallRigidBody == null) { return; }

        // Check if the primary touch is not pressed
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            // If previously dragging, launch the ball
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;

            return;
        }

        isDragging = true;

        // Disable physics and move the ball's position
        currentBallRigidBody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidBody.position = worldPosition;
    }

    // Spawn a new ball and attach it to the pivot
    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidBody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }

    // Launch the ball and schedule detachment
    private void LaunchBall()
    {
        currentBallRigidBody.isKinematic = false;
        currentBallRigidBody = null;

        // Delay detachment and respawn
        Invoke(nameof(DetachBall), delayDuration);
    }

    // Detach the ball and schedule respawn
    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        // Delay respawn of a new ball
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
