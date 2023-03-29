using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Button playButton;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;

    private int energy;

    // constant variables for PlayerPrefs keys
    private const string EnergyKey = " Energy" ;
    private const string EnergyReadyKey = "EnergyReady";

    private void Start()
    {
        // Call the OnApplicationFoucus method when true
        OnApplicationFoucus(true);
    }

    // OnApplicationFoucus() is called when the application loses or gains focus
    private void OnApplicationFoucus(bool hasFoucus)
    {
        // if the application lost focus, do nothing
        if (!hasFoucus) { return; }

        // cancel any previously scheduled Invoke calls
        CancelInvoke();

        // retrieve high score from PlayerPrefs
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);

        // set the high score text using string interpolation
        highScoreText.text = $"High Score: {highScore}";

        // retrieve energy level from PlayerPrefs
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if(energy == 0)
        {
            // if energy is depleted, check if it's time for recharge
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            // if there's no energy recharge time stored in PlayerPrefs, do nothing
            if (energyReadyString == string.Empty) { return; }

            // parse the energy recharge time from PlayerPrefs
            DateTime energyReady = DateTime.Parse(energyReadyString);

            // if it's time for recharge, enable the play button and recharge energy
            if (DateTime.Now > energyReady)
            {
                playButton.interactable = true;
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            // otherwise, schedule the energy recharge and disable the play button
            else
            {
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
            }

        }

        // update the energy text using string interpolation
        energyText.text = $"Play({energy})";

    }

    // EnergyRecharged() is called when the energy is fully recharged
    private void EnergyRecharged()
    {
        // enable the play button and recharge energy
        playButton.interactable = true;
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = $"Play({energy})";
    }

    // Play() is called when the play button is clicked
    public void Play()
    {
        // if there's no energy, do nothing
        if (energy < 1) { return; }

        // consume energy and update PlayerPrefs
        energy--;
        PlayerPrefs.SetInt(EnergyKey, energy);

        // if energy is depleted, schedule the energy recharge and notify the user
        if (energy == 0)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());

            // Only include this constant if the platform is Android
#if UNITY_ANDROID
            // schedule a notification for the energy recharge time
            androidNotificationHandler.SceduleNotification(energyReady);
#endif
        }

        // Calculate the datetime one minute from now
        DateTime oneMinuteFromNow = DateTime.Now.AddMinutes(1);

        // Convert the datetime object to string, but the result is not assigned to any variable or used in any way
        oneMinuteFromNow.ToString();

        // Load the scene with index 1 
        SceneManager.LoadScene(1);
    }
}
