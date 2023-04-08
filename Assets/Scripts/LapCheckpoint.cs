using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCheckpoint : MonoBehaviour
{
    [SerializeField] LapsController lapsController;
    [SerializeField] int checkpointId;  //the placing of the checkpoint in relation to the other ones (1st, 2nd, 3rd, etc)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            lapsController.passedCheckpoint[checkpointId - 1] = true;
        }
    }
}
