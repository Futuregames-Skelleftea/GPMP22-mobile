using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    // UI variables
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Button playButton;

    //Energy Variable
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;
    private int energy;
    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";

    [SerializeField] private NotificationHandler notificationHandler;

    private void Start()
    {
        OnApplicationFocus(true);
    }

    //called when the application is in focus
    private void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus) { return; }

        CancelInvoke();

        // set int Highscore to "highScoreKey"
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.text = $"HighScore: {highScore}";

        energy = PlayerPrefs.GetInt("Energy", maxEnergy);

        //if the energy is 0 start timer for energy recharge.
        if (energy == 0)
        {
            string enegryReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            if (enegryReadyString == string.Empty) { return; }

            DateTime energyReady = DateTime.Parse(enegryReadyString);
            
            //set energy to max energy after the timer.
            if(DateTime.Now >  energyReady) 
            {               
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }

            //make the button noninteractable and start timer for energy recharge.
            else
            {                
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged),(energyReady - DateTime.Now).Seconds);
            }
        }
        //text for the play button
        energyText.text = $"Play({energy})";
    }

    //method for recharging energy and setting the play button to interactable.
    private void EnergyRecharged()
    {
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey,energy);
        energyText.text = $"Play({energy})";
        playButton.interactable = true;
    }
    public void LoadGame(int BuildIndex)
    {
        //If you dont have atleast 1 energy do nothing
        if(energy < 1) { return; }

        //remove 1 energy
        energy--;

        //Set energy
        PlayerPrefs.SetInt(EnergyKey, energy);

        //recharge energy at the speed of energy recharge.
        if(energy == 0)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
            //send notification when energyReady
            notificationHandler.ScheduleNotification(energyReady);
        }
        
        //load scene based on buildinex
        SceneManager.LoadScene(BuildIndex);
    }

}
