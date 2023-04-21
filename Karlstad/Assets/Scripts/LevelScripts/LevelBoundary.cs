using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundary : MonoBehaviour
{
    // Declare two static public floats that represent the left and right boundaries of the level.
    public static float leftSide = -2.5f;
    public static float rightSide = 2.5f;

    // Declare two public floats that represent the internal left and right boundaries of the level.
    public float internalLeft;
    public float internalRight;

    // Update is called once per frame
    void Update()
    {
        // Set the value of internalLeft to be equal to leftSide.
        internalLeft = leftSide;

        // Set the value of internalRight to be equal to rightSide.
        internalRight = rightSide;
    }
}
