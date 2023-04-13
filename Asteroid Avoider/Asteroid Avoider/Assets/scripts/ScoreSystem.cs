using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    // A flag indicating whether the score should be counted or not
    private bool shouldCount = true;
    // The current score value
    private float score;

    void Update()
    {
        if (!shouldCount)
        {
            // If the score should not be counted, exit the function
            return;
        }

        // Increase the score over time using the scoreMultiplier
        score += Time.deltaTime * scoreMultiplier;

        // Update the scoreText with the new score value, rounded down to the nearest integer
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    // This method is called to start counting the score
    public void StartTimer()
    {
        // Set the shouldCount flag to true to start counting the score
        shouldCount = true;
    }

    // This method is called to end counting the score and return the final score value
    public int EndTimer()
    {
        // Set the shouldCount flag to false to stop counting the score
        shouldCount = false;

        // Clear the scoreText from the screen
        scoreText.text = string.Empty;

        // Return the final score value, rounded down to the nearest integer
        return Mathf.FloorToInt(score);
    }

}
