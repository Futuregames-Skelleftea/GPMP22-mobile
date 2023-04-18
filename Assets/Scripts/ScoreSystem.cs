using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    // Playing state
    private bool _isPlaying = false;
    public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            // set score text active depending on play state
            if (_scoreDisplayText) _scoreDisplayText.gameObject.SetActive(value);
            
            // set value
            _isPlaying = value;
        }
    }
    //

    private void Awake()
    {
        // say hey here I am to game manager
        GameManager.Instance.ScoreSystem = this;
        
        // set Playing state to true
        IsPlaying = true;
    }

    // score
    private float _scoreMultiplier = 5f;
    private float _score = 0f;
    public int ScoreFloored => Mathf.FloorToInt(_score);
    //

    private void Update()
    {
        if (IsPlaying)
        {
            // add score
            UpdateScore(Time.deltaTime);

            // update UI with new score
            UpdateUI();
        }

    }

    private void UpdateScore(float deltaTime)
    {
        _score += deltaTime * _scoreMultiplier;
    }

    private void UpdateUI()
    {
        if (!_scoreDisplayText) return;

        _scoreDisplayText.text = ScoreFloored.ToString();
    }

    [SerializeField]
    private TMP_Text _scoreDisplayText;
    public int EndTimer()
    {

        return ScoreFloored;
    }
}
