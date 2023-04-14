using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameOverHandler gameOverHandler;


    public void Crash()
    {
        gameOverHandler.EndGame();  // Call the EndGame function.

        gameObject.SetActive(false);  // When Crash is called set object to false.
    }
}
