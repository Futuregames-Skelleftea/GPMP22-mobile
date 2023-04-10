using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
	const string HIGHSCORE = "highscore";
	[SerializeField] TextMeshProUGUI scoreText;
	[SerializeField] float scoreMultiplier;
	float playerScore;
	Car car;

	private void Start()
	{
		car = FindObjectOfType<Car>();
	}

	private void FixedUpdate()
	{
		playerScore += car.Speed * scoreMultiplier * Time.fixedDeltaTime;
		scoreText.text = Mathf.RoundToInt(playerScore).ToString();
	}

	public void AssignHighscore()
	{
		int currentHighscore = PlayerPrefs.GetInt(HIGHSCORE);
		if (currentHighscore < Mathf.RoundToInt(playerScore))
		{
			PlayerPrefs.SetInt("highscore", Mathf.RoundToInt(playerScore));
		}

	}
}
