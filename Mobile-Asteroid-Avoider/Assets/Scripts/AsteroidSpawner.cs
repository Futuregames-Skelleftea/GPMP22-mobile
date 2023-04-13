using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    //variables are set in the inspector
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private float secondsBetweenAsteroids = 1.5f;
    [SerializeField] private Vector2 forceRange;

    //variables for the timer and the camera
    private float timer;
    private Camera mainCamera;

    private void Start() 
    {
        //Set maincamera to the camera in game
        mainCamera = Camera.main;
    }
    void Update()
    {
        //Minus timer amount of time that has passed
        timer -= Time.deltaTime;

        //When timer reach zero, spawn a new asteroid
        if(timer <= 0)
        {
            //Calling SpawnAsteroid method
            SpawnAsteroid();

            //Reset timer of seconds between asteroids
            timer += secondsBetweenAsteroids;
        }
    }

    //Method creates an asteroid and gets a random position and speed
    private void SpawnAsteroid()
    {
        //Setting a random side for the asteroid to spawn
        int side = Random.Range(0, 4);

        //Variables that will hold spawnpoints and directions of asteroids
        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        //Random side that will spawn and set direction of asteroid
        switch(side) 
        {
            case 0:
                //left
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(-1f, 1f));
                break;
            case 1:
                //right
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1f, 1f));
                break;
            case 2:
                //bottom
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                direction = new Vector2(Random.Range(-1f, 1f), 1f);
                break;
            case 3:
                //top
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                direction = new Vector2(Random.Range(-1f, 1f), -1f);
                break;

        }

        //Converting spawnpoint from viewport to worldpoint
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0;

        //Random asteroid will spawn of the prefabs
        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        //Instantiate the asteroid at the spawn point with a random rotation in all axies
        GameObject asteroidInstance = Instantiate(selectedAsteroid, worldSpawnPoint, Quaternion.Euler(Random.Range(0f,360f),Random.Range(0f,360f),Random.Range(0f,360f)));

        //Getting the rigidbody of asteroid
        Rigidbody _rB = asteroidInstance.GetComponent<Rigidbody>();
        
        //Setting the force at random
        _rB.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);

    }
}
