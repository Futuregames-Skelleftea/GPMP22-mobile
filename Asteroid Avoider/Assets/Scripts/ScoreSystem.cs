using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This class manages the score and its display
public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;


    private bool shouldCount = true;
    private float score;

    void Update()
    {
        // Update score if timer is active
        if (!shouldCount) { return;}

        // Increase score based on time and multiplier
        score += Time.deltaTime * scoreMultiplier;

        // Display the score as an integer
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    // Start the score timer
    public void StartTimer()
    {
        shouldCount = true;
    }

    // Stop the score timer and return the final score
    public int EndTimer()
    {
        shouldCount = false;

        scoreText.text = string.Empty;

        return Mathf.FloorToInt(score);
    }
}
