using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;

	private void Start()
	{
		highscoreText.text = $"Current Highscore: {PlayerPrefs.GetInt("highscore"), 0}";
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}
}
