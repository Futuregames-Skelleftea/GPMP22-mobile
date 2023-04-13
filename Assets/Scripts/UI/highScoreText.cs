using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class highScoreText : MonoBehaviour
{
    /// <summary>
    /// refrence to TextMesh
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _highScoreText;

    private void Start()
    {
        // Give refrence to GameManager
        if (_highScoreText)
            GameManager.Instance.HighScoreText = _highScoreText;
    }
}
