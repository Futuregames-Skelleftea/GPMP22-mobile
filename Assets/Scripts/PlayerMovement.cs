using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
/// No explicit access modifiers
    [SerializeField] float _touchForce;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _rotationSpeed;

    Camera _mainCamera;
    Rigidbody _rigidBody;

    Vector3 _movementDirection;

    void Start()
    {
        _mainCamera = Camera.main;
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.velocity = Vector2.up * 0.01f;
    }

    void Update()
    {
        ProcessInput();
        WrapAround();

    }
/// inconsistent use of access modifiers
    private void FixedUpdate()
    {
        AddShipForce();
        RotateShip();
    }
/// does not "Gets" as there is no return value instead it caches values.
/// could use xml comments for Method comments
    // Gets the touch position if there is any
    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(touchPosition);

            _movementDirection = (transform.position - worldPosition).normalized;
            _movementDirection.z = 0f;
        }

        else
            _movementDirection = Vector3.zero;
    }
/// xml comment, is accurate.
    // Moves the ship based on the touch position
    private void AddShipForce()
    {
        if (_movementDirection == Vector3.zero) return;

        _rigidBody.AddForce(_movementDirection * _touchForce, ForceMode.Force);

        if (_rigidBody.velocity.magnitude > _maxVelocity)
            _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _maxVelocity);
    }
/// comment missing. does not really need one in my opinion but I am here to correct small errors
    private void RotateShip()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_rigidBody.velocity, Vector3.back);

        _rigidBody.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
/// accurate comments in the whole method still missing the xml comment before method though.
    // Puts the ship on the other side of the screen if it goes out of bounds
    private void WrapAround()
    {
        Vector3 newPosition = transform.position;

        Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(transform.position);
        
        // Wrap around the left and right side of the screen
        if (viewportPosition.x > 1f)
            newPosition.x = -newPosition.x + 0.1f;
        else if (viewportPosition.x < 0f)
            newPosition.x = -newPosition.x - 0.1f;
            
        // Wrap around the upper and lower side of the screen
        if (viewportPosition.y > 1f)
            newPosition.y = -newPosition.y + 0.1f;
        else if (viewportPosition.y < 0f)
            newPosition.y = -newPosition.y - 0.1f;

        transform.position = newPosition;
    }
}
