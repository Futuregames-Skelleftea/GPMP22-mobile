using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    [SerializeField] Image _lifeGauge;
    [SerializeField] Score _scoreManager;
    [SerializeField] float _maxLife = 5f;
    [SerializeField] float _lifeIncreaseValue;
    [SerializeField] float _lifeDecreaseValue;

    float _life;

    private void Awake()
    {
        _life = _maxLife;
    }

    // To prevent the life from going over the limits
    private void ClampLife()
    {
        _life = Mathf.Clamp(_life, 0f, 5f);
    }

    public void IncreaseLife()
    {
        _life += _lifeIncreaseValue;
        ClampLife();
        UpdateLifeGauge();
    }

    public void DecreaseLife()
    {
        _life -= _lifeDecreaseValue;
        ClampLife();
        UpdateLifeGauge();

        if (_life == 0f)
        {
            _scoreManager.EndGame();
        }
    }

    // Changes the color of the life gauge
    // depending on the remaining life
    private void UpdateLifeGauge()
    {
        float red = 0f;
        float green = 0f;

        // Upper half of the life gauge
        // Gradually gets more green the closer the life is to max
        if(_life/_maxLife > 0.5f)
        {
            red = 1 - (_life/_maxLife - 0.5f) * 2f;
            green = 1f;
        }

        // Lower Half of the life gauge
        // Gradually gets more red the close the life is to 0
        else if(_life/_maxLife <= 0.5)
        {
            red = 1f;
            green = _life / _maxLife * 2;
        }

        _lifeGauge.color = new Color(red, green, 0f);
    }
}
