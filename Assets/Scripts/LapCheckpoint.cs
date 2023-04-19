using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCheckpoint : MonoBehaviour
{
    [SerializeField] LapsController lapsController; //the controller for laps, script is placed on the starting line
    [SerializeField] int checkpointId;  //the placing of the checkpoint in relation to the other ones (1st, 2nd, 3rd, etc)
    private void OnTriggerEnter(Collider other)
    {
        //if a car enters the checkpoint then set the bool value corresponding with the checkpoint ID to true
        if (other.CompareTag("Car"))
        {
            lapsController.passedCheckpoint[checkpointId - 1] = true;   //the checkpoints are labeled 1st, 2nd, 3rd, etc hence the "-1"
        }
    }
}
