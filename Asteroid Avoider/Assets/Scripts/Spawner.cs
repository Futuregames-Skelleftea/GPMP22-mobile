using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private float spawnTime = 1.5f;
    [SerializeField] private Vector2 forceRange;

    private Camera mainCamera;

    private float Timer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {      
        //timer for when to spawn the next asteroid
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            SpawnAsteroid();

            Timer += spawnTime;
        }
    }

    //method for spawning the asteroids
    private void SpawnAsteroid()
    {
        int side = Random.Range(0, 2);

        //location
        Vector2 spawnPoint = Vector2.zero;
        //direction
        Vector2 direction = Vector2.zero;

        switch (side)
        {
            //Left
            case 0:
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(1f, -1));
                break;
            // Right
            case 1:
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1, 1f));
                break;
            // Bottom
            case 2:
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                direction = new Vector2(Random.Range(-1, 1f),1f);
                break;
            // Top
            case 3:
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                direction = new Vector2(Random.Range(-1, 1f), -1f);
                break;
        }

        //convert viewport to worldpoint
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0;

        //randomlu selects on of the asteroids prefabs
        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        GameObject asteroidInstance = Instantiate
            //position
            (selectedAsteroid, worldSpawnPoint,
            //rotation
            Quaternion.Euler(0,0,Random.Range(0f, 360)));

        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();

        rb.velocity = direction.normalized * Random.Range(forceRange.x,forceRange.y);
    }
}
