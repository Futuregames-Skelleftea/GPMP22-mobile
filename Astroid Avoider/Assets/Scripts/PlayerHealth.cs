using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameOverHandler gameOverHandler;

    // if the player crashes with a astroid, End game is called and the gameobject is deactivated
    public void Crash()
    {
        gameOverHandler.EndGame();
        gameObject.SetActive(false);
    }
}
