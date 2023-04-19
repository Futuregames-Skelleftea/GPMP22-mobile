using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private GameOver gameOver;
    public void Crash()
    {
        gameObject.SetActive(false);
        gameOver.EndGame();
    }

}
