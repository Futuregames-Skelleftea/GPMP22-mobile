using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;

    private Rigidbody _rB;
    private Camera mainCam;
    private Vector3 movementDir;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        _rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }

    private void FixedUpdate() 
    {
        if(movementDir == Vector3.zero)
        {
            return;
        }

        _rB.AddForce(movementDir * forceMagnitude * Time.deltaTime, ForceMode.Force);

        _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, maxVelocity);

    }

    private void ProcessInput()
    {
        if(Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 worldPosition = mainCam.ScreenToWorldPoint(touchPosition);  

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
        if(_rB.velocity == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation =  Quaternion.LookRotation(_rB.velocity, Vector3.back);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
