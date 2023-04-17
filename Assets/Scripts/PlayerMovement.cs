using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _breakForce;
    [SerializeField] float _maxVelocity;

    float _currMoveSpeed;

    Camera _mainCam;
    Rigidbody _rb;
    Vector3 _direction;
    Vector3 _inputDirection;
    Vector3 _moveDirection;

    void Start()
    {
        _mainCam = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _currMoveSpeed = Mathf.Max(0, _currMoveSpeed - (_breakForce * Time.deltaTime));
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            _currMoveSpeed = _moveSpeed;
            Vector3 currentPos = this.transform.position;
            //get touch pos and convert from screen to world space
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPos = _mainCam.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, _mainCam.transform.position.z));
            _moveDirection = Vector3.MoveTowards(currentPos, worldPos, _moveSpeed);
            _moveDirection.z = 0;
            _moveDirection.Normalize();
        }
        else
        {
            _moveDirection = Vector3.zero;
        }
        Debug.Log(_moveDirection);

    }
    private void FixedUpdate()
    {
        if(_moveDirection == Vector3.zero) return;
        _rb.AddForce(_moveDirection * _currMoveSpeed, ForceMode.Force);

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);
    }
}
