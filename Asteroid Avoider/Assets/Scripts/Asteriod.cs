using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteriod : MonoBehaviour
{
    public Sprite[] sprites;
    private Rigidbody2D rb;
    PolygonCollider2D pc;
    public float asteriodSpeed;
    SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PolygonCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void kick(float astMass, Vector2 direction)
    {

        sr.sprite = this.sprites[Random.Range(0, this.sprites.Length)];

        List<Vector2> path = new List<Vector2>();
        sr.sprite.GetPhysicsShape(0, path);
        pc.SetPath(0, path.ToArray());


        rb.mass = astMass;
        float width = Random.Range(0.75f, 1.33f);
        float height = 1 / width;
        transform.localScale = new Vector2(width, height) * astMass;


        //Move and Rotate Asteriod
        rb.velocity = direction.normalized * asteriodSpeed;
        rb.AddTorque(Random.Range(-4f, 4f));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Background")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {

            //Check if it is large enough to be split into two
            if (rb.mass > 0.7f)
            {
                split();
                split();
            }

            Destroy(gameObject);
        }
    }

    void split()
    {

        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;


        Asteriod small = Instantiate(this, position, this.transform.rotation);
        Vector2 direction = Random.insideUnitCircle;
        float mass = rb.mass / 2;
        small.kick(mass, direction);
    }
}
