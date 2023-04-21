using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemTimeScale : MonoBehaviour
{
    public GameObject explosionPrefab; // Reference to the explosion prefab
    private ParticleSystem explosionParticleSystem;
    private ParticleSystem.MainModule explosionMainModule;
    private float originalSimulationSpeed;

    void Start()
    {
        // Get the ParticleSystem component of the explosionPrefab
        explosionParticleSystem = explosionPrefab.GetComponent<ParticleSystem>();

        // Get the MainModule of the explosionParticleSystem
        explosionMainModule = explosionParticleSystem.main;

        // Save the original simulation speed of the explosionParticleSystem
        originalSimulationSpeed = explosionMainModule.simulationSpeed;
    }

    void Update()
    {
        // If the game is paused
        if (Time.timeScale == 0)
        {
            // Set the simulation speed of the explosionParticleSystem to 0
            explosionMainModule.simulationSpeed = 0;

            // Force a single frame of particles to be emitted to update the particle system
            explosionParticleSystem.Emit(0);
        }
        // If the game is not paused
        else
        {
            // Set the simulation speed of the explosionParticleSystem to its original value
            explosionMainModule.simulationSpeed = originalSimulationSpeed;
        }
    }
}