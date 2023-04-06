using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedGainOvertime = 0.2f;
    [SerializeField] private float turnSpeed = 200f;

    // steering input value
    private int steerValue;

    void Update()
    {
        // increase speed over time
        speed += speedGainOvertime * Time.deltaTime;
        // rotate the car based on steering input
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);
        // move the car forward based on its speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // called when the car triggers a collider
    public void OnTriggerEnter(Collider other)
    {
        // check if the collider has the "Obstacles" tag
        if (other.CompareTag("Obstacles"))
        {
            // load the "Main Menu" scene
            SceneManager.LoadScene("Main Menu");
        }
    }

    // called when steering input is received
    public void Steer(int Value)
    {
        // set the steering input value
        steerValue = Value;
    }
}
