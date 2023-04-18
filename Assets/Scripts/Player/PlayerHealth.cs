using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    private void Awake()
    {
        GameManager.Instance.PlayerHealth = this;
    }

    private const float invincibleTime = 3f;
    private bool _isInvincible = false;

    public void GiveInvincibility()
    {
        _isInvincible = true;
        StartCoroutine(nameof(stopInvincibilityAfter), invincibleTime);
    }

    IEnumerator stopInvincibilityAfter(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        _isInvincible = false;
        yield return 0;
    }

    public void Crash()
    {
        if (_isInvincible) return;
        Time.timeScale = 0f;
        GameManager.Instance.Crashed();
        gameObject.SetActive(false);
    }
}
