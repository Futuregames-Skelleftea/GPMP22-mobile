using UnityEngine;

public class Asteroid : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
		{
			playerHealth.Crash();
		}
	}
}
