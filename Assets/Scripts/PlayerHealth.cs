using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameOverHandler gameOverHandler;
    public void GameOver()
    {
        playerMovement.StopAiming();
        playerMovement.StopMovement();
        playerMovement.laserAnchor.SetActive(false);
        gameObject.SetActive(false);
        gameOverHandler.Endgame();
    }
}
