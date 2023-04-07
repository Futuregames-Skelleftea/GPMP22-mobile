using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private TMP_Text _energyText;
    [SerializeField] private Button _playButton;
    [SerializeField] private AndroidNotificationHandler _notificationHandler;
    [SerializeField] private int _maxEnergy;
    [SerializeField] private int _energyRechargeDuration;

    private int _energy;

    private const string _EnergyKey = "Energy";
    private const string _EnergyReadyKey = "EnergyReady";

    private void Start()
    {
        OnApplicationFocus(true);
    }

    // Handles all the main menu UI, including the high score and 
    // the energy system
    private void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus) { return; }

        CancelInvoke();

        _highScoreText.text = $"High Score: {PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0)}";

        _energy = PlayerPrefs.GetInt(_EnergyKey, _maxEnergy);

        if (_energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(_EnergyReadyKey, string.Empty);

            if (energyReadyString == string.Empty) { return; }

            DateTime energyReady = DateTime.Parse(energyReadyString);

            if (DateTime.Now > energyReady)
            {
                _energy = _maxEnergy;
                PlayerPrefs.SetInt(_EnergyKey, _energy);
            }
            else
            {
                _playButton.interactable = false;
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
            }
        }
        _energyText.text = $"Play ({_energy})";
    }

    // Allows the play button to start the game if energy has been recharged
    private void EnergyRecharged()
    {
        _playButton.interactable = true;
        _energy = _maxEnergy;
        PlayerPrefs.SetInt(_EnergyKey, _energy);
        _energyText.text = $"Play ({_energy})";
    }

    // Checks if there's enough energy to start the game
    // If there's enough play, otherwise waits for recharge
    public void Play()
    {
        if(_energy > 0)
        {
            _energy--;
            PlayerPrefs.SetInt(_EnergyKey, _energy);

            if(_energy == 0)
            {
                DateTime energyReady = DateTime.Now.AddMinutes(_energyRechargeDuration);
                PlayerPrefs.SetString(_EnergyReadyKey, energyReady.ToString());
#if UNITY_ANDROID
                _notificationHandler.ScheduleNotification(energyReady);
#endif
            }

            SceneManager.LoadScene(1);
        }

    }
}
