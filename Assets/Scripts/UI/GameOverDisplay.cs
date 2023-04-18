using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameOverDisplay : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private TMP_Text _scoreDisplay;

    public TMP_Text ScoreDisplay => _scoreDisplay;

    private void Awake()
    {
        GameManager.Instance.CanvasGameOverDisplay = this;
        gameObject.SetActive(false);
    }

    public void AdButtonInteractable(bool enable)
    {
        if (!_button) return;

        _button.interactable = enable;
    }
}
