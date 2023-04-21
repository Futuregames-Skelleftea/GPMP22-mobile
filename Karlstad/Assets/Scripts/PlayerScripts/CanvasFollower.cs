using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollower : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        // Set the position of the canvas to be the same as the position of the player
        transform.position = player.transform.position;
    }
}
