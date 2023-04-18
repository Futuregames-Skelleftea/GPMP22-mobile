using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    private float _rotationalSpeed = 0;
    public void Init(Vector2 spawnDirection, float force)
    {

        _rotationalSpeed = Random.value * force * 50f;
        gameObject.SetActive(true);
        _rigidbody.velocity = spawnDirection.normalized * force;
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.rotation *= Quaternion.Euler(0, 0, _rotationalSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.Crash();
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
