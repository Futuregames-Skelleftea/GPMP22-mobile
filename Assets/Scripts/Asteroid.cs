using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Method is called when the asteroid collides with another collider
    private void OnTriggerEnter(Collider other)
    {
        //Checks if the other collider has a PlayerHealth component attached
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        //if not, return
        if (playerHealth == null)
        {
            return;
        }
        //If we have, call our chrash method
        playerHealth.Crash();
    }

    // When the asteroid is no longer visible on the screen it gets destroyed
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
