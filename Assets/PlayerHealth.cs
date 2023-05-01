using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    public void Crash()
    {
        asteroidSpawner.StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
