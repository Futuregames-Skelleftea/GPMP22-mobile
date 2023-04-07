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
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;

    private int energy;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";

    private void Start() // Start the focus when start is called.
    {
        OnApplicationFocus(true);
    }

    private void OnApplicationFocus(bool hasFocus) // Run main menu when app has focus.
    {
        if(!hasFocus){return;}

        CancelInvoke(); // Cancel the invokes upon focus.


        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0); // Check high score.

        highScoreText.text = $"High Score: {highScore}"; // Show current high score.

        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy); // Check amount of energy.

        if(energy == 0) // If out of energy.
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty); // Should energy be regenerated.

            if(energyReadyString == string.Empty) { return; } // IGNORE THIS: This Cancels code should an error occur.

            DateTime energyReady = DateTime.Parse(energyReadyString);

            if(DateTime.Now > energyReady) // If time has passed beyond energyReady, set energy to max.
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            else // If time has not passed, set button interactable to false.
            {
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds); // Subtracts the time left until energy is ready, even when minimized app.

            }
        }
        energyText.text = $"Play ({energy})"; // Display energy in text.
    }

    private void EnergyRecharged() // Once energy is recharged.
    {
        playButton.interactable = true;          // Make button interactable.

        energy = maxEnergy;                      // Set energy to max.

        PlayerPrefs.SetInt(EnergyKey, energy);   // Set it in Playerprefs.

        energyText.text = $"Play ({energy})";    // Display Energy in Playbutton Text.

    }

    public void Play()            // When pressing the play button.
    {
        if (energy < 1) {return;} // If energy less than one dont run rest of code.

        energy--;                 // Decrease energy by 1.

        PlayerPrefs.SetInt(EnergyKey, energy); // Save energy to playerprefs.

        if(energy == 0)           // If energy is 0 then wait until set amount of time has passed.
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(energyReady); // If running on android send notification.
#endif
        }
        SceneManager.LoadScene(1); // Run the game.
    }

    










}
