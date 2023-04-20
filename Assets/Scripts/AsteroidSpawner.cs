using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] asteroidPrefabs;
    [SerializeField] ScoreController scoreController;
    [SerializeField] float asteroidCooldown;
    [SerializeField] Vector2 forceRange;

    public bool canSpawn = true;
    private float timer;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;   //reference to the main camera
        StartCoroutine(SpawnAsteroids());   //starts the spawning coroutine
    }

    //coroutine which continously spawns asteroids 
    public IEnumerator SpawnAsteroids()
    {
        //spawn as long as the cooldown isnt zero and the canSpawn variable is true
        while (asteroidCooldown != 0 && canSpawn)
        {
            int side = Random.Range(0, 4);  //picks a side to spawn from

            Vector2 spawnPoint = Vector2.zero;
            Vector2 direction = Vector2.zero;

            //switch statement which creates a spawn viewport position and a corresponding direction
            switch (side)
            {
                case 0:
                    //left side
                    spawnPoint.x = 0;
                    spawnPoint.y = Random.value;
                    direction = new Vector2(1, Random.Range(-1f, 1f));
                    break;
                case 1:
                    //right side
                    spawnPoint.x = 1;
                    spawnPoint.y = Random.value;
                    direction = new Vector2(-1, Random.Range(-1f, 1f));
                    break;
                case 2:
                    //bottom side
                    spawnPoint.x = Random.value;
                    spawnPoint.y = 0;
                    direction = new Vector2(Random.Range(-1f, 1f), 1);
                    break;
                case 3:
                    //top side
                    spawnPoint.x = Random.value;
                    spawnPoint.y = 1;
                    direction = new Vector2(Random.Range(-1f, 1f), -1);
                    break;
            }

            Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);  //transforms the viewport spawn position to a worldposition
            worldSpawnPoint.z = 0;  //its a 2.5d game so making sure there is no depth value

            GameObject asteroidToSpawn = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];  //picks a random asteroid prefab

            GameObject spawnedAsteroid = Instantiate(asteroidToSpawn, worldSpawnPoint,Quaternion.Euler(0,0,Random.Range(0,360)));   //spawns it with a random rotation

            spawnedAsteroid.GetComponent<Rigidbody>().velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);

            //sets the score controller reference on the individual asteroids so that they can add score if they get destroyed
            spawnedAsteroid.GetComponent<Asteroid>().scoreController = scoreController; 

            //waits a set cooldown before spawning the next asteroid
            yield return new WaitForSeconds(asteroidCooldown);
        }
    }
}
