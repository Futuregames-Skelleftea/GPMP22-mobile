using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidSpawner : MonoBehaviour
{
    // array with prefabs
    [SerializeField] private GameObject[] astroidPrefab;
    [SerializeField] private float secondsBetweenAstroids = 1.5f;
    [SerializeField] private Vector2 forceRange;

    private Camera mainCamera;
    private float timer;


    private void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            SpawnAstroid();

            timer += secondsBetweenAstroids;
        }
    }

    private void SpawnAstroid()
    {
        // Select a random side for the asteroid to spawn from
        int side = Random.Range(0, 4);

        // Initialize spawnPoint and direction vectors to zero
        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        // Depending on which side is selected, set the appropriate x and y values for spawnPoint and direction
        switch (side)
        {
            case 0:
                // Left
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
                // Top
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                direction = new Vector2(Random.Range(-1f, 1f), -1f);
                break;
        }

        // Convert the spawnPoint from viewport coordinates to world coordinates
        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0;

        // Randomly select an asteroid prefab from the array of asteroid prefabs
        GameObject selectedAstroid = astroidPrefab[Random.Range(0, astroidPrefab.Length)];

        // Instantiate the selected asteroid at the worldSpawnPoint with a random rotation
        GameObject astroidInstance = Instantiate(
            selectedAstroid, 
            worldSpawnPoint, 
            Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        Rigidbody rb = astroidInstance.GetComponent<Rigidbody>();

        // Set the velocity of the asteroid instance to the normalized direction vector multiplied by a random force in the forceRange
        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);
    }
}
