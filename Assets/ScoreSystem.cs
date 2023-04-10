using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreSystem : MonoBehaviour
{
	const string HIGHSCORE = "highscore";
	[SerializeField] TextMeshProUGUI scoreText;
	[SerializeField] float scoreMultiplier;
	float playerScore;
	Car car;

	bool maxSpeedReached;

	private void Start()
	{
		car = FindObjectOfType<Car>();
	}

	private void FixedUpdate()
	{
		playerScore += car.Speed * scoreMultiplier * Time.fixedDeltaTime;
		scoreText.text = Mathf.RoundToInt(playerScore).ToString();
		if (!maxSpeedReached && playerScore > 5000)
		{
			maxSpeedReached = true;
			car.SetMaxSpeed();
		}
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
