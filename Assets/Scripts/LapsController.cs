using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapsController : MonoBehaviour
{
    private int completedLaps;  //the number of completed laps for the current playthrough
    public bool[] passedCheckpoint; //array which is updated by the individual checkpoints when the player passes through them
    [SerializeField] TextMeshProUGUI completedLapsText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car")) TryCompleteLap();
    }

    //function which checks if all the checkpoints along the track have been passed before incrementing the completed laps
    private void TryCompleteLap()
    {
        //if a checkpoint has been somehow skipped then dont complete the lap
        for (int i = 0; i < passedCheckpoint.Length; i++)
        {
            if(passedCheckpoint[i] == false)
            {
                return; 
            }
        }

        completedLaps++; //increments the completed laps
        completedLapsText.SetText("Completed Laps: " + completedLaps);

        if (GameManager.instance.GetSavedHighscore() < completedLaps)    //if the current number of completed laps is more than the saved number then save the new highscore
        {
            GameManager.instance.SaveAtIndex(0, completedLaps.ToString());  //saving the new highscore
        }

        for (int i = 0; i < passedCheckpoint.Length; i++)
        {
            passedCheckpoint[i] = false;    //reseting which checkpoints have been passed
        }
    }
}
