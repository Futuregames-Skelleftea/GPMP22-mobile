using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedGainPerSecond = 0.2f;
    [SerializeField] private float turnSpeed = 200f;

    // The current steering value of the car
    private int steerValue;

    // Update is called once per frame
    void Update()
    {
        // Increase the car's speed based on the speed gain per second and time elapsed
        speed += speedGainPerSecond * Time.deltaTime;

        // Rotate the car based on the steering value and turn speed
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);

        // Move the car forward based on the speed and time elapsed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other collider is tagged as "Obstacle"
        if (other.CompareTag("Obstacle"))
        {
            // Load the main menu scene
            SceneManager.LoadScene("Scene_MainMenu");
        }
    }

    // Sets the steering value of the car based on the input
    public void Steer (int value)
    {
        steerValue = value;
    }

}
