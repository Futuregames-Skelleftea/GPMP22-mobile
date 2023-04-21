using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallShooter : MonoBehaviour
{
    [SerializeField] GameObject placeButton;
    [SerializeField] GameObject winningUi;
    [SerializeField] GameObject playModeInteractCover;
    [SerializeField] CreateBlock blockCreator;
    [SerializeField] Transform spawnPoint;
    [SerializeField] TextMeshProUGUI playButtonText;

    public bool isPlaying;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] float shootSpeed;
    private GameObject spawnedObject;

    public void PlayButtonDown()
    {
        if (!isPlaying)
        {
            StartPlaying();
        }
        else
        {
            StopPlaying();
        }
    }

    private void StartPlaying()
    {
        if(spawnedObject) { Destroy(spawnedObject); }
        spawnedObject = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        spawnedObject.GetComponent<Rigidbody2D>().velocity = -transform.up * shootSpeed;
        playButtonText.text = "STOP";
        playModeInteractCover.SetActive(true);
        spawnedObject.GetComponent<PlayerBallController>().ballShooter = this;
        isPlaying = true;
    }

    public void StopPlaying()
    {
        playModeInteractCover.SetActive(false);
        playButtonText.text = "PLAY";
        Destroy(spawnedObject);
        isPlaying = false;
    }

    public void EndGame()
    {
        winningUi.SetActive(true);
        Destroy(spawnedObject);
    }
}
