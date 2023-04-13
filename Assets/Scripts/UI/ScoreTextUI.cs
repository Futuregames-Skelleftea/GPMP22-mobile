using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextUI : MonoBehaviour
{
    /// <summary>
    /// Refrence to TextMesh
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private void Start()
    {
        // Give Refrence to GameManager
        if (_scoreText) GameManager.Instance.ScoreText = _scoreText;
    }
}
