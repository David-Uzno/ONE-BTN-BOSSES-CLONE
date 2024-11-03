using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;         
    [SerializeField] TextMeshProUGUI finalTimeText;     
    [SerializeField] TextMeshProUGUI highScoreMessage;  

    float elapsedTime;
    bool isGameOver = false;
    float bestTime;

    void Start()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
    }

    void Update()
    {
        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        finalTimeText.text = "Tiempo total: " + FormatTime(elapsedTime);

        if (elapsedTime < bestTime)
        {
            bestTime = elapsedTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            highScoreMessage.text = "¡Nuevo récord!";
        }
        else
        {
            highScoreMessage.text = "No superaste el récord.";
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
