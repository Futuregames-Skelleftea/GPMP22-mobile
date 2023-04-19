using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playText;  //the text for the play button
    [SerializeField] GameObject noEnergyText;   //the text which appears if you have no energy
    [SerializeField] TextMeshProUGUI highscoreText; //the text which displays the highscore
    [SerializeField] AndroidNotificationHandler notificationHandler;    //reference to the androidnotificationhandler script
    private bool hasSetRechargeReadyTime = false;   //if the recharge time has been set 

    private void Start()
    {
        //If the recharge time has not been assigned then set that time to some time in the future and schedule a notification if on android
        if (GameManager.instance.GetSavedEnergy() == 0 && !GameManager.instance.GetSavedEnergyReadyTimeHasBeenAssigned() && GameManager.instance.GetSavedEnergyReadyTime() < DateTime.Now)
        {
            DateTime energyReadyTime = DateTime.Now.AddMinutes(GameManager.instance.energyRechargeTime);
            GameManager.instance.SaveAtIndex(2, energyReadyTime.ToString());
#if UNITY_ANDROID
            notificationHandler.ScheduleNotification(energyReadyTime);
#endif
            GameManager.instance.SaveAtIndex(3,"True");
        }

        //if there is no energy then display the no energy text
        if(GameManager.instance.GetSavedEnergy() == 0)
        {
            noEnergyText.SetActive(true);
        }

        //if there is a recharge time set then invoke a method which resets the energy in however long it is left untill that recharge time
        if (GameManager.instance.GetSavedEnergyReadyTimeHasBeenAssigned())
        {
            Invoke(nameof(SetMaxEnergy), (float)(GameManager.instance.GetSavedEnergyReadyTime() - DateTime.Now).TotalSeconds);
        }

        //update the energy in displayed on the play button
        if (playText)
        {
            playText.SetText("PLAY (" + GameManager.instance.GetSavedEnergy() + ")");
        }

        //update the highscore text with the saved highscore
        if (highscoreText)
        {
            highscoreText.SetText("Completed Laps: " + GameManager.instance.GetSavedHighscore());
        }
    }

    //function called by the Play button
    //lowers the saved energy and loads the main game scene
    public void StartGame()
    {
        int tempEnergy = GameManager.instance.GetSavedEnergy();
        if (tempEnergy > 0)
        {
            tempEnergy--;
            GameManager.instance.SaveAtIndex(1, tempEnergy.ToString());
            SceneManager.LoadScene(1);
        }
    }

    //function called by the Quit Button
    public void QuitGame()
    {
        Application.Quit();
    }

    //everytime the game is focused
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) { return; }

        //If the recharge time has not been assigned then set that time to some time in the future and schedule a notification if on android
        if (GameManager.instance.GetSavedEnergy() == 0 && !GameManager.instance.GetSavedEnergyReadyTimeHasBeenAssigned() && GameManager.instance.GetSavedEnergyReadyTime() < DateTime.Now)
        {
            DateTime energyReadyTime = DateTime.Now.AddMinutes(GameManager.instance.energyRechargeTime);
            GameManager.instance.SaveAtIndex(2, energyReadyTime.ToString());
#if UNITY_ANDROID
            notificationHandler.ScheduleNotification(energyReadyTime);
#endif
            GameManager.instance.SaveAtIndex(3, "True");
        }

        if (GameManager.instance.GetSavedEnergy() == 0)
        {
            noEnergyText.SetActive(true);
        }

        CancelInvoke();  //cancels the previous invoke if the SetMaxEnergy function

        GameManager gameManager = GameManager.instance;     //gets a referance to the gamemanager, just so i dont have to write Gamemanager.instance all the time 

        if (gameManager.GetSavedEnergy() == 0)
        {
            DateTime energyReadyTime = gameManager.GetSavedEnergyReadyTime();   //gets the energy recharge time from save data

            if (DateTime.Now > energyReadyTime)
            {
                gameManager.energy = gameManager.maxEnergy;
                gameManager.SaveAtIndex(1, gameManager.maxEnergy.ToString());   //saves the new energy value
                GameManager.instance.SaveAtIndex(3, "False");   //saves that there is no recharge time set and a new can be assigned if the energy gets low enough
                noEnergyText.SetActive(false);  //remove the no energy text
            }
            else
            {
                Invoke(nameof(SetMaxEnergy), (float)(energyReadyTime - DateTime.Now).TotalSeconds);     //invoking a function which resets the energy at the same time as the energy ready time
            }
        }
        if (playText)
        {
            playText.SetText("PLAY (" + gameManager.GetSavedEnergy() + ")");    //updates the play button text
        }
    }

    //function to be called at the same time as the recharge time if you have the game open when that time is reached
    //done this way because otherwise you would need to re-open the game to refresh your energy because that check only happnes once
    private void SetMaxEnergy()
    {
        GameManager gameManager = GameManager.instance;
        gameManager.energy = gameManager.maxEnergy; //cant remember if this is necessary but cant hurt
        gameManager.SaveAtIndex(1, gameManager.energy.ToString());  //resets the energy to whatever the max is and saves it
        if (playText)
        {
            playText.SetText("PLAY (" + gameManager.GetSavedEnergy() + ")");    //updates the play text
        }
        GameManager.instance.SaveAtIndex(3, "False");   //saves that there is no recharge time set and a new can be assigned if the energy gets low enough
        noEnergyText.SetActive(false);  //remove the no energy text
    }
}
