using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    private bool shouldCount = true;
    private float score;

    void Update()
    {
        if (!shouldCount) { return; }  // Stop running if shouldCount is set to false.

        score += Time.deltaTime * scoreMultiplier;  // Updates score based on time alive in game.

        scoreText.text = Mathf.FloorToInt(score).ToString();  // Rounds down score to int and displays it in text.
    }

    public void StartTimer()  // Start Count.
    {
        shouldCount = true;
    }

    public int EndTimer()  // Stops count, empties the score text from game & returns the final score.
    {
        shouldCount = false;

        scoreText.text = string.Empty;

        return Mathf.FloorToInt(score);

    }
}
