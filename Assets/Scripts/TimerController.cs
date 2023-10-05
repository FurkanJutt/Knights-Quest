using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    public float timerDuration = 30f;
    private float currentTime;
    private bool isGameOver = false;

    public TextMeshProUGUI timerText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timerDuration += GetMaxTime();
        currentTime = timerDuration;
        UpdateTimerText();
    }

    public int GetMaxTime()
    {
        if (PlayerPrefs.GetInt("Time3", 0) == 1)
            return 20;
        else if (PlayerPrefs.GetInt("Time2", 0) == 1)
            return 15;
        else if (PlayerPrefs.GetInt("Time1", 0) == 1)
            return 10;
        else
            return 0;
    }

    private void Update()
    {
        if (!isGameOver)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 1)
            {
                currentTime = 0;
                isGameOver = true;

                GameManager.instance.GameLost();
            }

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        string minutesString = minutes.ToString("00");
        string secondsString = seconds.ToString("00");

        timerText.text = minutesString + ":" + secondsString;

        if (minutes == 0 && seconds == 0)
        {
            timerText.color = Color.red;
        }
    }
}
