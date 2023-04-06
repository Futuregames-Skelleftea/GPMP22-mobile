using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Button playButton;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;

    // Current energy level
    private int energy;

    // Key to store and retrieve energy level from PlayerPrefs
    private const string EnergyKey = "Energy";
    // Key to store and retrieve energy recharge time from PlayerPrefs
    private const string EnergyReadyKey = "EnergyReady";

    private void Start()
    {
        // Call the OnApplicationFocus method when the application gains focus
        OnApplicationFocus(true);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        // If the application does not have focus, do nothing
        if (!hasFocus)
        {
            return;
        }

        // Cancel any pending invocations of methods with Invoke or InvokeRepeating
        CancelInvoke();

        // Get the high score from PlayerPrefs
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);

        // Display the high score in the UI
        highScoreText.text = $"High Score: {highScore}";

        // Get the current energy level from PlayerPrefs
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        // If the energy level is zero, check if it needs to be recharged
        if (energy == 0)
        {
            // Get the energy recharge time from PlayerPrefs
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            // If there is no energy recharge time, do nothing
            if (energyReadyString == string.Empty)
            {
                return;
            }

            // Parse the energy recharge time from string to DateTime
            DateTime energyReady = DateTime.Parse(energyReadyString);

            // If the current time is later than the energy recharge time, recharge energy
            if (DateTime.Now > energyReady)
            {
                energy = maxEnergy;
                // Save the new energy level to PlayerPrefs
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            // If the current time is earlier than the energy recharge time, disable the play button and schedule a recharge
            else
            {
                playButton.interactable = false;
                // Invoke the EnergyRecharged method after the remaining recharge time
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
            }
        }

        // Display the current energy level in the UI
        energyText.text = $"Play ({energy})";
    }

    private void EnergyRecharged()
    {
        // Enable the play button
        playButton.interactable = true;
        // Set the energy level to the maximum
        energy = maxEnergy;
        // Save the new energy level to PlayerPrefs
        PlayerPrefs.SetInt(EnergyKey, energy);
        // Display the new energy level in the UI
        energyText.text = $"Play ({energy})";
    }

    public void Play()
    {
        // If the energy level is zero or negative, do nothing
        if (energy < 1)
        {
            return;
        }

        // Decrement the energy level
        energy--;

        // Save the new energy level to PlayerPrefs
        PlayerPrefs.SetInt(EnergyKey, energy);

        if (energy == 0)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(energyReady);
#endif
        }

        SceneManager.LoadScene(1);
    }
}
