using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    // Serialize fields for car speed and turning
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedGain = 0.2f;
    [SerializeField] private float turnSpeed = 200f;
    private int steerValue;
    void Update()
    {
        // Increase speed over time
        speed += speedGain * Time.deltaTime;
        
        // Rotate the car based on steering
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);

        // Move the car forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for collision with obstacle and restart the scene
        if (other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(0);
        }
    }

    // Method to set the steering value
    public void Steer(int value)
    {
        steerValue = value;
    }
}
