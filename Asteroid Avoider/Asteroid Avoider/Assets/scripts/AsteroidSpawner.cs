using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // an array of asteroid prefabs
    [SerializeField] private GameObject[] asteroidPrefabs;
    // the time between spawning asteroids
    [SerializeField] private float secondsBetweenAsteroids = 1.5f;
    // the range of force to apply to the spawned asteroids
    [SerializeField] private Vector2 forceRange;

    private Camera mainCamera;
    private float timer;

    private void Start()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Decrement the timer by the time that has passed since the last frame
        timer -= Time.deltaTime;

        // If the timer has reached zero or less
        if (timer <= 0)
        {
            // Spawn an asteroid
            SpawnAsteroid();
            // Reset the timer to the time between spawning asteroids
            timer += secondsBetweenAsteroids;  
        }
    }

    private void SpawnAsteroid()
    {
        // Choose a random side for the asteroid to spawn on
        int side = Random.Range(0, 4);

        // Declare variables for the spawn point and direction of the asteroid
        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        // Depending on the chosen side, set the spawn point and direction of the asteroid
        switch (side)
        {
            //Left side
            case 0:
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(-1f, 1f));
                break;
                //Right side
            case 1:
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1f, 1f));
                break;
                //Bottom side
            case 2:
                spawnPoint.x = Random.value;
                spawnPoint.y = 0f;
                direction = new Vector2(Random.Range(-1f, 1f), 1f);
                break;
                //Top side
            case 3:
                spawnPoint.x = Random.value;
                spawnPoint.y = 1f;
                direction = new Vector2(Random.Range(-1f, 1f), -1f);
                break;
        }

        // Convert the spawn point from viewport coordinates to world coordinates
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0;

        // Choose a random asteroid prefab to instantiate
        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // Instantiate the selected asteroid prefab at the world spawn point, with a random rotation
        GameObject asteroidInstance = Instantiate(selectedAsteroid, worldSpawnPoint, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        // Get the Rigidbody component of the asteroid instance
        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();

        // Apply a random force and range
        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);
    }
}
