using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedGainPerSecond = 0.2f;
    [SerializeField] private float turnSpeed = 200f;

    private int steerValue;

    void Update()
    {
        speed += speedGainPerSecond * Time.deltaTime;  // Each frame increase speed exponentially according to speedGainPerSecond

        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f); // Rotate the car in Y axis according to steervalue and turnspeed


        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Apply speed to car each frame.
    }

    private void OnTriggerEnter(Collider other) // When colliding with something that has the tag Obstacle, Load Scene 0.
    {
        if(other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Steer(int value)
    {
        steerValue = value; // Sets the steervalue when called outside of the script.
    }
}
