using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleButton : MonoBehaviour
{
    [SerializeField] Life _lifeManager;
    [SerializeField] Score _scoreManager;
    [SerializeField] GameObject _circle;
    [SerializeField] float _timingThreshold;

    List<GameObject> _circles = new List<GameObject>();
    RectTransform _rectTransform;
    AudioSource _source;
    Vector3 _buttonPosition;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _source = GetComponent<AudioSource>();
        _buttonPosition = Camera.main.ScreenToWorldPoint(_rectTransform.position);
        _buttonPosition.z = 0;
    }

    // The circle spawner will use this to spawn a circle for the button
    // that should spawn one
    public void SpawnCircle(float startSize)
    {
        GameObject newCircle = Instantiate(_circle, _buttonPosition, Quaternion.identity);
        newCircle.transform.localScale = Vector3.one * startSize;
        newCircle.GetComponent<Circle>().AssignButton(this);
        _circles.Add(newCircle);
    }

    // Checks how well the player timed the button press with the circle
    public void ClickCircle()
    {
        if (_circles.Count == 0) return;

        if (Mathf.Abs(_circles[0].transform.localScale.x - 1f) <= _timingThreshold)
        {
            _scoreManager.UpdateScore();
            _lifeManager.IncreaseLife();
            _source.Play();
        }

        else
            _lifeManager.DecreaseLife();

        RemoveCircle();
    }        

    public void RemoveCircle()
    {
        Destroy(_circles[0]);
        _circles.RemoveAt(0);
    }

    // Used when a circle vanishes before the player
    // presses the corresponding button
    public void MissCircle()
    {
        _lifeManager.DecreaseLife();
        RemoveCircle();
    }
}
