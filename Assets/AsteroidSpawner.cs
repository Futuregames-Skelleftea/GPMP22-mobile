using System.Collections;
using UnityEditor;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
	[SerializeField] private GameObject asteroidPrefabs;
	[SerializeField] private float secondsBetweenAsteroids;
	[SerializeField] private Vector2 asteroidInitialForceRange;

	public enum ScreenSides { Left, Right, Up, Down };

	private void Start()
	{
		StartCoroutine(AsteroidSpawnChain());
	}

	private void SpawnAsteroid()
	{
		asteroidInitialForceRange = new Vector2(0, Random.Range(0, 4));
		int side = Random.Range(0, 4);
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
