using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameOver _gameOverHandler;

    public void Crash()
    {
        _gameOverHandler.EndGame();
        gameObject.SetActive(false);
    }
}
