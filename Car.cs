using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    //moving forward variables
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    //steering variables
    [SerializeField] private float turnSpeed;
    private int steerValue;



    void Update()
    {
        //changes the speed based on acceleration.
        speed += acceleration * Time.deltaTime;

        //makes the car go forward at the speed of "speed"
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //rotates car in at the speed of "turnspeed" in the direction of "steervalue"
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(0);
        }
    }

    //method for steering the car.
    public void steer(int value)
    {
        steerValue = value;
    }
}
