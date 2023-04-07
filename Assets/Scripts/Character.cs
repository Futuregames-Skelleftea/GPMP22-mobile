using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.SceneManagement;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField] float _acceleration;
    [SerializeField] float _speed;
    [SerializeField] Animator _animator;

    float _steeringValue;
    bool _running = false;

    // Update is called once per frame
    void Update()
    {
        Run();

        // Changes the character from walking to running animation when 
        // a certain speed is reached
        if (!_running && _speed > 25f)
        {
            _running = true;
            _animator.SetBool("Running", true);
        }
    }

    // Accelerates the character to go faster over time
    private void Run()
    {
        _speed += _acceleration * Time.deltaTime;

        transform.Rotate(transform.up, _steeringValue * Time.deltaTime);

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    // Used by buttons on the touch UI
    public void Steer(float steeringValue)
    {
        _steeringValue = steeringValue;
    }

    // Goes straight to main menu as soon as the player crashes
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            SceneManager.LoadScene(0);
    }
}
