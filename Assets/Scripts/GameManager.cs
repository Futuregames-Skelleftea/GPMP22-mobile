using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //scorekeeping
    private int _laps;
    public int Laps => _laps;
    private int _score = 0;
    public int Score => _score;




    //gameplay ui
    [SerializeField] private TextMeshProUGUI _scoreDisplay;
    [SerializeField] private TextMeshProUGUI _lapDisplay;

    //post-game ui
    [SerializeField] private TextMeshProUGUI _endScoreDisplay;
    [SerializeField] private TextMeshProUGUI _endLapDisplay;
    [SerializeField] private GameObject _highScoreText;


    public void OnCarCollision()
    {
        int currHighScore = PlayerPrefs.GetInt("hiScore", 0);

        if (_score > currHighScore)
        {
            PlayerPrefs.SetInt("hiScore", _score);
            _highScoreText.SetActive(true);
        }


        _endLapDisplay.text = $"Laps: {Laps.ToString()}";
        _endScoreDisplay.text = $"Score: {Score.ToString()}";
    }

    public void PointIncrease(int points)
    {
        _score += points;
    }
    public void LapIncrease()
    {
        _laps++;
    }
    public void UpdateUI()
    {
        _lapDisplay.text = $"Laps: {Laps.ToString()}";
        _scoreDisplay.text = $"Score: {Score.ToString()}";
    }
}
