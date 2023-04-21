using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [SerializeField] List<CircleButton> _circleButtons = new List<CircleButton>();
    [SerializeField] float _startSize = 3;
    [SerializeField] float _spawnDelay = 0.5f;

    float _timer;

    // Shortens the interval between the circles over time
    void Update()
    {
        _spawnDelay -= Time.deltaTime * 0.01f;

        SpawnCircle();
    }

    // Spawns the circles at intervals
    private void SpawnCircle()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _circleButtons[Random.Range(0, _circleButtons.Count)].SpawnCircle(_startSize);
            _timer += _spawnDelay;
        }
    }
}
