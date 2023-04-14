using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private float secondsBetweenAsteroids = 1.5f;
    [SerializeField] private Vector2 forceRange;

    private Camera mainCamera;
    private float timer;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        timer -= Time.deltaTime;  // Counts down the timer.

        if (timer <= 0)  // If timer is less than or 0.
        {
            SpawnAsteroid();

            timer += secondsBetweenAsteroids;  // Adds X seconds to the timer.
        }
    }

    private void SpawnAsteroid()
    {
        int side = Random.Range(0, 4);  // Randomize the side from wich the asteroid will come from.

        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        switch (side)
        {
            case 0:  // If left side then change the random direction field to only face the right.
               // Left
               spawnPoint.x = 0;
               spawnPoint.y = Random.value;
               direction = new Vector2(1f, Random.Range(-1f, 1f));
               break;
            
            case 1:  // If Right side then change the random direction field to only face the left.
               // Right
               spawnPoint.x = 1;
               spawnPoint.y = Random.value;
               direction = new Vector2(-1f, Random.Range(-1f, 1f));
               break;

            case 2:  // If bottom then change the random direction field to only face upward.
               // Bottom
               spawnPoint.x = Random.value;
               spawnPoint.y = 0;
               direction = new Vector2(Random.Range(-1f, 1f), 1f);
               break;

            case 3:  // If top then change the random direction field to only face downward.
               // Top
               spawnPoint.x = Random.value;
               spawnPoint.y = 1;
               direction = new Vector2(Random.Range(-1f, 1f), -1f);
               break;
        }

        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint); // Convert spawnpoint based on viewport.
        worldSpawnPoint.z = 0;

        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];  // Pick Asteroid from list of prefabs randomly.

        GameObject asteroidInstance = Instantiate(  // Instantiates the selected prefab at a random rotation on Z axis.
            selectedAsteroid,
            worldSpawnPoint,
            Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();  // Get instantiated objects rigidbody.

        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);  // Normalize the velocity and multiply by random force range.

    }
}
