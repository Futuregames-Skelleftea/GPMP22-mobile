

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Car : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _speedGainPerSecond = 0.2f;
    [SerializeField] private float _turnRate = 200f;
    [SerializeField] private float _breakForce;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxTurnRate;
    
    private float _steerValue;

    
    private Vector2 _midScreenPoint;



    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        _midScreenPoint.x = Screen.width / 2;
        _midScreenPoint.y = Screen.height / 2;
    }

    void Update()
    {

        if (Touch.activeTouches.Count < 1)
        {
            _moveSpeed = Mathf.Max(0, _moveSpeed -= Time.deltaTime * _breakForce);
            return;
        }

        //get current desired touchposition from aggregate of all active touches
        Vector2 touchPoint = new();
        foreach (Touch touch in Touch.activeTouches)
        {
            touchPoint += touch.screenPosition;
        }
        touchPoint /= Touch.activeTouches.Count;

        //apply steering
        Steer(touchPoint);

        //increase speed and apply it
        _moveSpeed = Mathf.Min(_moveSpeed + _speedGainPerSecond * Time.deltaTime, _maxSpeed);
        transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);


        //TODO: make this if() look for a range of numbers rather than just 0
        if (_steerValue == 0) return;


        transform.Rotate(0f, _steerValue * _turnRate * Time.deltaTime, 0f, Space.Self);
    }

    public void Steer(Vector2 touchPosition)
    {



        float value = 0;
        //use vector maths to make _steervalue stronger the further away from _midscreenpoint.x touchposition.x is
        if (touchPosition.x < _midScreenPoint.x)
        {

            value = Mathf.Max(touchPosition.x - _midScreenPoint.x, -_maxTurnRate);//negative numbers are to the left of midscreenpoint
        }
        else if(touchPosition.x > _midScreenPoint.x)
        {
            value = Mathf.Min(touchPosition.x - _midScreenPoint.x, _maxTurnRate);
        }
        _steerValue = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
        }
    }
}
