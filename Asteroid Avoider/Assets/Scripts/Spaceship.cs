using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{

    private Rigidbody2D rb;
    bool moveForce = false;
    public float moveSpeed = 10.0f;
    private float moveDirection = 0f;
    public float moveAmount = 5f;

    public Bullet bullet;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {

        


        //Apply force lby pressing W key

        moveForce = Input.GetKey(KeyCode.W);

        //To rotate Spaceship by 180

        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.RotateAround(transform.position, new Vector3(0, 0, 1), 180f);
        }

        //Set Left and Right Keys

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {moveDirection = -1f;
        }
        else
        {
            moveDirection = 0f;
        }

        //Shoot Bullet by pressing space key

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Bullet myBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            myBullet.shoot(transform.up);

        }

    }

    // Checking Space Boundaries

    private void checkBoundaries()
    {

        float x = transform.position.x;
        float y = transform.position.y;

        if (x> 8f)
        {
            x = x - 16f;
        }

        if (x < -8f)
        {
            x = x + 16f;
        }

        if (y > 4.5f)
        {
            y= y - 9f;
        }

        if (y < -4.5f)
        {
            y = y + 9f;
        }

        transform.position = new Vector2(x, y);
    }

    private void FixedUpdate()
    {
        if (moveForce)
        {
            rb.AddForce(transform.up * moveSpeed);
        }

        if (moveDirection != 0)
        {
            rb.AddTorque(moveDirection * moveAmount);
        }
    }
}
