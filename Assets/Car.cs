using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
	public float Speed;
	[SerializeField] float speedGainOverTime;
	[SerializeField] float steeringSpeed;

	float steeringValue;
	float screenMidValue;

	public UnityEvent gameOverEvent;
	private void Start()
	{
		screenMidValue = Screen.width / 2;
	}
	void Update()
	{
		CheckPlayerTouch();
		AccelerateCar();
		MoveCar();
	}

	private void CheckPlayerTouch()
	{
		if (Touchscreen.current.primaryTouch.press.IsPressed())
		{
			float inputXvalue = Touchscreen.current.primaryTouch.position.ReadValue().x;
			if (inputXvalue < screenMidValue)
			{
				steeringValue = -(screenMidValue - inputXvalue);
			}
			else
			{
				steeringValue = (inputXvalue - screenMidValue);
			}
		}
		else
		{
			steeringValue = 0;
		}
	}

	private void AccelerateCar()
	{
		Speed += speedGainOverTime * Time.deltaTime;
	}

	private void MoveCar()
	{
		transform.Translate(Vector3.forward * Speed * Time.deltaTime);
		transform.Rotate(0, steeringValue * steeringSpeed * Time.deltaTime, 0);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Obstacle"))
		{
			gameOverEvent.Invoke();
			SceneManager.LoadScene(0);
		}
	}
}
