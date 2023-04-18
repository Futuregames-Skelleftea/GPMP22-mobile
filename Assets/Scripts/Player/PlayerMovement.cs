using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables

    #region CameraVariable
    private Camera _mainCamera;
    public Camera MainCamera 
    {
        get 
        {
            if (!_mainCamera) _mainCamera = Camera.main;
            return _mainCamera;
        }
    }
    #endregion

    #region InputVariables

    private Vector2 _touchPosition = Vector2.zero;
    private Vector2 _allTouchesPosition = Vector2.zero;
    /// <summary>
    /// returns true if there are any touches on the screen
    /// </summary>
    public bool Touching => Touch.activeTouches.Count > 0;
    
    #endregion

    #region MovementVariables

    [SerializeField, Range(0,300f)]
    private float _movementSpeed = 5f;
    [SerializeField, Range(0,300f)]
    private float _maxMovementSpeed = 10f;
    [SerializeField]
    private ForceMode _forceMode = ForceMode.Acceleration;

    [SerializeField]
    private Rigidbody _rigidbody;
    private Rigidbody Rigidbody 
    {
        get
        {
            if (!_rigidbody)
            {
                if (!TryGetComponent(out _rigidbody))
                {
                    Debug.LogError("Did not find a rigidbody:" + gameObject.name);
                    // Create a rigidbody
                    _rigidbody = gameObject.AddComponent<Rigidbody>();
                    // Freeze Z axis
                    _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
                }
            }

            return _rigidbody;
        }
    }

    private Vector3 MovementDirection => (ViewToWorldPosition(_touchPosition) - _rigidbody.position).normalized;
    #endregion

    #endregion

    #region Methods

    #region UNITY_METHODS
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


    private void Update()
    {
        if (Touching)
        {
            GetInputs();
        }
        else if (_touchPosition != Vector2.zero) 
        {
            // Reset input Vector
            _touchPosition = Vector2.zero;
        }
        KeepPlayerOnScreen();

    }

    private void FixedUpdate()
    {
        if (_touchPosition == Vector2.zero) 
        {
            deaccelerat(Time.fixedDeltaTime);
        }
        else
        {
            Accelerate(Time.fixedDeltaTime);
        }

        RotateTowardsVelocity(Time.fixedDeltaTime);

    }

    #endregion

    #region InputMethods
    /// <summary>
    /// Gets the mouse/touch Position on screen
    /// </summary>
    private void GetInputs()
    {

        // Calculate final touch position
        _allTouchesPosition = Vector2.zero;
        foreach (Touch touch in Touch.activeTouches)
        {
            _allTouchesPosition += touch.screenPosition;
        }
        // insure there are no divided by zero and useless calculations
        if (Touch.activeTouches.Count != 1 && Touch.activeTouches.Count != 0)
            _allTouchesPosition /= Touch.activeTouches.Count;

        // Set final touch position
        _touchPosition = _allTouchesPosition;
    }
    #endregion

    #region MovementMethods

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = Rigidbody.position;
        Vector2 screenPosition = MainCamera.WorldToViewportPoint(Rigidbody.position);
        if (screenPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if (screenPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }
        if (screenPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if (screenPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }
        Rigidbody.position = newPosition;
    }

    private void RotateTowardsVelocity(float deltaTime)
    {
        if (Rigidbody.velocity.magnitude > 0.5f)
        Rigidbody.rotation =(Quaternion.LookRotation(Rigidbody.velocity, Vector3.back));
    }

    /// <summary>
    /// Adds Force to rigidbody
    /// </summary>
    /// <param name="deltaTime">Time.deltaTime or Time.FixedDeltaTime</param>
    private void Accelerate(float deltaTime)
    {
        // Add Force
        Rigidbody.AddForce(MovementDirection * _movementSpeed * 100f * deltaTime, _forceMode);
        
        // Clamp Max Velocity
        Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, _maxMovementSpeed);
    }

    /// <summary>
    /// Negates current force to rigidbody
    /// </summary>
    /// <param name="deltaTime">Time.deltaTime or Time.FixedDeltaTime</param>
    private void deaccelerat(float deltaTime)
    {
        // Add Force Negative to velocity
        Rigidbody.AddForce(-Rigidbody.velocity.normalized * _movementSpeed * 100f * deltaTime, _forceMode);

        // Clamp Max Velocity
        Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, _maxMovementSpeed);
    }

    /// <summary>
    /// Will take your view position and convert it to world position (- Camera Z position)
    /// </summary>
    /// <param name="ScreenPosition">View Position</param>
    /// <returns>World Position</returns>
    private Vector3 ViewToWorldPosition(Vector2 ScreenPosition)
    {
        //return (Convert Position) - (cameras position)
        return MainCamera.ScreenToWorldPoint(ScreenPosition) - MainCamera.transform.position.z * Vector3.forward;
    }

    #endregion

    #endregion
}
