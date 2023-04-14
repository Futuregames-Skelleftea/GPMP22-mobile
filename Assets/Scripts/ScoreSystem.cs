using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] float _scoreMultiplier;

    public bool _shouldCount = true;
    public float _score;

    // Gradually increases the score over time
    void Update()
    {
        if (!_shouldCount) return;

        _score += Time.deltaTime * _scoreMultiplier;
        _scoreText.text = Mathf.FloorToInt(_score).ToString();
    }

    // Used when an ad has finished
    public void StartTimer()
    {
        _shouldCount = true;
    }

    // Used when the player crashes
    public int EndTimer()
    {
        _shouldCount = false;

        _scoreText.text = string.Empty;

        return Mathf.FloorToInt(_score);
    }
}
