using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraFollow : MonoBehaviour
{
    public Transform targetObject;  // the object that the camera will follow

    public Vector3 cameraOffset;  // the offset from the target object's position that the camera will maintain
    public float smoothFactor = 0.5f;  // the smoothing factor to apply to camera movement
    public bool lookAtTarget = false;  // whether or not the camera should always face the target object

    private float startingYPosition;  // the starting y position of the camera

    // event to be raised when the player dies
    public delegate void PlayerDeathEvent();
    public static event PlayerDeathEvent OnPlayerDeath;

    // Start is called before the first frame update
    void Start()
    {
        // Finds object with player tag
        targetObject = GameObject.FindWithTag("Player")?.transform;

        if (targetObject != null)
        {
            // set the camera offset to the distance between the camera and the target object
            cameraOffset = transform.position - targetObject.transform.position;

            // Store the camera's starting y position
            startingYPosition = transform.position.y;
        }
    }

    void LateUpdate()
    {
        if (targetObject != null)
        {
            // calculate the new position of the camera based on the target object's position and the camera offset
            Vector3 newPosition = new Vector3(transform.position.x, startingYPosition, targetObject.transform.position.z + cameraOffset.z);

            // smoothly move the camera to its new position
            transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

            if (lookAtTarget)
            {
                // make the camera always face the target object
                transform.LookAt(targetObject);
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the player's death event
        OnPlayerDeath = null;
    }

    private void Update()
    {
        // Check if the player has died
        if (targetObject == null)
        {
            // Raise the OnPlayerDeath event
            OnPlayerDeath?.Invoke();
        }
    }
}
