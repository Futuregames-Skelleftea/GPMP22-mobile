using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
	[SerializeField] float speed;
	[SerializeField] float speedGainOverTime;
	[SerializeField] float steeringSpeed;

	float steeringValue;
	float screenMidValue;

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
		speed += speedGainOverTime * Time.deltaTime;
	}

	private void MoveCar()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
		transform.Rotate(0, steeringValue * steeringSpeed * Time.deltaTime, 0);
	}
}
