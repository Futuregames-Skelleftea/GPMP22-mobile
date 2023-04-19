using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paddleController : MonoBehaviour
{
    //Setting our Variables
    private Rigidbody2D rb;
    public float moveSpeed;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        
    }

  
    void Update()
    {
      
    }

    // Setting the mouse Position on the screen
    void TouchMove()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Convert the screen point to world point and storing it in touch position

            if (touchPos.x < 0)
            {
                rb.velocity = Vector2.left * moveSpeed; // clicking left screen
            }
            else if (touchPos.x > 0)
            {
                rb.velocity = Vector2.right * moveSpeed; // clicking right screen
            }

        }

        else
        {
            rb.velocity = Vector2.zero; // Not clicking anywhere on the scree
        }
 

    }

    private void FixedUpdate()
    {
        TouchMove();
    }
}
