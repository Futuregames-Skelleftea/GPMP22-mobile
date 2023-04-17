using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour

{
    //Camera
    private Camera mainCamera;

    //timers
    [SerializeField] private float detachTime; 
    [SerializeField] private float respawnTime;

    //Prefab components
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;

    //Bool checking if the ball is currently being dragged or not.
    [SerializeField] private bool isDragging;

    private Rigidbody2D currentBallRB2D;
    private SpringJoint2D currentBallSpring;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        newBall();
    }

    // Update is called once per frame
    void Update()
    {
        // If there is no ball then do nothing.
        if(currentBallRB2D == null) {return;}

        // If there is no current touch do nothing.
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;            
            return;
        }

        //check if the ball is currently being dragged, if yes then ball is kinematic and unaffected by gravity.
        isDragging = true;
        currentBallRB2D.isKinematic = true;

        // Get position of touch relative to screen
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        // Make the  balls current position equal to touch position.
        currentBallRB2D.position = worldPosition;

    }

    // Launch Method
    private void LaunchBall()
    {
        //Change ball to dynamic
        currentBallRB2D.isKinematic = false;

        //Remove reference to currentBall.
        currentBallRB2D = null;

        //call function after detachTime
        Invoke(nameof(DetachBall), detachTime);


    }

    //method for detacging ball from the spring joint
    private void DetachBall()
    {
        currentBallSpring.enabled = false;
        currentBallSpring = null;

        //call newBall function after a set time.
        Invoke(nameof (newBall), respawnTime);
    }

    //Method for spawning a new ball.
    private void newBall()
    {
        GameObject ballinstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRB2D = ballinstance.GetComponent<Rigidbody2D>();
        currentBallSpring = ballinstance.GetComponent<SpringJoint2D>();

        currentBallSpring.connectedBody = pivot;
    }
}
