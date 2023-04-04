using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab; 
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachTimer;
    [SerializeField] private float respawnDelay;

    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;

    private Camera mainCamera;

    private bool isDraging;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    // enables enhanced touch support.
    void OnEnable() {
        EnhancedTouchSupport.Enable();
    }

    // disables enhanced touch support.
    void OnDisable() {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // if there isn't any current ball, exit the function.
        if(currentBallRigidbody == null) { return; }

        // if there isn't any active touch, launch the ball.
        if(Touch.activeTouches.Count == 0){
            if(isDraging){
                LaunchBall();
            }

            isDraging = false;

            return;
        }


        // if touching the screen, drag the ball.
        isDraging = true;

        // make the ball kinematic to stop physics simulation.
        currentBallRigidbody.isKinematic = true;

        Vector2 touchPosition = new Vector2();

        // Get the average position of all active touches.
        foreach(Touch touch in Touch.activeTouches){
            touchPosition += touch.screenPosition;
        }

        touchPosition /= Touch.activeTouches.Count;
        
        // Convert touch position to world position.
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        // Move the ball to the touched position.
        currentBallRigidbody.position = worldPosition;
        
    }

    private void LaunchBall(){

        // Make the ball non-kinematic to start physics simulation.
        currentBallRigidbody.isKinematic = false;

        // Reeset the current ball rigidbody to null.
        currentBallRigidbody = null;

        // Detach the ball from the pivot after a set amount of time.
        Invoke(nameof(DetachBall), detachTimer);

    }
    
    // Detach the ball from the pivot and respawn a new one. 
    private void DetachBall(){

        // Disable the current balls spring joint to detach it from the pivot.
        currentBallSpringJoint.enabled = false;

        // Reset current spring joint to null.
        currentBallSpringJoint = null;

        // Respawn ball after a set amount of time.
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }

    // Spawn a new ball and attach it to the pivot.
    private void SpawnNewBall(){

        // Instantiate a new ball and attach it to the pivots position with no rotation.
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        // Get the rigidbody and spring joint component of the new ball.
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();

        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        // Connect the SpringJoint2D component of the new ball to the 'pivot' object.
        currentBallSpringJoint.connectedBody = pivot;
    }
}
