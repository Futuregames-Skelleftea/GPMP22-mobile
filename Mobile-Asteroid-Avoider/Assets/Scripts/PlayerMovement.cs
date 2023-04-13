using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //variables are set in the inspector
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;

    //Rigidbody of the player
    private Rigidbody _rB;

    //Maincamera in the scene
    private Camera mainCam;

    //The direction which the player move
    private Vector3 movementDir;
    // Start is called before the first frame update
    void Start()
    {
        //Getting the reference of the maincamera and Rigidbody
        mainCam = Camera.main;
        _rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Calling Processinput, keepplayeronscreen and rotatetofacevelocity methods
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }

    private void FixedUpdate() 
    {
        //Applying force to player in the movement direction
        if(movementDir == Vector3.zero)
        {
            return;
        }

        _rB.AddForce(movementDir * forceMagnitude * Time.deltaTime, ForceMode.Force);

        //Clamping the player velocity to the maximum velocity
        _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, maxVelocity);

    }

    private void ProcessInput()
    {
        //Getting touchs input
        if(Touchscreen.current.primaryTouch.press.isPressed)
        {
            //Getting touch position in screenspace and convert it to worldspace
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 worldPosition = mainCam.ScreenToWorldPoint(touchPosition);  

            //Movement direction subtracting the players position from the touch position
            movementDir = transform.position - worldPosition;        
            movementDir.z = 0f;
            movementDir.Normalize();  
        }
        else
        {
            movementDir = Vector3.zero;
        }
    }

    private void KeepPlayerOnScreen()
    {   
        //Will keep the player on screen all the time
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = mainCam.WorldToViewportPoint(transform.position);

        if(viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if(viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }

        if(viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if(viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }

        transform.position = newPosition;
    }

    private void RotateToFaceVelocity()
    {
        //Rotating the player velocity to face right direction
        if(_rB.velocity == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation =  Quaternion.LookRotation(_rB.velocity, Vector3.back);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
