using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //If asteroids crash into gameobject with component "Health" call for method "Crash" from the "Health" script
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        if (health == null) { return; }

        health.Crash();
    }

    //Destroy when leaving the screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

