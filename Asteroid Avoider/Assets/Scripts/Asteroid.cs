using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Get the PlayerHealth component of the other object
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        // If the other object doesn't have a PlayerHealth component, exit the function
        if (playerHealth == null) { return; }

        // Call the Crash() method on the PlayerHealth component
        playerHealth.Crash();

    }

    // Called when the object becomes invisible
    private void OnBecameInvisible()
    {
        // Destroy this object
        Destroy(gameObject);
    }

}
