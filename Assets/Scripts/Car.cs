using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    // The base speed of the car.
    [SerializeField] private float speed = 10f;

    // The rate at which the car's speed increases over time.
    [SerializeField] private float speedGain = 0.2f;

    // The speed at which the car turns when steering.
    [SerializeField] private float turnSpeed = 200f;

    // The current steering input value.
    private int steerValue;

    // Update is called once per frame.
    void Update()
    {
        // Increase the speed of the car gradually over time.
        speed += speedGain * Time.deltaTime;

        // Rotate the car around the y-axis based on the steering input value.
        transform.Rotate(0, steerValue * turnSpeed * Time.deltaTime, 0);

        // Move the car forward along its local forward direction with the current speed.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Called when the car collides with a trigger collider.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "Obstacle" tag.
        if (other.CompareTag("Obstacle"))
        {
            // Reload the first scene, usually used for restarting the game.
            SceneManager.LoadScene(0);
        }
    }

    // A method to adjust the steering input value of the car.
    public void Steer(int value)
    {
        steerValue = value;
    }
}
