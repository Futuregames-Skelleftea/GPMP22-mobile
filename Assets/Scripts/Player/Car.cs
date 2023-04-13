using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Car : MonoBehaviour
{

    #region MovementVariables
    /// <summary>
    /// Character Rigidbody for Physics and movement
    /// </summary>
    private Rigidbody _rigidbody;
    /// <summary>
    /// Current forward momentum m/s
    /// </summary>
    [SerializeField]
    private float _forwardSpeed = 0f;

    /// <summary>
    /// Maximum forward momentum m/s
    /// </summary>
    [SerializeField]
    private float _maxForwardSpeed = 60f;

    /// <summary>
    /// Acceleration m/s
    /// </summary>
    [SerializeField]
    private float _acceleration = 5f;

    /// <summary>
    /// Rotational speed
    /// </summary>
    [SerializeField]
    private float _turnSpeed = 90f;
    #endregion

    #region inputVariables
    /// <summary>
    /// Display Size in pixels
    /// </summary>
    private Vector2 _displaySize;

    /// <summary>
    /// Width of the display in pixels
    /// </summary>
    private float _displayWidthHalf => _displaySize.x / 2f;

    /// <summary>
    /// Variable used to calculate the final touchPosition
    /// </summary>
    private Vector2 _allTouchePositionsCalculationVaraible = Vector3.zero;

    /// <summary>
    /// Median of every touch position | Screen Space
    /// </summary>
    private Vector2 _touchPosition = Vector3.zero;

    /// <summary>
    /// Current Distance between last touch on screen to middle of screen on the horizontal axis
    /// </summary>
    private float _currentRotation => ConvertViewPositionToRotational(_touchPosition);
    #endregion

    private Coroutine _scoreAddingPerSecond;

    #region UnityMethods
    private void Awake()
    {
        // Get Display Width and Height
        _displaySize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

        if (!_rigidbody)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        GameManager.Instance.Score = 0;
        //_scoreAddingPerSecond = StartCoroutine(nameof(AddScoreEverySecond));
    }

    private void OnEnable()
    {
        // Start tracking multiple touch inputs
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        // stop tracking multiple touch inputs
        EnhancedTouchSupport.Disable();
    }

    private void OnDestroy()
    {
        // stop Adding Score if score is adding
        if (_scoreAddingPerSecond != null) StopCoroutine(_scoreAddingPerSecond);    
    }

    private void OnTriggerEnter(Collider other)
    {
        // if colliding with object that has Obsticle script attached
        if (other.gameObject.TryGetComponent<Obsticle>(out Obsticle obs))
        {
            // Tell GameManager the Car has Crashed
            GameManager.CarCrashed();
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Checks for touches
        if (Touch.activeTouches.Count != 0)
        {
            // Get all inputs
            GetInputs();

            // Move car
            Move();

            return;
        }

        // Reset Touch position
        if (_touchPosition.x != _displayWidthHalf)
            _touchPosition.x = _displayWidthHalf;

        // Move car
        Move();
    }
    #endregion

    /// <summary>
    /// Gets all inputs | touchPositions and rotational Strength
    /// </summary>
    private void GetInputs()
    {

        // Calculate final touch position
        _allTouchePositionsCalculationVaraible = Vector2.zero;
        foreach (Touch touch in Touch.activeTouches)
        {
            _allTouchePositionsCalculationVaraible += touch.screenPosition;
        }
        // Insure there are no divided by zero and useless calculations
        if (Touch.activeTouches.Count != 1 && Touch.activeTouches.Count != 0)
            _allTouchePositionsCalculationVaraible /= Touch.activeTouches.Count;

        // Set final touch position
        _touchPosition = _allTouchePositionsCalculationVaraible;
    }

    /// <summary>
    /// Adds 1 Score every 1 second to GameManager
    /// </summary>
    /// <returns></returns>
    private IEnumerator AddScoreEverySecond()
    {
        // until corutine stops loop
        while (true)
        {
            // wait 1 second
            yield return new WaitForSeconds(1f);
            // Add 1 Score to GameManager
            GameManager.Instance.Score += 1;
        }
    }

    #region MovementMethods
    /// <summary>
    /// Uses view position to get back a float between (-1 <-> 1) depending on how close to center of screen from width
    /// </summary>
    /// <param name="ViewPosition">Mouse/Touch position</param>
    /// <returns></returns>
    private float ConvertViewPositionToRotational(Vector2 ViewPosition)
    {
        float returnValue = 0;

        // Safe gaurd from divided by 0
        if (_displayWidthHalf == 0) 
        {
            Debug.LogError("Screen Width is 0 somehow in pixels");
            return 0;
        }

        // calculate distance from center horizontal (-1 <-> 1) 
        returnValue = (ViewPosition.x / _displayWidthHalf) - 1;

        // return result
        return returnValue;
    }

    private void Move()
    {
        // Add more force to forward Movement variable
        _forwardSpeed = Mathf.Min(_forwardSpeed + _acceleration * Time.deltaTime, _maxForwardSpeed);

        // Rotational Movement
        if (_currentRotation != 0)
        {
            Quaternion forwardDirection = transform.rotation * Quaternion.Euler(0,_currentRotation * _turnSpeed * _forwardSpeed * Time.deltaTime,0);

            transform.rotation = forwardDirection;
        }

        // Forward Movement 
        transform.position += transform.forward * _forwardSpeed * Time.deltaTime;
    }
    #endregion

}
