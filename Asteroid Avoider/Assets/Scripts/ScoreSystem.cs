using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    // Reference to TextMeshProUGUI component for displaying score
    [SerializeField] private TMP_Text scoreText;
    // Multiplier for calculating score
    [SerializeField] private float scoreMultiplier;

    // Boolean flag for controlling whether to count the score
    private bool shouldCount = true;

    // The current score
    private float score;

    // Update is called once per frame
    void Update()
    {
        // If shouldCount is false, exit early and don't count score
        if (!shouldCount) { return; }

        // Calculate score based on time and multiplier
        score += Time.deltaTime * scoreMultiplier;

        // Convert score to an integer and update the score text display
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    // Method for starting the timer and counting the score
    public void StartTimer()
    {
        // Set shouldCount to true to start counting the score
        shouldCount = true;
    }

    // Method for stopping the timer and returning the final score
    public int EndTimer()
    {
        // Set shouldCount to false to stop counting the score
        shouldCount = false;

        // Clear the score text display
        scoreText.text = string.Empty;

        // Return the final score as an integer
        return Mathf.FloorToInt(score);

    }

}
