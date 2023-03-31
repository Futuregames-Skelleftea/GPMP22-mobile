using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject _ballPrefab;
    [SerializeField] Rigidbody2D _pivot;

    Rigidbody2D _ball;
    SpringJoint2D _spring;

    bool _dragging = false;
    bool _launched = false;
    Vector2 _threshold;

    void Start()
    {
        // Orientates the screen horizontally
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Respawn();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        MoveBall();

        // Runs the detach check after the player lets go of the ball
        if (_launched)
            Snap();
    }

    private void MoveBall()
    {
        if (_ball == null) return;

        // Ball will not be affected if the player is not touching the screen
        if (Touch.activeTouches.Count == 0)
        {
            _ball.isKinematic = false;

            if (_dragging)
            {
                _dragging = false;
                Shoot();
            }

            return;
        }

        // If there are multiple touches on screen, the ball
        // will be positioned in between all of the touches
        Vector2 touchPositions = new Vector2();

        foreach (Touch touch in Touch.activeTouches)
        {
            touchPositions += touch.screenPosition;
        }

        touchPositions /= Touch.activeTouches.Count;

        _dragging = true;
        _ball.isKinematic = true;
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touchPositions);

        _ball.position = worldPoint; 
    }

    // Sets the threshold for when the ball should detach
    // the spring joint when it's launched
    private void Shoot()
    {
        _threshold = _pivot.position - Vector2.Lerp(_ball.position, _pivot.position, 0.9f);
        _launched = true;
    }

    // Detaches the ball when it's close enough to the pivot point and adds 
    // gravity to the ball after doing so
    private void Snap()
    {
        if ((_ball.position - _pivot.position).magnitude < _threshold.magnitude)
        {
            _spring.enabled = false;
            _spring = null;
            _launched = false;
            _ball.gravityScale = 3f;
            Invoke(nameof(Respawn), 2f);
            Destroy(_ball.gameObject, 2f);
        }
    }

    // Respawns the ball and attaches the necessary
    // components to it
    private void Respawn()
    {
        GameObject newBall = Instantiate(_ballPrefab, _pivot.position, Quaternion.identity);

        _ball = newBall.GetComponent<Rigidbody2D>();
        _spring = newBall.GetComponent<SpringJoint2D>();
        _spring.connectedBody = _pivot;
    }
}
