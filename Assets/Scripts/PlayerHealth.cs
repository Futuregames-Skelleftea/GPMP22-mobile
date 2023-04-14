
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameOverHandler gameOverHandler; //Reference to the GameOverHandler object - ADD IN INSPECTOR

    public void Crash() //Called when we crashes
    {
        gameOverHandler.EndGame(); //Call our EndGame method which stops spawning asteroids and activate the UI

        gameObject.SetActive(false); //Disables the players game object
    }
}
