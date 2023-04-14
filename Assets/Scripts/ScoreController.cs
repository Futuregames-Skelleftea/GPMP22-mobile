using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    public float score;
    public bool stoppedIncreasingScore;
    // Start is called before the first frame update
    void Update()
    {
        if(!stoppedIncreasingScore) IncreaseScore();
    }

    private void IncreaseScore()
    {
        score += Time.deltaTime * scoreMultiplier;
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    public int EndScoreIncrease()
    {
        stoppedIncreasingScore = true;
        scoreText.text = "";
        return Mathf.FloorToInt(score);
    }

    public void StartTimer()
    {
        stoppedIncreasingScore = false;
        
    }
}
