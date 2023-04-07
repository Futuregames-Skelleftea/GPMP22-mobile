using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
	[SerializeField] GameObject ballPrefab;
	[SerializeField] Rigidbody2D currentBallRigidBody2D;
	[SerializeField] SpringJoint2D currentSpringJoint2D;
	[SerializeField] GameObject launcherGameObject;
	bool isDragging;

	private void Start()
	{
		StartCoroutine(CreateNewBall());
	}

	void Update()
	{
		if (currentBallRigidBody2D != null)
		{
			CheckScreenTouch();
		}
		else if (currentSpringJoint2D != null)
		{
			CheckLaunchedBallDistanceToLauncher();	
		}
	}

	private void CheckLaunchedBallDistanceToLauncher()
	{
		if (Vector3.Distance(currentSpringJoint2D.gameObject.transform.position, launcherGameObject.transform.position) < 1)
		{
			currentSpringJoint2D.enabled = false;
			currentSpringJoint2D = null;
			StartCoroutine(CreateNewBall());
		}
	}

	private void CheckScreenTouch()
	{
		if (Touchscreen.current.primaryTouch.press.isPressed)
		{
			Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
			currentBallRigidBody2D.position = worldPosition;
			currentBallRigidBody2D.isKinematic = true;
			isDragging = true;
		}
		else
		{
			if (isDragging)
			{
				isDragging = false;
				LaunchBall();
			}
		}
	}

	private IEnumerator CreateNewBall()
	{
		yield return new WaitForSeconds(1);
		var clone = Instantiate(ballPrefab, launcherGameObject.transform.position, Quaternion.identity);
		currentBallRigidBody2D = clone.GetComponent<Rigidbody2D>();
		currentSpringJoint2D = clone.GetComponent<SpringJoint2D>();
		currentSpringJoint2D.connectedBody = launcherGameObject.GetComponent<Rigidbody2D>();
	}

	private void LaunchBall()
	{
		currentBallRigidBody2D.isKinematic = false;
		currentBallRigidBody2D = null;
	}
}
