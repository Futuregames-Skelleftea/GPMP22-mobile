using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private ScoreWigdetController scoreWigdetController;
    public void Crash()
    {
        asteroidSpawner.StopAllCoroutines();
        scoreWigdetController.GameOverState();
        gameObject.SetActive(false);
    }
}
