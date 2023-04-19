using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bounceForce;

    bool gameStarted;
    


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }


    void Start()
    {
        
    }


    void Update()
    {
        if (!gameStarted) // Checking if game has not started then set Startbounce = null
        {
            if (Input.anyKeyDown)
            {
                StarBounce();
                gameStarted = true;
                gameManager.instance.Gamestart();
            }
        }
    }

    void StarBounce()
    {

        Vector2 randomDirection = new Vector2(Random.Range(-1, 1), 1);
        rb.AddForce(randomDirection * bounceForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "FallCheck")
        {
            gameManager.instance.RestartGame();
        }

        else if (other.gameObject.tag == "peddal")
        {
            gameManager.instance.ScoreUp(); // Adding score to the screen
        }
    }
}
