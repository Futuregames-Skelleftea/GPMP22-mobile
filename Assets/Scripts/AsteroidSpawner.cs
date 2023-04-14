using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _asteroids;
    [SerializeField] float _spawnDelay;
    [SerializeField] Vector2 _forceRange;

    Camera _mainCamera;
    float _timer;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if(_timer <= 0f)
        {
            SpawnAsteroid();

            _timer += _spawnDelay;
        }
    }

    // Spawns an asteroid from one of the edges of the screen
    // at an interval
    private void SpawnAsteroid()
    {
        int side = Random.Range(0, 4);

        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        switch (side)
        {
            case 0:
                spawnPoint.x = 0f;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(-1f, 1f));
                break;
            case 1:
                spawnPoint.x = 1f;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1f, 1f));
                break;
            case 2:
                spawnPoint.x = Random.value;
                spawnPoint.y = 0f;
                direction = new Vector2(Random.Range(-1f, 1f), 1f);
                break;
            case 3:
                spawnPoint.x = Random.value;
                spawnPoint.y = 1f;
                direction = new Vector2(Random.Range(-1f, 1f), -1f);
                break;
        }

        Vector3 worldSpawnPoint = _mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0f;
        

        GameObject newAsteroid = Instantiate(_asteroids[Random.Range(0, _asteroids.Length)],
                                             worldSpawnPoint,
                                             Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        newAsteroid.GetComponent<Rigidbody>().velocity = direction.normalized * Random.Range(_forceRange.x, _forceRange.y);
    }
}
