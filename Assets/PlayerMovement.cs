using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float forceMagnitude;
	[SerializeField] float maxVelocity;

	Camera mainCamera;
	Rigidbody rigidBody;
	Vector3 movementDirection;

	private void Start()
	{
		mainCamera = Camera.main;
		rigidBody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		EvaluatePlayerInput();
		KeepPlayerOnScreen();
	}

	private void FixedUpdate()
	{
		MovePlayer();
	}

	private void EvaluatePlayerInput()
	{
		if (Touchscreen.current.primaryTouch.press.IsPressed())
		{
			Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
			movementDirection = transform.position - worldPosition;
			movementDirection.z = 0f;
			movementDirection.Normalize();
		}
		else
		{
			movementDirection = Vector3.zero;
		}
	}

	private void MovePlayer()
	{
		if (movementDirection != Vector3.zero)
		{
			rigidBody.AddForce(movementDirection * forceMagnitude, ForceMode.Force);
			rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxVelocity);
		}
	}

	private void KeepPlayerOnScreen()
	{
		Vector3 newPosition = transform.position;
		Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
		if (viewportPosition.x > 1)
		{
			newPosition.x = -newPosition.x + 0.1f;
		}
		else if (viewportPosition.x < 0)
		{
			newPosition.x = -newPosition.x - 0.1f;
		}
		if (viewportPosition.y > 1)
		{
			newPosition.y = -newPosition.y + 0.1f;
		}
		else if (viewportPosition.y < 0)
		{
			newPosition.y = -newPosition.y - 0.1f;
		}
		transform.position = newPosition;
	}
}
