using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helps Button to do stuff without direct Refrences to Scipts
/// </summary>
public class ButtonHelper : MonoBehaviour
{
    /// <summary>
    /// Loads Level 1 Scene
    /// </summary>
    public void LoadPlayScene()
    {
        // Load Scene
        GameManager.LoadLevel1();
    }

    /// <summary>
    /// Loads Main Menu
    /// </summary>
    public void LoadMainMenuScene()
    {
        // Load Scene
        GameManager.LoadMainMenu();
    }
}
