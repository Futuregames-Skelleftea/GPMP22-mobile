using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playText;
    [SerializeField] GameObject noEnergyText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] AndroidNotificationHandler notificationHandler;
    private bool hasSetRechargeReadyTime = false;


    private void Start()
    {
        if (GameManager.instance.GetSavedEnergy() == 0 && !GameManager.instance.GetSavedEnergyReadyTimeHasBeenAssigned() && GameManager.instance.GetSavedEnergyReadyTime() < DateTime.Now)
        {
            DateTime energyReadyTime = DateTime.Now.AddMinutes(GameManager.instance.energyRechargeTime);
            GameManager.instance.SaveAtIndex(2, energyReadyTime.ToString());
#if UNITY_ANDROID
            notificationHandler.ScheduleNotification(energyReadyTime);
#endif
            GameManager.instance.SaveAtIndex(3,"True");
        }

        if(GameManager.instance.GetSavedEnergy() == 0)
        {
            noEnergyText.SetActive(true);
        }


        if (GameManager.instance.GetSavedEnergyReadyTimeHasBeenAssigned())
        {
            Invoke(nameof(SetMaxEnergy), (float)(GameManager.instance.GetSavedEnergyReadyTime() - DateTime.Now).TotalSeconds);
        }

        if (playText)
        {
            playText.SetText("PLAY (" + GameManager.instance.GetSavedEnergy() + ")");
        }

        if (highscoreText)
        {
            highscoreText.SetText("Completed Laps: " + GameManager.instance.GetSavedHighscore());
        }
    }

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

    public void QuitGame()
    {
        Application.Quit();
    }

    //everytime the game is focused
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) { return; }

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

        CancelInvoke();

        GameManager gameManager = GameManager.instance;     //gets a referance to the gamemanager, just so i dont have to write Gamemanager.instance all the time 

        if (gameManager.GetSavedEnergy() == 0)
        {
            DateTime energyReadyTime = gameManager.GetSavedEnergyReadyTime();

            if (DateTime.Now > energyReadyTime)
            {
                gameManager.energy = gameManager.maxEnergy;
                gameManager.SaveAtIndex(1, gameManager.maxEnergy.ToString());
                GameManager.instance.SaveAtIndex(3, "False");
                noEnergyText.SetActive(false);
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

    private void SetMaxEnergy()
    {
        GameManager gameManager = GameManager.instance;
        gameManager.energy = gameManager.maxEnergy;
        gameManager.SaveAtIndex(1, gameManager.energy.ToString());
        if (playText)
        {
            playText.SetText("PLAY (" + gameManager.GetSavedEnergy() + ")");
        }
        GameManager.instance.SaveAtIndex(3, "False");
        noEnergyText.SetActive(false);
    }
}
