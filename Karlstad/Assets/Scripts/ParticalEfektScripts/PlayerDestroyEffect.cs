using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyEffect : MonoBehaviour
{
    public GameObject particleSystemPrefab;

    private void OnDestroy()
    {
        // Create an instance of the particle system prefab at the position and rotation of the destroyed object
        Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
    }
}
