using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Rigidbody2D rb;
    public float bulletSpeed;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

// Adding velocity to bullet
    public void shoot(Vector2 direction)
    {

        rb.velocity = direction.normalized * bulletSpeed;
    }

// separating bullet from other game objects
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Background")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Asteriod")
        {
            Destroy(gameObject);
        }
    }

}
