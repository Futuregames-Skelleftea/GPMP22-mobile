using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    //camera reference for screentoworldpoint functionality
    private Camera _cam;

    //serialized instance references
    [SerializeField] private Rigidbody2D _currBallRB;
    [SerializeField] private SpringJoint2D _currSpringJoint;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Rigidbody2D _pivot;


    [SerializeField] private float _respawnDelay;

    //the delay between when you release the ball and when the springjoint2d releases the ball
    [SerializeField] private float _detachDelay;

    private bool _isDragging;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (!_currBallRB) return; //sanity check

        if (Touch.activeTouches.Count < 1)
        {
            if (_isDragging)
            {
                Destroy(_currBallRB.gameObject, _respawnDelay * 5f);
                LaunchBall();
            }

            _isDragging = false;

            return;
        }
        _isDragging = true;

        _currBallRB.isKinematic = true;

        Vector2 touchPoint = new();

        foreach (Touch touch in Touch.activeTouches)
        {
            touchPoint += touch.screenPosition;
        }

        touchPoint /= Touch.activeTouches.Count;

        Vector3 worldPoint = _cam.ScreenToWorldPoint(touchPoint);

        _currBallRB.position = worldPoint;


    }

    private void LaunchBall()
    {
        _currBallRB.isKinematic = false;
        _currBallRB = null;

        Invoke(nameof(DetachBall), _detachDelay);
    }

    private void DetachBall()
    {
        _currSpringJoint.enabled = false;

        _currSpringJoint = null;
        Invoke(nameof(SpawnBall), _respawnDelay);
    }

    private void SpawnBall()
    {
        //declare var newball as a new instance of the ball prefab
        var newBall = Instantiate(_ballPrefab, _pivot.position, Quaternion.identity);

        //get necessary components
        _currBallRB = newBall.GetComponent<Rigidbody2D>();
        _currSpringJoint = newBall.GetComponent<SpringJoint2D>();

        //connect springjoint to Rigidbody2D _pivot
        _currSpringJoint.connectedBody = _pivot;
    }

}
