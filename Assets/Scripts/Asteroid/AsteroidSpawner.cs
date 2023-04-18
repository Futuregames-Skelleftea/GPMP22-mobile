using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    /// <summary>
    /// array of all Asteroid prefabs
    /// </summary>
    [SerializeField]
    private GameObject[] _asteroidPrefabs;

    /// <summary>
    /// time between Spawning asteroid
    /// </summary>
    [SerializeField]
    private float _asteroidSpawnRate = 0.5f;

    /// <summary>
    /// x => Minimum velocity force | y => Maximum velocity force
    /// </summary>
    [SerializeField]
    private Vector2 forceRange = new Vector2(1,3);

    /// <summary>
    /// Gets a random Prefab from prefabs array
    /// </summary>
    private GameObject RandomAsteroidPrefab { 
        get 
        {
            return _asteroidPrefabs[Random.Range(0, _asteroidPrefabs.Length)];        
        } 
    }

    private void Update()
    {
        UpdateSpawnTimer(Time.deltaTime);
    }

    /// <summary>
    /// Time until next Asteroid is spawned
    /// </summary>
    private float _asteroidSpawnTimer;

    /// <summary>
    /// Counts down to spawn a new Asteroid
    /// </summary>
    /// <param name="deltaTime">Time.deltaTime or Time.fixedDeltaTime</param>
    private void UpdateSpawnTimer(float deltaTime)
    {
        _asteroidSpawnTimer -= deltaTime;
        if (_asteroidSpawnTimer <= 0)
        {
            SpawnAsteroid();
            _asteroidSpawnTimer += _asteroidSpawnRate;
        }
    }

    /// <summary>
    /// Spawns a Asteroid
    /// </summary>
    private void SpawnAsteroid()
    {
        // Get a random Asteroid Prefab
        GameObject newAsteroid = RandomAsteroidPrefab;

        // Select Side to spawn on
        SpawnSide spawnSide = (SpawnSide)Random.Range(0, 4);
        
        // Generate Variables
        Vector2 spawnPoint = Vector2.zero;
        Vector2 spawnDirection = Vector2.zero;

        // Select Spawnpoint and velocity direction
        switch (spawnSide)
        {
            case SpawnSide.North:
                // Position
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                // Direction
                spawnDirection = new Vector2((Random.value*2) - 1,-1);
                break;
            case SpawnSide.East:
                // Position
                spawnPoint.y = Random.value;
                spawnPoint.x = 1;
                // Direction
                spawnDirection = new Vector2(-1, (Random.value * 2) - 1);
                break;
            case SpawnSide.South:
                // Position
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                // Direction
                spawnDirection = new Vector2((Random.value * 2) - 1, 1);
                break;
            case SpawnSide.West:
                // Position
                spawnPoint.y = Random.value;
                spawnPoint.x = 0;
                // Direction
                spawnDirection = new Vector2(1, (Random.value * 2) - 1);
                break;
        }

        // spawn the Prefab and get refrence to asteroid
        Asteroid newlySpawnedAsteroid =
        Instantiate(
            // selected Asteroid
            newAsteroid,
            // View position - z
            Camera.main.ViewportToWorldPoint(spawnPoint) - Camera.main.transform.position.z * Vector3.forward,
            // random Rotation
            Quaternion.Euler(0,0,Random.Range(-180f,180f)) //
            )// Get Asteroid Refrence
            .GetComponent<Asteroid>(); 

        // Give Asteroid direction and force of velocity
        newlySpawnedAsteroid.Init(spawnDirection, Random.Range(forceRange.x,forceRange.y));
        
    }

    private enum SpawnSide
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }
}
