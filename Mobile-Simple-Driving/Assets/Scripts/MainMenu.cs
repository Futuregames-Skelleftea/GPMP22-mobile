using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //Serialized fields that you will see Unity Editor
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Button playButton;
    [SerializeField] private AndroidNoteHandler androidNoteHandler;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDur;

    //current energy count
    private int energy;

    //constants PlayerPrefskeys
    private const string EnergyKey = "Enery";
    private const string EnergyRdyKey = "EneryRdy";


    private void  Start() 
    {
        //calling the OnApplicationFocus method as true
        OnApplicationFocus(true);
    }
    //handle focus changes for application
    private void OnApplicationFocus(bool hasFocus) 
    {
        // if application are not active, return
        if(!hasFocus)
        {
            return;
        }

        //cancel any Invoke calls
        CancelInvoke();

        //getting the highscore from PlayerPrefs
        int highScore =  PlayerPrefs.GetInt(ScoreSystem.HighScoreKey,0);

        //displaying highscore
        highScoreText.text = $"HighScore: {highScore}";

        //getting current energy from PlayerPrefs
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if(energy == 0)
        {
            //if energyRdy is empty, return
            string energyRdyString = PlayerPrefs.GetString(EnergyRdyKey, string.Empty);

            if(energyRdyString == string.Empty)
            {
                return;
            }

            //parse energyRdy string into DateTime
            DateTime energyRdy =  DateTime.Parse(energyRdyString);

            //current time is less than energyRdy
            if(DateTime.Now > energyRdy)
            {
                //setting energy to max
                energy = maxEnergy;

                //saving energy to PlayerPrefs
                PlayerPrefs.SetInt(EnergyKey, energy);
            }

            //if current time is earlier than energyRdy
            else
            {
                //dont display the play button and schedule energy recharge
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharge), (energyRdy - DateTime.Now).Seconds);
            }
        }
        //displaying energy
        energyText.text = $"Play({energy})";
    }

    //handle energy recharge
    private void EnergyRecharge()
    {
        //display the play button, setting energy to max, saving energy to PlayerPrefs and display energy
        playButton.interactable = true;
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = $"Play({energy})";
    }

    //handle play button
    public void Play()
    {
        //if energy is less than 1, return
        if(energy < 1)
        {
            return;
        }
        //minus the energy
        energy--;
        //saving the energy to PlayerPrefs
        PlayerPrefs.SetInt(EnergyKey, energy);

        //if energy is 0
        if(energy == 0)
        {
            // Setting the time to energy will be recharged
            DateTime energyRdy = DateTime.Now.AddMinutes(energyRechargeDur);
            PlayerPrefs.SetString (EnergyRdyKey, energyRdy.ToString());

            // Schedule a notification on Android devices
#if UNITY_ANDROID
            androidNoteHandler.ScheduleNote(energyRdy);
#endif

        }
        
        //Load Level scene
        SceneManager.LoadScene(1);

    }
}
