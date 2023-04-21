using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] float _shrinkSpeed;

    CircleButton _circleButton;
    SpriteRenderer _sprite;
    float _shrink;
    float _fadeInSpeed = 0.03f;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        ShrinkCircle();
    }

    // Assigns a circle to the corresponding button in order
    // to remove it properly when necessary
    public void AssignButton(CircleButton circlebutton)
    {
        _circleButton = circlebutton;
    }

    // Shrinks the circle at its place until it's too small
    // which then destroys it
    private void ShrinkCircle()
    {
        _shrink = _shrinkSpeed * Time.deltaTime;

        transform.localScale -= Vector3.one * _shrink;

        if (transform.localScale.x <= 0f)
            MissCircle();
    }

    // Removes the circle and decreases life if missed
    private void MissCircle()
    {
        _circleButton.MissCircle();
    }

    // This one is used to so that spawning a circle looks smooth
    IEnumerator FadeIn()
    {
        float opacity = 0f;

        while (opacity < 1f)
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, opacity);

            opacity += _fadeInSpeed;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
