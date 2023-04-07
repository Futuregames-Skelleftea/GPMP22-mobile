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

    //ui
    [SerializeField] private TextMeshProUGUI ScoreDisplay;
    [SerializeField] private TextMeshProUGUI LapDisplay;



    void Start()
    {

    }

    void Update()
    {

    }
    public void PointIncrease(int points)
    {
        Debug.Log("log");
        _score += points;
    }
    public void LapIncrease()
    {
        Debug.Log("g");
        if(Laps < 1) _laps--;
        _laps++;
    }
    public void UpdateUI()
    {
        LapDisplay.text = Laps.ToString();
        ScoreDisplay.text = Score.ToString();
    }
}
