using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] asteroidPrefabs; // Array of asteroids we want to spawn - ADD IN INSPECTOR
    [SerializeField] private float secondsBetweenAsteroids = 1.5f; // Time between spawns
    [SerializeField] private Vector2 forceRange; // Forcerange that applys upon spawning

    private Camera mainCamera;

    private float timer; //To keep track of when to spawn the next asteroid

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        timer -= Time.deltaTime; //Decrement the timer by the amount of time since last frame

        if (timer <= 0) //If the timer has elapsed
        {
            SpawnAsteroid();  //Spawn a new asteroid

            timer += secondsBetweenAsteroids; //Reset the timer for the next asteroid spawn
        }
    }

    private void SpawnAsteroid()
    {
        int side = Random.Range(0, 4);  //Choose a random side which asteroids will spawn from (0 = left, 1 = right, 2 = bottom, 3 = top)

        Vector2 spawnPoint = Vector2.zero; //Initialize spawn point to zero
        Vector2 direction = Vector2.zero; //Initialize direction point to zero

        switch (side)
        {
            case 0:
                //Leftside
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(-1f, 1f));
                break;
            case 1:
                //Rightside
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1f, 1f));
                break;
            case 2:
                //Bottomside
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                direction = new Vector2(Random.Range(-1f, 1f), 1f);
                break;
            case 3:
                //Topside
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                direction = new Vector2(Random.Range(-1f, 1f), -1f);
                break;
        }

        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);  //Convert spawn point to world coordinates
        worldSpawnPoint.z = 0;

        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)]; //Get a random asteroid prefab from the asteroidPrefabs array

        //Instantiate the selected asteroid prefab at the calculated worldSpawnPoint, with a random rotation
        GameObject asteroidInstance = Instantiate(
            selectedAsteroid, 
            worldSpawnPoint, 
            Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>(); //Get the asteroid rb

        //Calculate a random force and apply it to the asteroid in its direction
        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y); 
    }
}
