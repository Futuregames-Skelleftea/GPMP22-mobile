using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // An array of asteroid prefabs to 
    [SerializeField] private GameObject[] asteroidPrefabs;

    // Time interval between asteroid spawns.
    [SerializeField] private float secondsBetweenAsteroids = 1.5f;

    // Range of force to apply to the asteroid.
    [SerializeField] private Vector2 forceRange;

    // The main camera in the scene.
    private Camera mainCamera;

    // A timer to keep track of when to spawn asteroids.
    private float timer;


    void Start()
    {
        // Get the main camera in the scene.
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease the timer by the amount of time passed since the last frame.
        timer -= Time.deltaTime;

        // If the timer is less than or equal to 0.
        if (timer <= 0)
        {
            // Spawn an asteroid.
            SpawnAsteroid();

            // Reset the timer to the time interval between asteroid spawns.
            timer += secondsBetweenAsteroids;
        }
    }

    // This method spawns an asteroid.
    private void SpawnAsteroid()
    {
        // Randomly choose a side for the asteroid to spawn from.
        int side = Random.Range(0, 4);

        // The spawn point of the asteroid.
        Vector2 spawnPoint = Vector2.zero;
        // The direction the asteroid will travel in.
        Vector2 direction = Vector2.zero;

        // Depending on the side chosen...
        switch (side)
        {
            case 0:
                //Left
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(-1f, 1f));

                break;
            case 1:
                // Right
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1f, 1f));

                break;
            case 2:

                // Bottom
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                direction = new Vector2(Random.Range(-1f, 1f), 1f);

                break;
            case 3:
                //Top
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                direction = new Vector2(Random.Range(-1f, 1f), -1f);

                break;

        }

        // Convert the spawn point from viewport to world space.
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);

        // Set the z-coordinate to 0 to spawn the asteroid on the same plane as the game.
        worldSpawnPoint.z = 0;

        // Randomly select an asteroid prefab.
        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // Instantiate the asteroid at the spawn point with a random rotation.
        GameObject asteroidInstance = Instantiate(selectedAsteroid, worldSpawnPoint, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        // Get the asteroid's rigidbody.
        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();

        // Apply a random force to the asteroid in the chosen direction.
        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);

    }

}
