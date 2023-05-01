using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField] private List<GameObject> asteroidPrefabs = new();
	[SerializeField] private float secondsBetweenAsteroids;
	[SerializeField] private Vector2 asteroidInitialForceRange;
	[SerializeField] private float asteroidSecondsAlive;

	public enum ScreenSides { Left, Right, Up, Down };
	private ScreenSides screenSides = ScreenSides.Left;

	private void Awake()
	{
		StartCoroutine(AsteroidSpawnChain());
	}

	private void SpawnAsteroid()
	{
		float sidePosition = Random.value;
		Vector2 spawnPoint = Vector2.zero;
		Vector2 movementDirection = Vector2.zero;

		screenSides = (ScreenSides)Random.Range(0, 4);

		switch (screenSides)
		{
			case ScreenSides.Left:
				spawnPoint.x = 0;
				spawnPoint.y = sidePosition;
				movementDirection = new Vector2(1, Random.Range(-1f, 1f));
				break;
			case ScreenSides.Right:
				spawnPoint.x = 1;
				spawnPoint.y = sidePosition;
				movementDirection = new Vector2(-1, Random.Range(-1f, 1f));
				break;
			case ScreenSides.Up:
				spawnPoint.x = sidePosition;
				spawnPoint.y = 1;
				movementDirection = new Vector2(Random.Range(-1f, 1f), -1);
				break;
			case ScreenSides.Down:
				spawnPoint.x = sidePosition;
				spawnPoint.y = 0;
				movementDirection = new Vector2(Random.Range(-1f, 1f), 1);
				break;
			default:
				break;
		}

		GameObject spawnedAsteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)],
												Camera.main.ViewportToWorldPoint(spawnPoint),
												Quaternion.Euler(0, 0, Random.Range(0, 360)));

		spawnedAsteroid.transform.position = new Vector3(spawnedAsteroid.transform.position.x, spawnedAsteroid.transform.position.y, 0);

		if (spawnedAsteroid.TryGetComponent<Rigidbody>(out Rigidbody rb))
		{
			rb.velocity = movementDirection.normalized * Random.Range(asteroidInitialForceRange.x, asteroidInitialForceRange.y);
		}

		Destroy(spawnedAsteroid, asteroidSecondsAlive);
	}

	private IEnumerator AsteroidSpawnChain()
	{
		SpawnAsteroid();
		float progress = 0;
		while (progress < secondsBetweenAsteroids)
		{
			progress += Time.deltaTime;
			yield return null;
		}
		StartCoroutine(AsteroidSpawnChain());
	}
}
