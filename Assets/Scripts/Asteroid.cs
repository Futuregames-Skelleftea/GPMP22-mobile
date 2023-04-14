using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)  // When entering another collider.
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();  // if other collider has the PlayerHealth component.

        if (playerHealth == null) { return; }  // If not, don't run crash.

        playerHealth.Crash();  // Run crash.

    }

    private void OnBecameInvisible()  // When objecct is off screen.
    {
        Destroy(gameObject);  // Destroy gameobject.
    }

}
