using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    //Serialized fields that you will see Unity Editor
    [SerializeField] private float speed = 10f; 
    [SerializeField] private float speedGainPerSecond = 0.2f;
    [SerializeField] private float turnSpeed = 200f;

    private int steerValue;

    void Update()
    {
        //Increase the car's speed over time
        speed += speedGainPerSecond * Time.deltaTime;
        //Rotate the car the current value
        transform.Rotate(0,steerValue * turnSpeed * Time.deltaTime,0);
        //Move the car forward on its current speed
        transform.Translate(Vector3.forward * speed *  Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) 
    {
        //If you collied to an obstacle, load MainMenu scene
        if(other.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(0);
        }
    }

    //Called by the Input System to update the steervalue
    public void Steer(int Value)
    {
        steerValue = Value;
    }
}
