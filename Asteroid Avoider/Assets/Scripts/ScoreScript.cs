using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    private bool counting = true;
    private float score;

    private void Update()
    {
        if(!counting) { return; }

        //update score
        score += Time.deltaTime * scoreMultiplier;

        //update score text.
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    public int StopCount()
    {
        counting = false;

        scoreText.text = string.Empty;

        return Mathf.FloorToInt(score);
    }
}
